// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace User.ApplicationService.Application.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAndSaveEventAsync(object evt, CancellationToken cancellationToken);

        ///  <summary>
        /// 添加指定的事件
        ///  </summary>
        ///  <param name="router">路由名称</param>
        ///  <param name="evt"></param>
        ///  <param name="cancellationToken"></param>
        ///  <returns></returns>
        Task AddAndSaveEventAsync(string router, object evt, CancellationToken cancellationToken);
    }
}