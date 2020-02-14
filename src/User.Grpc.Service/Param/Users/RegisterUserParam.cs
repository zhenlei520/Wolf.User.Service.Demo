using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.Tools;
using FluentValidation;
using Newtonsoft.Json;
using User.Infrastructure.Extension.Validator;

namespace User.Grpc.Service.Param.Users
{
    /// <summary>
    /// 注册用户
    /// </summary>
    public class RegisterUserParam : FluentlValidatorEntity
    {
        /// <summary>
        /// 账户
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// 邀请人
        /// </summary>
        [JsonProperty(PropertyName = "inviter_user_id")]
        public string InviterUserId { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        [JsonProperty(PropertyName = "apple_id")]
        public string AppleId { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        [JsonProperty(PropertyName = "referer")]
        public string Referer { get; set; }

        /// <summary>
        /// 校验规则
        /// </summary>
        public class RegisterUserParamValidator : AbstractValidator<RegisterUserParam>,
            IFluentlValidator<RegisterUserParam>,
            IPerRequest
        {
            public RegisterUserParamValidator()
            {
                RuleFor(x => x.Account).NotEmpty().WithMessage("请填写注册手机号").Must(x => x.IsMobile())
                    .WithMessage("请填写11位手机号");
                RuleFor(x => x.Password).NotEmpty().WithMessage("请填写账户密码");
            }
        }
    }
}