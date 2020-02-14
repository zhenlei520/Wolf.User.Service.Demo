// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.MySql.Repository;
using Microsoft.EntityFrameworkCore;
using User.Domain.AggregatesModel.UserAggregate;

namespace User.Repository.MySql.Repository
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserRepository : RepositoryBase<Users, string>, IUserRepository, IPerRequest
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 根据条件获得用户详情

        /// <summary>
        /// 根据条件获得用户详情
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public Users Get(Expression<Func<Users, bool>> condition)
        {
            var user = base.Dbcontext.Set<Users>().Where(condition).FirstOrDefault();
            if (user != null)
            {
                base.Dbcontext.Entry(user).Reference(x => x.Gender).Load();
                base.Dbcontext.Entry(user).Collection(x => x.UserSafetyInformationItems).Load();
                base.Dbcontext.Entry(user).Reference(x => x.UserSources).Load();
            }

            return user;
        }

        #endregion

        #region 根据条件获得用户详情

        /// <summary>
        /// 根据条件获得用户详情
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public async Task<Users> GetAsync(Expression<Func<Users, bool>> condition)
        {
            return await base.Dbcontext.Set<Users>().Where(condition)
                .Include(x => x.Gender)
                .Include(x=>x.UserState)
                .Include(x => x.UserSources)
                .Include(x => x.UserSafetyInformationItems)
                .ThenInclude(x => x.SafetyType)
                .Include(x => x.UserSafetyInformationItems)
                .ThenInclude(x => x.VerifyState)
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}