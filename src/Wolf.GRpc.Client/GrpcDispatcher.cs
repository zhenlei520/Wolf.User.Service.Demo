// Copyright (c) zhenlei520 All rights reserved.

using System;
using System.Linq;
using System.Reflection;
using EInfrastructure.Core.Config.ExceptionExtensions;
using Grpc.Core;

namespace Wolf.GRpc.Client
{
    public class GrpcDispatcher
    {
        private static Assembly[] _assemblys;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">url地址</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instance<T>(string url) where T : ClientBase<T>
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var types = GetAssembly()
                .SelectMany(x => x.GetTypes().Where(y => y.GetInterfaces().Contains(typeof(IGrpcDispatcher<T>))))
                .FirstOrDefault();

            return ((IGrpcDispatcher<T>) Activator.CreateInstance(types ?? throw new BusinessException<string>("未发现服务"))
                ).Instance(url);
        }

        #region 得到程序集

        /// <summary>
        /// 得到程序集
        /// </summary>
        /// <returns></returns>
        private static Assembly[] GetAssembly()
        {
            if (_assemblys == null)
            {
                _assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            }

            return _assemblys;
        }

        #endregion
    }
}