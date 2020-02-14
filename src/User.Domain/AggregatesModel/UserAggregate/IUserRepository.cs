using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;

namespace User.Domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserRepository : IRepository<Users, string>
    {
        /// <summary>
        /// 根据条件获得用户详情
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        Users Get(Expression<Func<Users, bool>> condition);

        /// <summary>
        /// 根据条件获得用户详情
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        Task<Users> GetAsync(Expression<Func<Users, bool>> condition);
    }
}