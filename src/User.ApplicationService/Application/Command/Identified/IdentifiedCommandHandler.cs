// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Infrastructure;
using User.ApplicationService.Infrastructure.Identified;
using User.Domain.AggregatesModel.Idempotency;

namespace User.ApplicationService.Application.Command.Identified
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R> where T : IRequest<R>
    {
        #region Ctor

        private readonly IMediator _mediator;
        private readonly ILogger<IdentifiedCommandHandler<T, R>> _logger;
        private readonly RequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestRepository requestRepository,
            ILogger<IdentifiedCommandHandler<T, R>> logger)
        {
            _mediator = mediator;
            _requestManager = new RequestManager(requestRepository);
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Method

        /// <summary>
        /// 构造默认返回值
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        /// <summary>
        /// 唯一表处理Handle
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            if (_requestManager.ExistAsync(message.Id))
            {
                return CreateResultForDuplicateRequest(); //返回默认值
            }

            try
            {
                var command = message.Command;

                var request = new ClientRequest(message.Id, command.GetGenericTypeName(), message.IdentityType,
                    global::User.Infrastructure.Core.ServiceProvider.GetJsonProvider().Serializer(command));

                // 向唯一表添加数据
                await _requestManager.CreateRequestForCommandAsync<T>(request, cancellationToken);

                //将嵌入的业务命令发送给中介，使其运行相关的命令处理程序

                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                global::User.Infrastructure.Core.ServiceProvider.GetLogService().Error($"唯一表错误：{ex}");
                return default(R);
            }
        }

        #endregion
    }
}