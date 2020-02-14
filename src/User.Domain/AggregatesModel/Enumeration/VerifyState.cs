namespace User.Domain.AggregatesModel.Enumeration
{
    /// <summary>
    /// 校验状态
    /// </summary>
    public class VerifyState : EInfrastructure.Core.Config.EntitiesExtensions.SeedWork.Enumeration
    {
        /// <summary>
        /// 待验证
        /// </summary>
        public static VerifyState Pending = new VerifyState(1, "待验证");

        /// <summary>
        /// 验证通过
        /// </summary>
        public static VerifyState Pass = new VerifyState(2, "验证通过");

        /// <summary>
        /// 验证失败
        /// </summary>
        public static VerifyState Lose = new VerifyState(3, "验证失败");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public VerifyState(int id, string name) : base(id, name)
        {
        }
    }
}