using System;
using MediatR;

namespace User.Domain.Event
{
    /// <summary>
    /// 设置生日
    /// </summary>
    public class SetBirthDayEvent : INotification
    {
        /// <summary>
        /// 设置生日
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="accountId">账户id</param>
        /// <param name="birthday">原生日</param>
        /// <param name="birthdayOpt">新生日</param>
        public SetBirthDayEvent(string account, string accountId, DateTime? birthday, DateTime birthdayOpt)
        {
            Account = account;
            AccountId = accountId;
            Birthday = birthday;
            BirthdayOpt = birthdayOpt;
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public DateTime? Birthday { get; }

        /// <summary>
        /// 新昵称
        /// </summary>
        public DateTime BirthdayOpt { get; }

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