// Copyright (c) zhenlei520 All rights reserved.

using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Wolf.GRpc.Client.Interceptors;

namespace Wolf.GRpc.Client.Users
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterClientDispatcher : IGrpcDispatcher<User.Grpc.Service.Users.UsersClient>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public User.Grpc.Service.Users.UsersClient Instance(string url)
        {
            var channel = GrpcChannel.ForAddress(url);
            var invoker = channel.Intercept(metadata =>
            {
                metadata ??= new Metadata();
                metadata.Add(new Metadata.Entry("RequestId", "attched"));
                return metadata;
            }).Intercept(new ClientLoggerInterceptor(), new ClientRequestInterceptor());
            var client = new User.Grpc.Service.Users.UsersClient(invoker);
            return client;
        }
    }
}