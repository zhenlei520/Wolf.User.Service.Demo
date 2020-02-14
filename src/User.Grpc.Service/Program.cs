using System;
using System.IO;
using System.Net;
using Autofac.Extensions.DependencyInjection;
using EInfrastructure.Core.Configuration.Ioc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using User.ApplicationService.Extension;
using User.Infrastructure.Configuration;
using User.Repository.MySql;

namespace User.Grpc.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var configuration = GetConfiguration();
                var host = BuildWebHost(configuration, args);

                host.MigrateDbContext<WolfDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogService>();

                    WolfDbContextSeed.SeedAsync(context, env, logger).Wait();
                });

                host.Run();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseNLog()
                .UseKestrel(SetHost)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json");
                })
                .UseNLog()
                .UseStartup<Startup>()
                .UseIISIntegration().Build();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();


            return builder.Build();
        }
        
        /// <summary>
        /// 配置Kestrel
        /// </summary>
        /// <param name="options"></param>
        private static void SetHost(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
        {
            var configuration = (IConfiguration) options.ApplicationServices.GetService(typeof(IConfiguration));
            var host = configuration.GetSection("WolfHostServiceConfig").Get<WolfHostServiceConfig>(); //依据Host类反序列化appsettings.json中指定节点
            foreach (var endpointKvp in host.Endpoints)
            {
                var endpointName = endpointKvp.Key;
                var endpoint = endpointKvp.Value; //获取appsettings.json的相关配置信息
                if (!endpoint.IsEnabled)
                {
                    continue;
                }

                var address = IPAddress.Parse(endpoint.Address);
                options.Listen(address, endpoint.Port, opt =>
                {
                    if (endpoint.Certificate != null) //证书不为空使用UserHttps
                    {
                        switch (endpoint.Certificate.Source)
                        {
                            case "File":
                                opt.UseHttps(endpoint.Certificate.Path, endpoint.Certificate.Password);
                                break;
                            case "":
                                opt.UseHttps();
                                break;
                            default:
                                throw new NotImplementedException($"文件 {endpoint.Certificate.Source}还没有实现");
                        }

                        //opt.UseConnectionLogging();
                    }
                });

                options.UseSystemd();
            }
        }

       
    }
}