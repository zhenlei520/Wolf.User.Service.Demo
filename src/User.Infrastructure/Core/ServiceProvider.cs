// Copyright (c) zhenlei520 All rights reserved.

using System;
using EInfrastructure.Core.AutomationConfiguration.Interface;
using EInfrastructure.Core.Config.SerializeExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.HelpCommon.Randoms;
using EInfrastructure.Core.Redis.Config;
using Microsoft.Extensions.DependencyInjection;
using User.Infrastructure.Configuration;

namespace User.Infrastructure.Core
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public class ServiceProvider
    {
        #region 接口容器

        /// <summary>
        /// 容器
        /// </summary>
        private static IServiceProvider _provider;

        #region 得到接口容器

        /// <summary>
        /// 得到接口容器
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider GetServiceProvider()
        {
            return _provider;
        }

        /// <summary>
        /// 得到接口容器
        /// </summary>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return _provider.GetService<T>();
        }

        #endregion

        #region 设置接口容器

        /// <summary>
        /// 设置接口容器
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }

        #endregion

        #endregion

        #region 随机数

        /// <summary>
        /// 随机数
        /// </summary>
        public static RandomCommon RandomCommon = new RandomCommon();

        /// <summary>
        /// 数字随机数
        /// </summary>
        public static RandomNumberGenerator RandomNumberCommon = new RandomNumberGenerator();

        #endregion

        #region 得到日志服务

        private static ILogService _logService;

        /// <summary>
        /// 得到日志服务
        /// </summary>
        /// <returns></returns>
        public static ILogService GetLogService()
        {
            return _logService ??= _provider.GetService<ILogService>();
        }

        #endregion

        #region json序列化

        private static IJsonService _jsonProvider;

        /// <summary>
        /// 得到Json序列化
        /// </summary>
        /// <returns></returns>
        public static IJsonService GetJsonProvider()
        {
            if (_jsonProvider == null)
            {
                _jsonProvider = GetServiceProvider().GetService<IJsonService>();
            }

            return _jsonProvider;
        }


        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="defaultResult">反序列化异常</param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        public static T Deserialize<T>(string s, T defaultResult = default(T), Action<Exception> action = null)
            where T : class, new()
        {
            return GetJsonProvider().Deserialize<T>(s);
        }

        #endregion

        #region 获取配置文件

        #region 得到AppConfig

        private static AppConfig _appConfig;

        /// <summary>
        /// 得到AppConfig
        /// </summary>
        /// <returns></returns>
        public static AppConfig GetAppConfig()
        {
            return _appConfig ??= GetConfig<AppConfig>();
        }

        #endregion

        #region 获取RedisConfig配置

        private static RedisConfig _redisConfig;

        /// <summary>
        /// 得到RedisConfig
        /// </summary>
        /// <returns></returns>
        public static RedisConfig GetRedisConfig()
        {
            return _redisConfig ??= GetConfig<RedisConfig>();
        }

        #endregion

        #region 获取配置文件

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T GetConfig<T>() where T : ISingletonConfigModel
        {
            var config = GetServiceProvider().GetService<T>();
            if (config == null)
            {
                GetLogService().Error($"获取{typeof(T)}配置文件失败");
            }

            return config;
        }

        #endregion

        #endregion
    }
}