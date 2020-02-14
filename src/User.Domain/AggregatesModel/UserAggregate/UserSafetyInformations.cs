using System;
using User.Domain.AggregatesModel.Enumeration;
using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// 用户安全信息
    /// </summary>
    public class UserSafetyInformations : EntityWork<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public UserSafetyInformations()
        {
            Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            _verifyStateId = VerifyState.Pending.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">账户id</param>
        /// <param name="safetyType">类型</param>
        /// <param name="content">内容</param>
        public UserSafetyInformations(string userId, UserSafetyType safetyType, string content) : this()
        {
            UserId = userId;
            _safetyTypeId = safetyType.Id;
            Content = content;
        }

        #region 认证类型

        /// <summary>
        /// 
        /// </summary>
        private int _safetyTypeId;

        /// <summary>
        /// 类型
        /// </summary>
        public UserSafetyType SafetyType { get; private set; }

        #endregion

        /// <summary>
        /// 手机号/邮箱
        /// </summary>
        public string Content { get; private set; }

        #region 验证状态

        /// <summary>
        /// 
        /// </summary>
        private int _verifyStateId;
        
        /// <summary>
        /// 验证状态
        /// </summary>
        public VerifyState VerifyState { get; private set; }

        #endregion

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime? VerifyTime { get; private set; }

        /// <summary>
        /// 账户id
        /// </summary>
        public string UserId { get;  set; }
    }
}