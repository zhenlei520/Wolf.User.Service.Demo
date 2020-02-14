// Copyright (c) zhenlei520 All rights reserved.

using EInfrastructure.Core.AutomationConfiguration.Interface;

namespace User.Infrastructure.Configuration
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AppConfig : ISingletonConfigModel
    {
        /// <summary>
        /// mySql connection string
        /// </summary>
        public string DbConn { get; set; }

        /// <summary>
        /// enable mysql log
        /// </summary>
        public bool EnableDebug { get; set; }

        /// <summary>
        /// 是否启用定时任务
        /// </summary>
        public bool RunTask { get; set; }
        
        /// <summary>
        /// 是否启用服务发现
        /// </summary>
        public bool ServiceDiscovery { get; set; }
    }
}