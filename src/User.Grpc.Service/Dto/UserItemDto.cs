// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using EInfrastructure.Core.Config.EnumerationExtensions;
using Newtonsoft.Json;

namespace User.Grpc.Service.Dto
{
    /// <summary>
    /// 用户详细信息
    /// </summary>
    public class UserItemDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "birthday")]
        public string BirthdayStr => Birthday == null ? "" : Birthday.Value.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 性别
        /// </summary>
        [JsonIgnore]
        public Gender Gender { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty(PropertyName = "gender")]
        public string GenderStr => Gender.Name;

        /// <summary>
        /// 注册时间
        /// </summary>
        [JsonProperty(PropertyName = "register_time")]
        public DateTime RegisterTime { get; set; }
    }
}