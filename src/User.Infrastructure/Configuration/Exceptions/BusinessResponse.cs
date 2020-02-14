// Copyright (c) zhenlei520 All rights reserved.

using Newtonsoft.Json;

namespace User.Infrastructure.Configuration.Exceptions
{
    /// <summary>
    /// 业务异常响应信息
    /// </summary>
    public class BusinessResponse
    {
        /// <summary>
        /// 响应信息
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        
        /// <summary>
        /// 提示信息
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
    }
}