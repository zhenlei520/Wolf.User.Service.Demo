using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Config.EnumerationExtensions;
using EInfrastructure.Core.Config.ExceptionExtensions;
using EInfrastructure.Core.Tools;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Domain.AggregatesModel.UserAggregate;
using User.Infrastructure.Configuration.Enum;
using User.Infrastructure.Configuration.Ioc;

namespace User.Grpc.Service.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : User.Grpc.Service.Users.UsersBase, IGrpcService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IQuery<Domain.AggregatesModel.UserAggregate.Users, string> _userQuery;
        private readonly IMapper _mapper;

        public UserService()
        {
            _logger = User.Infrastructure.Core.ServiceProvider.GetService<ILogger<UserService>>();
            _mediator = User.Infrastructure.Core.ServiceProvider.GetService<IMediator>();
            _userRepository = User.Infrastructure.Core.ServiceProvider.GetService<IUserRepository>();
            _userQuery = User.Infrastructure.Core.ServiceProvider
                .GetService<IQuery<Domain.AggregatesModel.UserAggregate.Users, string>>();
            _mapper = User.Infrastructure.Core.ServiceProvider.GetService<IMapper>();
        }

        /// <summary>
        /// 根据账户密码得到用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetUserReply> GetAccount(GetUserRequest request,
            ServerCallContext context)
        {
            var user = await _userQuery.GetQueryable().FirstOrDefaultAsync(x => x.Account == request.Account);
            if (user == null)
            {
                throw new BusinessException<string>("用户不存在", ErrCode.NoFind.Code);
            }

            if (user.Password != SecurityCommon.Sha256($"{request.Password}_{user.SecretKey}"))
            {
                throw new BusinessException<string>("账户密码错误", ErrCode.Error.Code);
            }

            return await Task.FromResult(new GetUserReply()
            {
                Id = user.Id,
                Account = user.Account,
                Avatar = user.Avatar ?? "",
                Name = user.Name,
                Registertime = user.RegisterTime.FormatDate(FormatDateType.One)
            });
        }

        /// <summary>
        /// 得到用户详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetUserDetailReply> Get(GetUserDetailRequest request, ServerCallContext context)
        {
            var user = await _userQuery.GetQueryable().Include(x => x.Gender).Include(x => x.UserState)
                .Where(x => x.Id == request.Id).Select(x =>
                    new
                    {
                        x.Account,
                        x.Name,
                        x.Avatar,
                        gender_desc = x.Gender.Name,
                        state_id = x.UserState.Id,
                        x.BirthDay,
                    }).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new BusinessException<string>("账户不存在");
            }

            return await Task.FromResult(new GetUserDetailReply()
            {
                Account = user.Account,
                Name = user.Name,
                Avatar = user.Avatar??"",
                Birthday = user.BirthDay?.FormatDate(FormatDateType.One) ?? "",
                State = user.state_id.ToString(),
                Gender = user.gender_desc
            });
        }
    }
}