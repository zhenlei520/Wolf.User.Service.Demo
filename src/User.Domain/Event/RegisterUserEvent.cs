using System;
using MediatR;

namespace User.Domain.Event
{
    /// <summary>
    /// 注册账户时间
    /// </summary>
    public class RegisterUserEvent : INotification
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="accountId">账户id</param>
        /// <param name="inviteAccountId">邀请人id</param>
        /// <param name="appleId">应用id</param>
        /// <param name="referer">来源地址</param>
        public RegisterUserEvent(string account,string accountId, string inviteAccountId, string appleId,
            string referer)
        {
            Account = account;
            AccountId = accountId;
            InviterUserId = inviteAccountId;
            AppleId = appleId;
            Referer = referer;
        }
        /// <summary>
        /// 账户id
        /// </summary>
        public string AccountId { get; }
        
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; }

        /// <summary>
        /// 邀请人
        /// </summary>
        public string InviterUserId { get; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppleId { get; }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string Referer { get; }
    }
}