// Copyright (c) zhenlei520 All rights reserved.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.ExceptionExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Infrastructure;
using User.Infrastructure.Core;

namespace User.ApplicationService.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            var failures = _validators.Select(v => v.Validate(request)).FirstOrDefault(x => x.Errors.Count > 0);

            if (failures != null)
            {
                ServiceProvider.GetLogService().Error($"交易信息导入错误，原因为：{failures.Errors.FirstOrDefault()}，交易信息为：{ServiceProvider.GetJsonProvider().Serializer(request)}");

                throw new BusinessException($"{failures.Errors.Select(x => x.ErrorMessage).FirstOrDefault()}");
            }

            return await next();
        }
    }
}
