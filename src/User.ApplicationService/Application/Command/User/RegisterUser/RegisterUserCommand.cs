// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using User.Infrastructure.Configuration.Api;

namespace User.ApplicationService.Application.Command.User.RegisterUser
{
    /// <summary>
    /// 注册账户
    /// </summary>
    public class RegisterUserCommand : IRequest<AddResponse>
    {
        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="account">账户（11位手机号）</param>
        /// <param name="password">密码（6-20位）</param>
        public RegisterUserCommand(string account, string password) : this(account, password, "", "", "")
        {
            Account = account;
            Password = password;
        }

        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="account">账户（11位手机号）</param>
        /// <param name="password">密码（6-20位）</param>
        /// <param name="inviteAccountId">邀请者账户id</param>
        /// <param name="appleId">应用id</param>
        /// <param name="referer">来源地址</param>
        public RegisterUserCommand(string account, string password, string inviteAccountId, string appleId,
            string referer)
        {
            Account = account;
            Password = password;
            InviterUserId = inviteAccountId;
            AppleId = appleId;
            Referer = referer;
        }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 邀请人
        /// </summary>
        public string InviterUserId { get; private set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppleId { get; private set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string Referer { get; private set; }
    }
}