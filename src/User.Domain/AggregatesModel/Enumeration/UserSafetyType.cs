namespace User.Domain.AggregatesModel.Enumeration
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSafetyType : EInfrastructure.Core.Config.EntitiesExtensions.SeedWork.Enumeration
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public static UserSafetyType Phone = new UserSafetyType(1, "手机号");
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public static UserSafetyType Email=new UserSafetyType(2,"邮箱");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public UserSafetyType(int id, string name) : base(id, name)
        {
        }
    }
}