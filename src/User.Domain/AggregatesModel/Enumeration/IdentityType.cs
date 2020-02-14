namespace User.Domain.AggregatesModel.Enumeration
{
    /// <summary>
    /// 唯一键类型
    /// </summary>
    public class IdentityType : EInfrastructure.Core.Config.EntitiesExtensions.SeedWork.Enumeration
    {
        /// <summary>
        /// 消息队列
        /// </summary>
        public static IdentityType MessageQueue = new IdentityType(1, "消息队列");

        /// <summary>
        /// 普通请求消息
        /// </summary>
        public static IdentityType Request = new IdentityType(2, "普通请求");

        public IdentityType(int id, string name) : base(id, name)
        {
        }
    }
}
