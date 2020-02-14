// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Polly;

namespace User.ApplicationService.Extension
{
    public static class IWebHostExtensions
    {
        /// <summary>
        /// 注册数据库链接
        /// </summary>
        /// <param name="webHost"></param>
        /// <param name="seeder"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost,Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetService<TContext>();
                try
                {
                    var retry = Policy.Handle<MySqlException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(8),
                        });

                    /*对于网络相关的异常，迁移不会失败。
                    DbContext的重试选项只适用于暂时异常注意，这在运行某些编排器时不适用(让编排器重新创建失败的服务)*/
                    retry.Execute(() => InvokeSeeder(seeder, context, services));
                }
                catch (Exception)
                {
                }
            }

            return webHost;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}