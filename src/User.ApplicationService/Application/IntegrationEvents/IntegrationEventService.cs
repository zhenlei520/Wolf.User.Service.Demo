// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Infrastructure;
using User.Repository.MySql;

namespace User.ApplicationService.Application.IntegrationEvents
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegrationEventService : IIntegrationEventService
    {
        #region Ctor

        private readonly ICapPublisher _eventBus;
        private readonly WolfDbContext _dbContext;
        private readonly ILogger<IntegrationEventService> _logger;

        public IntegrationEventService(ICapPublisher eventBus, WolfDbContext dbContext,
            ILogger<IntegrationEventService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region 添加指定的事件

        /// <summary>
        /// 添加指定的事件
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddAndSaveEventAsync(object evt, CancellationToken cancellationToken)
        {
            await AddAndSaveEventAsync(evt.GetGenericTypeName(), evt, cancellationToken);
        }

        #endregion

        #endregion

        #region 添加指定的事件

        ///  <summary>
        /// 添加指定的事件
        ///  </summary>
        ///  <param name="router">路由名称</param>
        ///  <param name="evt"></param>
        ///  <param name="cancellationToken"></param>
        ///  <returns></returns>
        public async Task AddAndSaveEventAsync(string router, object evt, CancellationToken cancellationToken)
        {
            using (_dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                await _eventBus.PublishAsync(router, evt, cancellationToken: cancellationToken);
            }
        }

        #endregion
    }
}