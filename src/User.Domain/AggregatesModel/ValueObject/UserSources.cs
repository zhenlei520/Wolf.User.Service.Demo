using System.Collections.Generic;

namespace User.Domain.AggregatesModel.ValueObject
{
    /// <summary>
    /// 邀请人信息
    /// </summary>
    public class UserSources : SeedWork.ValueObject
    {
        public UserSources(){}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">邀请人id</param>
        /// <param name="appleId">应用id</param>
        /// <param name="referer">来源地址</param>
        public UserSources(string userId, string appleId, string referer)
        {
            InviterUserId = userId;
            AppleId = appleId;
            Referer = referer;
        }
        
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return InviterUserId;
            yield return AppleId;
            yield return Referer;
        }
    }
}