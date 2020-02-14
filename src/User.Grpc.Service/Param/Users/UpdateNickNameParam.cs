// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Newtonsoft.Json;
using User.Infrastructure.Extension.Validator;

namespace User.Grpc.Service.Param.Users
{
    /// <summary>
    /// 更新昵称
    /// </summary>
    public class UpdateNickNameParam : FluentlValidatorEntity
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}