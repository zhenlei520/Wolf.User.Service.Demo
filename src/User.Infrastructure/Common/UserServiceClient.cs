using System.Threading.Tasks;
using User.Grpc.Service;
using Wolf.GRpc.Client;

namespace User.Infrastructure.Common
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserServiceClient
    {
        #region 注册账户

        /// <summary>
        /// 注册账户
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="password">密码</param>
        /// <param name="inviteAccountId">邀请者账户id</param>
        /// <param name="appleId">应用id</param>
        /// <param name="referer">来源地址</param>
        /// <returns></returns>
        public static async Task<object> RegisterUser(string account, string password, string inviteAccountId = "",
            string appleId = "",
            string referer = "")
        {
            var client = GrpcDispatcher.Instance<Grpc.Service.Register.RegisterClient>("https://localhost:5001");
            RegisterUserRequest request = new RegisterUserRequest()
            {
                Account = account,
                Password = password
            };
            return await client.RegisterAsync(request);
            return null;
        }

        #endregion

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="id">账户id</param>
        /// <param name="nickName">昵称</param>
        /// <returns></returns>
        public static async Task<object> UpdateNickName(string id, string nickName)
        {
            var client = GrpcDispatcher.Instance<Grpc.Service.Users.UsersClient>("https://localhost:5001");
            return await client.UpdateNickNameAsync(new UpdateNickNameRequest()
            {
                Id = id,
                Nickname = nickName
            });
        }
    }
}