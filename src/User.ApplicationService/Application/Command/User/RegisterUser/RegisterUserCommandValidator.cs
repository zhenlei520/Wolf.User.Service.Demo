// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EInfrastructure.Core.Tools;
using FluentValidation;
using FluentValidation.Validators;

namespace User.ApplicationService.Application.Command.User.RegisterUser
{
    /// <summary>
    /// 注册用户
    /// </summary>
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Account).NotNull().WithMessage("请输入手机号").Must(x => x.IsMobile()).WithMessage("请输入正确的手机号");
            RuleFor(x => x.Password).NotNull().WithMessage("请输入密码")
                .SetValidator(new LengthValidator(6, 20));
        }
    }
}