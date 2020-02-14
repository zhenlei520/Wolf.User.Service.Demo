// Copyright (c) zhenlei520 All rights reserved.

using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace User.Grpc.Service.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [Route("[controller]/[action]")]
    public class BaseController : Controller
    {
        private string _userId;

        /// <summary>
        /// 账户id
        /// </summary>
        protected string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = User.FindFirst(x => x.Type == JwtClaimTypes.Subject).Value;
                }

                return _userId;
            }
        }
    }
}