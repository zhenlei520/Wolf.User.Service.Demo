// Copyright (c) zhenlei520 All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Config.ExceptionExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Grpc.Service.Dto;
using User.Grpc.Service.Param.Users;
using User.Infrastructure.Common;
using User.Infrastructure.Configuration.Enum;

namespace User.Grpc.Service.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [Route("[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IQuery<Domain.AggregatesModel.UserAggregate.Users, string> _userQuery;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userQuery"></param>
        /// <param name="mapper"></param>
        public UserController(IQuery<Domain.AggregatesModel.UserAggregate.Users, string> userQuery, IMapper mapper)
        {
            _userQuery = userQuery;
            _mapper = mapper;
        }

        #region 注册用户

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Register([FromBody] RegisterUserParam param)
        {
            return Json(await UserServiceClient.RegisterUser(param.Account, param.Password, param.InviterUserId,
                param.AppleId, param.Referer));
        }

        #endregion

        /// <summary>
        /// 得到用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public object GetUser()
        {
            return User.Claims.ToList();
        }

        #region 得到用户详情

        /// <summary>
        /// 得到用户详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return Json(await _userQuery.GetQueryable()
                            .Where(x => x.Id == UserId)
                            .ProjectTo<UserItemDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync() ??
                        throw new BusinessException<string>("用户不存在或者已经被删除", ErrCode.NoFind.Code));
        }

        #endregion

        #region 修改用户昵称

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> UpdateNickName([FromBody] UpdateNickNameParam param)
        {
            return Json(await UserServiceClient.UpdateNickName(UserId, param.Name));
        }

        #endregion
    }
}