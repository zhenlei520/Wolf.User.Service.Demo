// Copyright (c) zhenlei520 All rights reserved.

using System;

namespace User.Infrastructure.Extension.Exceptions
{
    /// <summary>
    /// 请求重复
    /// </summary>
    public class RepeatException : Exception
    {
        /// <summary>
        /// 请求重复
        /// </summary>
        /// <param name="msg"></param>
        public RepeatException(string msg = "重复请求") : base(msg)
        {
        }
    }
}