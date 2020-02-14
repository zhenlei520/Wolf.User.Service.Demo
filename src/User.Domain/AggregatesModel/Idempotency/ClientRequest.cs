﻿// Copyright (c) zhenlei520 All rights reserved.

 using System;
 using User.Domain.AggregatesModel.Enumeration;
 using User.Domain.SeedWork;

 namespace User.Domain.AggregatesModel.Idempotency
{
    /// <summary>
    /// 客户端请求
    /// </summary>
    public class ClientRequest : AggregateRootWork<string>
    {
        public ClientRequest()
        {
            Time = DateTime.Now;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="identityType"></param>
        /// <param name="content"></param>
        public ClientRequest(string id, string name, IdentityType identityType, string content) : this()
        {
            Id = id;
            Name = name;
            IdentityTypeId = identityType.Id;
            Content = content;
        }

        /// <summary>
        ///
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; private set; }

        #region 请求方式

        public int IdentityTypeId { get; private set; }

        #endregion

        /// <summary>
        ///
        /// </summary>
        public DateTime Time { get; private set; }
    }
}
