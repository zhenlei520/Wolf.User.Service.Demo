// Copyright (c) zhenlei520 All rights reserved.

using Microsoft.Extensions.DependencyInjection;

namespace User.Infrastructure
{
    public static class StartUp
    {
        #region 启用配置

        /// <summary>
        /// 启用配置
        /// </summary>
        public static IServiceCollection Run(this IServiceCollection services)
        {
            EInfrastructure.Core.StartUp.Run();
            return services;
        }

        #endregion
    }
}