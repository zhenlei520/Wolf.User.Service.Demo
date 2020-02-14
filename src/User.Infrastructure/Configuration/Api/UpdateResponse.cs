// Copyright (c) zhenlei520 All rights reserved.

using Newtonsoft.Json;

namespace User.Infrastructure.Configuration.Api
{
    /// <summary>
    /// 更新响应信息
    /// </summary>
    public class UpdateResponse
    {
        public UpdateResponse()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">状态</param>
        public UpdateResponse(bool state)
        {
            State = state;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public bool State { get; set; }
    }
}