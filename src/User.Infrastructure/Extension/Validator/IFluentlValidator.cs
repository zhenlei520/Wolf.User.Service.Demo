// Copyright (c) zhenlei520 All rights reserved.

using FluentValidation;

namespace User.Infrastructure.Extension.Validator
{
    public interface IFluentlValidator<TEntity> : IValidator
        where TEntity : IFluentlValidatorEntity
    {
    }
}
