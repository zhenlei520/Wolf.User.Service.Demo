// Copyright (c) zhenlei520 All rights reserved.

using System;
using EInfrastructure.Core.Config.EntitiesExtensions;

namespace User.Domain.SeedWork
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityWork<T> : Entity<T>
    {
        protected int? _requestedHashCode;
        T _Id;

        public new T Id
        {
            get { return _Id; }
            protected set { _Id = value; }
        }


        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
        protected string RandomNumber()
        {
            string random=new Random().Next(100,999).ToString();
            string time = DateTime.Now.ToString("MMddHHmmssffff");
           
            return $"{random}{time}";
        }
    }
}
