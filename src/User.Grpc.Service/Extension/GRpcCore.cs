// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Grpc.Core;

namespace User.Grpc.Service.Extension
{
    /// <summary>
    /// Grpc帮助类
    /// </summary>
    public static class GRpcCore
    {
        #region 得到请求标识

        /// <summary>
        /// 得到请求标识
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientId(this Metadata request)
        {
            return Get(request, "ClientId");
        }

        #endregion

        #region 得到请求头信息

        /// <summary>
        /// 得到请求头信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="headerKey">请求头key</param>
        /// <returns></returns>
        public static string Get(this Metadata request, string headerKey)
        {
            return request.Where(x => x.Key == headerKey).Select(x => x.Value).FirstOrDefault();
        }

        #endregion
    }
}