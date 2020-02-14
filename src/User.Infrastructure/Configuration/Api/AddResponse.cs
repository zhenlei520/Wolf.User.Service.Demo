// Copyright (c) zhenlei520 All rights reserved.

using Newtonsoft.Json;

namespace User.Infrastructure.Configuration.Api
{
    /// <summary>
    /// 添加响应信息
    /// </summary>
    public class AddResponse : UpdateResponse
    {
        public AddResponse():base(){}
        
        /// <summary>
        /// 添加响应信息
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="extend">扩展信息</param>
        public AddResponse(bool state, string extend = "") : base(state)
        {
            Extend = extend;
        }

        /// <summary>
        /// 扩展信息
        /// </summary>
        [JsonProperty(PropertyName = "extend", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Extend { get; set; }
    }
}