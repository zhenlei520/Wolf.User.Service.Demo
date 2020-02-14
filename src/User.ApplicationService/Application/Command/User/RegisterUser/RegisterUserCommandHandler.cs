// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.ExceptionExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Application.Command.Identified;
using User.Domain.AggregatesModel.Enumeration;
using User.Domain.AggregatesModel.Idempotency;
using User.Domain.AggregatesModel.UserAggregate;
using User.Infrastructure.Configuration.Api;
using User.Infrastructure.Configuration.Enum;

namespace User.ApplicationService.Application.Command.User.RegisterUser
{
    /// <summary>
    /// 注册账户
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AddResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? throw new BusinessException<string>("注入失败");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AddResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x =>
                x.Account == request.Account && x.UserState.Id != UserState.Logout.Id);
            if (user != null)
            {
                throw new BusinessException<string>("用户账户已存在", ErrCode.Repeat.Code);
            }

            user = new Users(request.Account, request.Password);
            _userRepository.Add(user);
            if (await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                return new AddResponse(true, user.Id);
            }
            else
            {
                return new AddResponse(false);
            }
        }
    }

    public class RegisterUserIdentifiedCommandHandler : IdentifiedCommandHandler<RegisterUserCommand, AddResponse>
    {
        public RegisterUserIdentifiedCommandHandler(IMediator mediator,
            IRequestRepository requestRepository,
            ILogger<IdentifiedCommandHandler<RegisterUserCommand, AddResponse>> logger) : base(mediator,
            requestRepository, logger)
        {
        }

        protected override AddResponse CreateResultForDuplicateRequest()
        {
            return new AddResponse(true); // Ignore duplicate requests for processing order.
        }
    }
}