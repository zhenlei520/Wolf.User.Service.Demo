using System;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EInfrastructure.Core.AutomationConfiguration;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.ServiceDiscovery.Consul.AspNetCore;
using EInfrastructure.Core.Tools;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using User.ApplicationService.Extension;
using User.ApplicationService.Extension.Timer;
using User.ApplicationService.Infrastructure.AutofacModules;
using User.ApplicationService.Infrastructure.Filters;
using User.Grpc.Service.Extension.AutoConversionProfile;
using User.Grpc.Service.Extension.Interceptors;
using User.Grpc.Service.Extension.Router;
using User.Infrastructure.Configuration;
using User.Infrastructure.Configuration.Ioc;
using User.Repository.MySql;

namespace User.Grpc.Service
{
    public class Startup
    {
        /// <summary>
        /// 环境信息
        /// </summary>
        private readonly string environmentName;

        private IServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            Console.WriteLine("环境：" + hostEnvironment.EnvironmentName);
            environmentName = hostEnvironment.EnvironmentName;
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
                options.Interceptors.Add<ClientRequestInterceptor>();
                options.Interceptors.Add<ServerLoggerInterceptor>();
            });
            services.AddRouting(options => options.LowercaseUrls = true);

            #region 启用压缩与缓存

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Append("image/svg+xml");
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCaching();

            #endregion

            services.AddControllers(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<MediatorModule>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = true;
                    ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
                });

            Assembly assembly = typeof(Startup).Assembly;

            services
                .AddCorsExt()
                .AddConsulServer(Configuration)
                .AddRelyModule()
                .AddCustomDbContext(Configuration, assembly)
                .AddCustomIntegrations()
                .AddIntegrationEventHandler()
                .AddCustomConfiguration(Configuration)
                .AddEventBus(Configuration)
                .AddDomainEventHandler()
                .AddCacheService(Configuration)
                .AddMonitoringService(Configuration)
                .AddTimer(Configuration)
                .AddAutoConversion(() => { AutoMapperConfiguration.Configure(); })
                .AddAutoConfig(Configuration, $"appsettings.{environmentName}.json");

            #region IdentityServer4

            services.AddAuthorization();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetSection("WolfHostServiceConfig").Get<WolfHostServiceConfig>().Service
                        .Authentication;
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "user_api";
                });

            #endregion

            try
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.Populate(services);
                containerBuilder.RegisterType<WolfDbContext>().As<IUnitOfWork>().InstancePerLifetimeScope();
                containerBuilder.RegisterModule(new MediatorModule());
                containerBuilder.RegisterModule(new ApplicationModule());

                var container = containerBuilder.Build();

                Infrastructure.Core.ServiceProvider.SetServiceProvider(container.Resolve<IServiceProvider>());
                var provider = new AutofacServiceProvider(container);
                
                //设置容器
                return provider;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env,
            Microsoft.AspNetCore.Hosting.IApplicationLifetime applicationLifetime)
        {
            //nlog
            loggerFactory.AddNLog();

            #region 启用压缩与缓存

            app.UseResponseCompression();
            app.UseResponseCaching();

            #endregion

            if (Configuration.GetSection("AppConfig")["ServiceDiscovery"].ConvertToBool(false))
            {
                app.UseConsul(applicationLifetime.ApplicationStopped);
            }

            if (env.IsDevelopment() || env.EnvironmentName == "Debug")
            {
                app.UseDeveloperExceptionPage();
            }

            //如果当前时开发者模式
            if (env.IsDevelopment())
            {
                //从管道中捕获同步和异步System.Exception实例并生成HTML错误响应。
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //跨域、启用定时任务
            app.UseCustomerCors();

            app.UseAuthentication(); //认证
            app.UseAuthorization(); //授权

            app.UseTimer();

            app.UseEndpoints(endpoints =>
            {
                Type workerType = typeof(Microsoft.AspNetCore.Builder.GrpcEndpointRouteBuilderExtensions);
                MethodInfo staticDoWorkMethod = workerType.GetMethod("MapGrpcService");
                new EInfrastructure.Core.Tools.Component.ServiceProvider().GetServices<IGrpcService>().ToList().ForEach(
                    type =>
                    {
                        if (staticDoWorkMethod != null && type.GetType().IsClass && !type.GetType().IsAbstract)
                        {
                            MethodInfo curMethod = staticDoWorkMethod.MakeGenericMethod(type.GetType());
                            curMethod.Invoke(null, new[] {endpoints}); //Static method
                        }
                    });

                endpoints.MapGet("/Check/Healthy",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}", null,
                        new LowercaseRouteConstraint())
                    .RequireAuthorization();
            });

            BaseServiceProvider.Configuration();
        }
    }
}