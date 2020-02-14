// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace User.Grpc.Service.Extension.Router
{
    /// <summary>
    /// 小写路由
    /// </summary>
    public class LowercaseRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="route"></param>
        /// <param name="routeKey"></param>
        /// <param name="values"></param>
        /// <param name="routeDirection"></param>
        /// <returns></returns>
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (httpContext.Request.Query != null)
            {
                var path = httpContext.Request.Query.ToString();
                return path.Equals(path.ToLowerInvariant(), StringComparison.InvariantCulture);
            }

            return true;
        }
    }
}