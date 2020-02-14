// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Reflection;
using AutoMapper;
using EInfrastructure.Core.Redis;
using EInfrastructure.Core.ServiceDiscovery.Consul.AspNetCore;
using EInfrastructure.Core.ServiceDiscovery.Consul.AspNetCore.Config;
using EInfrastructure.Core.Tools;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Redis;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using User.ApplicationService.Application.IntegrationEvents;
using User.ApplicationService.Extension.Behaviors;
using User.Repository.MySql;

namespace User.ApplicationService.Extension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static partial class WolfExtensionsMethods
    {
        #region 添加自定义Cors规则

        /// <summary>
        /// 添加自定义Cors规则
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsExt(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("WolfSiteCorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(host => true)
                        .WithHeaders("Content-Type", "Authorization", "X-Expansion-Fields")
                        .WithMethods("GET", "POST", "OPTIONS", "PUT", "PATCH", "DELETE")
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromHours(12)));
            });

            return services;
        }

        #endregion

        #region 添加自定义数据库链接

        /// <summary>
        /// 添加自定义数据库链接
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
            IConfiguration configuration, Assembly assembly)
        {
            Console.WriteLine("数据库配置：" + configuration.GetSection("AppConfig")["DbConn"]);
            services.AddEntityFrameworkMySql().AddDbContext<WolfDbContext>(options =>
                {
                    options.UseMySql(configuration.GetSection("AppConfig")["DbConn"],
                        mysqlOptions =>
                        {
                            mysqlOptions.MigrationsAssembly(assembly.GetName().Name);
                            mysqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(60), null);
                            mysqlOptions.CommandTimeout(120);
                            mysqlOptions.MaxBatchSize(100);
                        });
                },
                ServiceLifetime
                    .Scoped //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );
            return services;
        }

        #endregion

        #region 添加领域事务服务

        /// <summary>
        /// 添加领域事务服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IIntegrationEventService, IntegrationEventService>();
            return services;
        }

        #endregion

        #region 添加自定义配置

        /// <summary>
        /// 添加自定义配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = {"application/problem+json", "application/problem+xml"}
                    };
                };
            });
            return services;
        }

        #endregion

        #region 添加事件处理器

        /// <summary>
        /// 添加事件处理器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIntegrationEventHandler(this IServiceCollection services)
        {
//            services.AddTransient<PayNoticeIntegrationEventHandler>();
            return services;
        }

        #endregion

        #region 添加领域事件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainEventHandler(this IServiceCollection services)
        {
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        #endregion

        #region 添加事件Bus

        /// <summary>
        /// 添加事件Bus
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(options =>
            {
                options.UseMySql(configuration.GetSection("AppConfig")["DbConn"]);
                options.UseRabbitMQ(conf =>
                {
                    conf.HostName = configuration.GetSection("RabbitMqConfig")["HostName"];
                    conf.Port = configuration.GetSection("RabbitMqConfig")["Port"].ConvertToInt(0);
                    conf.UserName = configuration.GetSection("RabbitMqConfig")["UserName"];
                    conf.Password = configuration.GetSection("RabbitMqConfig")["Password"];
                    conf.VirtualHost = configuration.GetSection("RabbitMqConfig")["VirtualHost"];
                });

                if (!string.IsNullOrEmpty(configuration.GetSection("RabbitMqConfig")["SubscriptionClientName"]))
                {
                    options.DefaultGroup = configuration.GetSection("RabbitMqConfig")["SubscriptionClientName"];
                }

                options.FailedRetryCount =
                    configuration.GetSection("RabbitMqConfig")["FailedRetryCount"].ConvertToInt(50);
                options.SucceedMessageExpiredAfter =
                    configuration.GetSection("RabbitMqConfig")["SucceedMessageExpiredAfter"].ConvertToInt(45);
                options.FailedRetryInterval =
                    configuration.GetSection("RabbitMqConfig")["FailedRetryInterval"].ConvertToInt(60);
                options.FailedThresholdCallback =
                    (type,  content) =>
                    {
                        User.Infrastructure.Core.ServiceProvider.GetLogService().Error("CAP异常",
                            "MessageType：" + type  + "\n" + "Content：" + content);
                    };

                // options.UseDashboard(opt =>
                // {
                //     opt.Authorization = new[]
                //     {
                //         new CapAuthorizationFilter()
                //     };
                // });
            });

            return services;
        }

        #endregion

        #region 添加项目依赖

        /// <summary>
        /// 添加项目依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRelyModule(this IServiceCollection services)
        {
            User.Infrastructure.StartUp.Run(services);
            return services;
        }

        #endregion

        #region 添加Redis配置

        /// <summary>
        /// 添加Redis配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRedis(option =>
            {
                option.Ip = configuration.GetSection("RedisConfig")["Ip"];
                option.Port = configuration.GetSection("RedisConfig")["Port"].ConvertToInt(0);
                option.DataBase = configuration.GetSection("RedisConfig")["DataBase"].ConvertToInt(0);
                option.Name = configuration.GetSection("RedisConfig")["Name"];
                option.Password = configuration.GetSection("RedisConfig")["Password"];
                option.PoolSize = configuration.GetSection("RedisConfig")["PoolSize"].ConvertToInt(0);
            });
            return services;
        }

        #endregion

        #region 添加监控服务

        /// <summary>
        /// 添加监控服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMonitoringService(this IServiceCollection services,
            IConfiguration configuration)
        {
//            services.AddSkyWalking(option =>
//                {
//                    option.ApplicationCode = "Trade";
//
//                    option.DirectServers = "SkyWalkingServer:11800";
//                })
//                .AddHttpClient()
//                .AddEntityFrameworkCore(c => c.AddPomeloMysql())
//                .AddCap();
            return services;
        }

        #endregion

        #region 添加定时器任务

        /// <summary>
        /// 添加定时器任务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddTimer(this IServiceCollection services, IConfiguration configuration)
        {
            string redisConfig =
                $"{configuration.GetSection("RedisConfig")["Ip"]}:{configuration.GetSection("RedisConfig")["Port"]},password={configuration.GetSection("RedisConfig")["Password"]},defaultDatabase=10";
            services.AddHangfire(config =>
                config.UseRedisStorage(ConnectionMultiplexer.Connect(
                        redisConfig),
                    new RedisStorageOptions
                    {
                        Prefix = "hangfire:",
                        InvisibilityTimeout = TimeSpan.FromMinutes(30),
                        Db = 15
                    }));
//            services.AddHangfire(t => t.UseStorage(new MySqlStorage(
//                configuration.GetSection("AppConfig")["DbConn"] + "Allow User Variables=true;",
//                new MySqlStorageOptions
//                {
//                    TransactionIsolationLevel = System.Data.IsolationLevel.ReadCommitted,
//                    QueuePollInterval = TimeSpan.FromSeconds(15),
//                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
//                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
//                    PrepareSchemaIfNecessary = true,
//                    DashboardJobListLimit = 50000,
//                    TransactionTimeout = TimeSpan.FromMinutes(10),
//                })));
            return services;
        }

        #endregion

        #region 注册自动映射服务

        /// <summary>
        /// 注册自动映射服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        public static IServiceCollection AddAutoConversion(this IServiceCollection services, Action action)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            action?.Invoke();
            return services;
        }

        #endregion

        #region 注册Consul服务

        /// <summary>
        /// 注册Consul服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsulServer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetSection("AppConfig")["ServiceDiscovery"].ConvertToBool(false))
            {
                services.AddConsul(configs =>
                {
                    var config = new ConsulConfig
                    {
                        ApiServiceConfig = new ApiServiceConfig()
                        {
                            Id = configuration.GetSection("Consul")["ClientId"],
                            Name = configuration.GetSection("Consul")["ClientName"],
                            Ip = configuration.GetSection("Consul")["ClientIp"],
                            Port = configuration.GetSection("Consul")["ClientPort"].ConvertToInt(0),
                            Tags = configuration.GetSection("Consul")["ClientTags"]?.ConvertStrToList<string>()
                                       .ToArray() ??
                                   new string[0],
                            Datacenter = configuration.GetSection("Consul")["Datacenter"],
                            Token = configuration.GetSection("Consul")["Token"]
                        },
                        ConsulClientAddress = configuration.GetSection("Consul")["Service"],
                        ApiServiceHealthyConfig = new ApiServiceHealthyConfig()
                        {
                            CheckApi = configuration.GetSection("Consul")["CheckApi"],
                            RegisterTime = configuration.GetSection("Consul")["CheckRegisterTime"].ConvertToInt(0),
                            CheckIntervalTime =
                                configuration.GetSection("Consul")["CheckIntervalTime"].ConvertToInt(0),
                            TimeOutTime = configuration.GetSection("Consul")["TimeOutTime"].ConvertToInt(0)
                        }
                    };
                    configs.Add(config);
                });
            }

            return services;
        }

        #endregion

        #region 注册参数校验

        /// <summary>
        /// 注册参数校验
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddParamVerify(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IValidatorInterceptor, ParamValidatorInterceptor>();
            return serviceCollection;
        }

        #endregion
    }

    /// <summary>
    /// 客户扩展
    /// </summary>
    public static partial class WolfExtensionsMethods
    {
        #region 使用自定义跨域配置

        /// <summary>
        /// 使用自定义跨域配置
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomerCors(this IApplicationBuilder app)
        {
            app.UseCors("WolfSiteCorsPolicy");
            return app;
        }

        #endregion

        #region 启用定时任务

        /// <summary>
        /// 启用定时任务
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTimer(this IApplicationBuilder app)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = Math.Max(Environment.ProcessorCount, 20)
            });
            return app;
        }

        #endregion
    }
}