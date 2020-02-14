// Copyright (c) zhenlei520 All rights reserved.

using System.Collections.Generic;
using EInfrastructure.Core.AutomationConfiguration.Interface;

namespace User.Infrastructure.Configuration
{
    /// <summary>
    /// 域配置
    /// </summary>
    public class WolfHostServiceConfig : ISingletonConfigModel
    {
        /// <summary>
        /// appsettings.json字典
        /// </summary>
        public Dictionary<string, Endpoint> Endpoints { get; set; }

        /// <summary>
        /// 服务地址配置
        /// </summary>
        public ServiceConfig Service { get; set; }

        /// <summary>
        /// 终结点
        /// </summary>
        public class Endpoint : ISingletonConfigModel
        {
            /// <summary>
            /// 是否启用
            /// </summary>
            public bool IsEnabled { get; set; }

            /// <summary>
            /// ip地址
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 端口号
            /// </summary>
            public int Port { get; set; }

            /// <summary>
            /// 证书
            /// </summary>
            public Certificate Certificate { get; set; }
        }

        /// <summary>
        /// 证书类
        /// </summary>
        public class Certificate : ISingletonConfigModel
        {
            /// <summary>
            /// 源
            /// </summary>
            public string Source { get; set; }

            /// <summary>
            /// 证书路径()
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// 证书密钥
            /// </summary>
            public string Password { get; set; }
        }

        /// <summary>
        /// 服务配置
        /// </summary>
        public class ServiceConfig : ISingletonConfigModel
        {
            /// <summary>
            /// 认证中心地址
            /// </summary>
            public string Authentication { get; set; }
        }
    }
}