// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Infrastructure;
using User.Infrastructure.Core;
using User.Repository.MySql;

namespace User.ApplicationService.Application.Behaviors
{
    /// <summary>
    /// 用户服务
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        #region  构造函数

        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly WolfDbContext _dbContext;

        public TransactionBehaviour(IUnitOfWork unitOfWork, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = unitOfWork as WolfDbContext ?? throw new ArgumentException(nameof(WolfDbContext));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        #endregion

        /// <summary>
        /// 交易处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">事务标识</param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            //类名
            var typeName = request.GetGenericTypeName();
            TResponse response = default(TResponse);
            try
            {
                //如果开启事务
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.BeginTransactionAsync(cancellationToken))
                    {
                        response = await next();

                        await _dbContext.CommitTransactionAsync(transaction);
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                ServiceProvider.GetLogService()
                    .Error(
                        $"信息导入错误，原因为：{ex}，类名:{typeName}，信息为：{ServiceProvider.GetJsonProvider().Serializer(request)}");
                throw ex;
            }
        }
    }
}