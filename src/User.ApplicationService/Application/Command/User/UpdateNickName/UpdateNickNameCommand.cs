// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace User.ApplicationService.Application.Command.User.UpdateNickName
{
    /// <summary>
    /// 修改昵称
    /// </summary>
    public class UpdateNickNameCommand : IRequest<bool>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; private set; }
    }
}