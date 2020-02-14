using Newtonsoft.Json;

namespace User.Infrastructure.Configuration.Enum
{
    /// <summary>
    /// 错误码
    /// </summary>
    public sealed class ErrCode
    {
        /// <summary>
        /// 未经授权的
        /// </summary>
        public static ErrCode Unauthorized = new ErrCode("Unauthorized", "未授权的凭证");

        /// <summary>
        /// 系统错误
        /// </summary>
        public static ErrCode SystemError = new ErrCode("SystemError", "系统繁忙，请稍后再试");

        /// <summary>
        /// 数据已存在
        /// </summary>
        public static ErrCode Repeat = new ErrCode("The Data is Repeat", "数据已存在");

        /// <summary>
        /// 未处理的异常
        /// </summary>
        public static ErrCode UnDisposed = new ErrCode("UnDisposed", "未处理的异常");

        /// <summary>
        /// 未处理的异常
        /// </summary>
        public static ErrCode NoFind = new ErrCode("The Data is NoFind", "数据不存在");

        /// <summary>
        /// 检查数据
        /// </summary>
        public static ErrCode Error = new ErrCode("Check Data", "检查数据");

        /// <summary>
        /// 错误码
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="name">描述</param>
        public ErrCode(string code, string name)
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// 错误码描述
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}