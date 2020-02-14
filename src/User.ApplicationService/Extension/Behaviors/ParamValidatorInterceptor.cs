// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EInfrastructure.Core.Validation.Common;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace User.ApplicationService.Extension.Behaviors
{
    public class ParamValidatorInterceptor:IValidatorInterceptor
    {
        public ValidationContext BeforeMvcValidation(ControllerContext controllerContext, ValidationContext validationContext)
        {
            return validationContext;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, ValidationContext validationContext,ValidationResult result)
        {
            result.Check();
            return result;
        }
    }
}
