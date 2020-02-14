// Copyright (c) zhenlei520 All rights reserved.

namespace Wolf.GRpc.Client
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGrpcDispatcher<T>
    {
        #region 得到用户服务

        /// <summary>
        /// 得到用户服务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        T Instance(string url);

        #endregion
    }
}