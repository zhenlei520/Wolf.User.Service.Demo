// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.MySql.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace User.ApplicationService.Extension.Timer
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseServiceProvider
    {
        public static void Configuration()
        {
            JobBase.StartJob();
        }

        #region 创建容器

        /// <summary>
        /// 获取容器
        /// </summary>
        /// <returns></returns>
        protected static IServiceProvider BuildProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped(t => User.Infrastructure.Core.ServiceProvider.GetAppConfig());
            services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            services.AddScoped(typeof(IQuery<,>), typeof(QueryBase<,>));
            services.AddScoped(typeof(IRepository<,,>), typeof(RepositoryBase<,,>));
            services.AddScoped(typeof(IQuery<,,>), typeof(QueryBase<,,>));
            return services.BuildServiceProvider();
        }

        #endregion
    }
}