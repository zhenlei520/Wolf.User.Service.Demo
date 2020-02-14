using EInfrastructure.Core.Config.EnumerationExtensions;
using MediatR;

namespace User.Domain.Event
{
    /// <summary>
    /// 设置性别
    /// </summary>
    public class SetGenderEvent : INotification
    {
        /// <summary>
        /// 设置性别
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="accountId">账户id</param>
        /// <param name="gender">原性别</param>
        /// <param name="genderOpt">新性别</param>
        public SetGenderEvent(string account,string accountId,Gender gender,Gender genderOpt)
        {
            Account = account;
            AccountId = accountId;
            Gender = gender;
            GenderOpt = genderOpt;
        }
        
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; }

        /// <summary>
        /// 新性别
        /// </summary>
        public Gender GenderOpt { get; }

        /// <summary>
        /// 账户id
        /// </summary>
        public string AccountId { get; }
        
        /// <summary>
        /// 账户名
        /// </summary>
        public string Account { get; }
    }
}