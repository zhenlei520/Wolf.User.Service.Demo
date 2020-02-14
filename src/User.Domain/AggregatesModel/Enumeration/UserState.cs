// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace User.Domain.AggregatesModel.Enumeration
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class UserState : EInfrastructure.Core.Config.EntitiesExtensions.SeedWork.Enumeration
    {
        /// <summary>
        /// 正常
        /// </summary>
        public static UserState Normal = new UserState(0, "正常");

        /// <summary>
        /// 封禁
        /// </summary>
        public static UserState Forbidden = new UserState(1, "封禁");

        /// <summary>
        /// 注销
        /// </summary>
        public static UserState Logout = new UserState(2, "注销");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public UserState(int id, string name) : base(id, name)
        {
        }
    }
}