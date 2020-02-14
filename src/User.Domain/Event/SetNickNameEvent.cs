using MediatR;

namespace User.Domain.Event
{
    /// <summary>
    /// 设置用户昵称
    /// </summary>
    public class SetNickNameEvent : INotification
    {
        /// <summary>
        /// 设置用户昵称
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="accountId">账户id</param>
        /// <param name="name">原性别</param>
        /// <param name="nameOpt">新性别</param>
        public SetNickNameEvent(string account,string accountId,string name,string nameOpt)
        {
            Account = account;
            AccountId = accountId;
            Name = name;
            NameOpt = nameOpt;
        }
        
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 新昵称
        /// </summary>
        public string NameOpt { get; }

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