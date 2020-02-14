using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EInfrastructure.Core.Config.EntitiesExtensions;
using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.ApplicationService.Application.Command.User.RegisterUser;
using User.Domain.AggregatesModel.UserAggregate;
using User.Infrastructure.Configuration.Ioc;

namespace User.Grpc.Service.Services
{
    /// <summary>
    /// 注册账户
    /// </summary>
    public class RegisterService : global::User.Grpc.Service.Register.RegisterBase, IGrpcService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IQuery<Domain.AggregatesModel.UserAggregate.Users, string> _userQuery;
        private readonly IMapper _mapper;

        public RegisterService()
        {
            _logger = User.Infrastructure.Core.ServiceProvider.GetService<ILogger<UserService>>();
            _mediator = User.Infrastructure.Core.ServiceProvider.GetService<IMediator>();
            _userRepository = User.Infrastructure.Core.ServiceProvider.GetService<IUserRepository>();
            _userQuery = User.Infrastructure.Core.ServiceProvider
                .GetService<IQuery<Domain.AggregatesModel.UserAggregate.Users, string>>();
            _mapper = User.Infrastructure.Core.ServiceProvider.GetService<IMapper>();
        }

        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<RegisterUserReply> Register(RegisterUserRequest request, ServerCallContext context)
        {
            RegisterUserCommand command = new RegisterUserCommand(request.Account, request.Password);
            var result = await _mediator.Send(command);
            if (result.State)
            {
                return await _userQuery.GetQueryable().Where(x => x.Id == result.Extend)
                    .ProjectTo<RegisterUserReply>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            }
        
            throw new RpcException(Status.DefaultCancelled, "注册失败");
        }
        
        public override async Task RegisterStream(IAsyncStreamReader<RegisterUserRequest> requestStream,
            IServerStreamWriter<RegisterUserReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var note = requestStream.Current;
        
                await responseStream.WriteAsync(new RegisterUserReply {Gender = "1"});
            }
        }
    }
}