using System;
using System.Collections.Generic;
using EInfrastructure.Core.Config.EnumerationExtensions;
using EInfrastructure.Core.Tools;
using User.Domain.AggregatesModel.Enumeration;
using User.Domain.AggregatesModel.ValueObject;
using User.Domain.Event;
using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class Users : AggregateRootWork<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public Users()
        {
            this.Id = Guid.NewGuid().ToString();
            this.RegisterTime = DateTime.Now;
            this._genderId = Gender.Unknow.Id;
            this._userStateId = UserState.Normal.Id;
            this._userSafetyInformationItems = new List<UserSafetyInformations>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <param name="inviteAccountId">邀请者账号</param>
        /// <param name="appleId">应用id</param>
        /// <param name="referer">来源地址</param>
        public Users(string account, string password, string inviteAccountId = "", string appleId = "",
            string referer = "") : this()
        {
            this.Account = account;
            this.SecretKey = Math.Abs($"{this.Id}_{RegisterTime.GetTimeSpan(DateTimeKind.Utc)}".GetHashCode())
                .ToString();
            this.Password = SecurityCommon.Sha256($"{password}_{SecretKey}");
            this.Name = "匿名账户" + DateTime.Now.GetTimeSpan();
            this.UserSources = new UserSources(inviteAccountId, appleId, referer);
            this.AddDomainEvent(new RegisterUserEvent(this.Account, this.Id, inviteAccountId, appleId, referer));
        }

        #region 设置用户性别

        /// <summary>
        /// 设置用户性别
        /// </summary>
        /// <param name="gender">性别</param>
        public void SetGender(Gender gender)
        {
            this.AddDomainEvent(new SetGenderEvent(this.Account, this.Id, this.Gender, gender));
            _genderId = gender.Id;
        }

        #endregion

        #region 设置用户昵称

        /// <summary>
        /// 设置用户昵称
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.AddDomainEvent(new SetNickNameEvent(this.Account, this.Id, this.Name, name));
            Name = name;
        }

        #endregion

        #region 设置生日

        /// <summary>
        /// 设置生日
        /// </summary>
        /// <param name="birthDay"></param>
        public void SetBirthDay(DateTime birthDay)
        {
            this.AddDomainEvent(new SetBirthDayEvent(this.Account, this.Id, this.BirthDay, birthDay));
            BirthDay = birthDay;
        }

        #endregion

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string Account { get; private set; }
        
        /// <summary>
        /// 头像信息
        /// </summary>
        public string Avatar { get; private set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; private set; }

        /// <summary>
        /// 账户密码
        /// </summary>
        public string Password { get; private set; }

        #region 性别

        /// <summary>
        /// 
        /// </summary>
        private int _genderId;

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; private set; }

        #endregion

        #region 用户状态

        /// <summary>
        /// 用户状态
        /// </summary>
        private int _userStateId;

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserState UserState { get; private set; }

        #endregion

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay { get; private set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; private set; }

        /// <summary>
        /// 邀请人信息
        /// </summary>
        public UserSources UserSources { get; private set; }

        #region 用户安全信息记录

        /// <summary>
        /// 用户安全信息记录
        /// </summary>
        private readonly List<UserSafetyInformations> _userSafetyInformationItems;

        public IReadOnlyCollection<UserSafetyInformations> UserSafetyInformationItems => _userSafetyInformationItems;

        #endregion
    }
}