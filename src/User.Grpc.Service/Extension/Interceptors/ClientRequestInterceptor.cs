// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using EInfrastructure.Core.Config.ExceptionExtensions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using User.ApplicationService.Infrastructure;
using User.Domain.AggregatesModel.Enumeration;
using User.Domain.AggregatesModel.Idempotency;
using User.Infrastructure.Core;
using User.Infrastructure.Extension.Exceptions;

namespace User.Grpc.Service.Extension.Interceptors
{
    /// <summary>
    /// 客户端请求调用
    /// </summary>
    public class ClientRequestInterceptor : Interceptor
    {
        private readonly IRequestRepository _requestRepository;

        public ClientRequestInterceptor(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        #region 拦截阻塞调用

        /// <summary>
        /// 拦截阻塞调用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var clientId = context.Options.Headers?.GetClientId() ?? "";
            CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(request), request.GetGenericTypeName());
            return continuation(request, context);
        }

        #endregion

        #region 拦截异步调用

        /// <summary>
        /// 拦截异步调用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        /// <exception cref="AuthException"></exception>
        /// <exception cref="RepeatException"></exception>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var clientId = context.Options.Headers?.GetClientId() ?? "";
            CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(request), request.GetGenericTypeName());
            return continuation(request, context);
        }

        #endregion

        #region 拦截异步服务端流调用

        /// <summary>
        /// 拦截异步服务端流调用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
            TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var clientId = context.Options.Headers?.GetClientId() ?? "";
            CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(request), request.GetGenericTypeName());
            return continuation(request, context);
        }

        #endregion

        #region 拦截异步客户端流调用

        /// <summary>
        /// 拦截异步客户端流调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var clientId = context.Options.Headers?.GetClientId() ?? "";
            CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(context), context.GetGenericTypeName());
            return continuation(context);
        }

        #endregion

        #region 拦截异步双向流调用

        /// <summary>
        /// 拦截异步双向流调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            var clientId = context.Options.Headers?.GetClientId() ?? "";
            CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(context), context.GetGenericTypeName());
            return continuation(context);
        }

        #endregion

        // #region 用于拦截和传入普通调用服务器端处理程序
        //
        // /// <summary>
        // /// 用于拦截和传入普通调用服务器端处理程序
        // /// </summary>
        // /// <param name="request"></param>
        // /// <param name="context"></param>
        // /// <param name="continuation"></param>
        // /// <typeparam name="TRequest"></typeparam>
        // /// <typeparam name="TResponse"></typeparam>
        // /// <returns></returns>
        // public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        //     ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        // {
        //     var clientId = context.RequestHeaders?.GetClientId() ?? "";
        //     CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(request), request.GetGenericTypeName());
        //     return continuation(request, context);
        // }
        //
        // #endregion
        
        // #region 用于拦截客户端流调用的服务器端处理程序
        //
        // /// <summary>
        // /// 用于拦截客户端流调用的服务器端处理程序
        // /// </summary>
        // /// <param name="requestStream"></param>
        // /// <param name="context"></param>
        // /// <param name="continuation"></param>
        // /// <typeparam name="TRequest"></typeparam>
        // /// <typeparam name="TResponse"></typeparam>
        // /// <returns></returns>
        // public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        //     IAsyncStreamReader<TRequest> requestStream, ServerCallContext context,
        //     ClientStreamingServerMethod<TRequest, TResponse> continuation)
        // {
        //     var clientId = context.RequestHeaders?.GetClientId() ?? "";
        //     CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(context), context.GetGenericTypeName());
        //     return continuation(requestStream, context);
        // }
        //
        // #endregion
        //
        // #region 用于拦截服务端流调用的服务器端处理程序
        //
        // /// <summary>
        // /// 用于拦截服务端流调用的服务器端处理程序
        // /// </summary>
        // /// <param name="request"></param>
        // /// <param name="responseStream"></param>
        // /// <param name="context"></param>
        // /// <param name="continuation"></param>
        // /// <typeparam name="TRequest"></typeparam>
        // /// <typeparam name="TResponse"></typeparam>
        // /// <returns></returns>
        // public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request,
        //     IServerStreamWriter<TResponse> responseStream, ServerCallContext context,
        //     ServerStreamingServerMethod<TRequest, TResponse> continuation)
        // {
        //     var clientId = context.RequestHeaders?.GetClientId() ?? "";
        //     CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(request), request.GetGenericTypeName());
        //     return continuation(request, responseStream, context);
        // }
        //
        // #endregion
        //
        // #region 用于拦截双向流调用的服务器端处理程序
        //
        // /// <summary>
        // /// 用于拦截双向流调用的服务器端处理程序
        // /// </summary>
        // /// <param name="requestStream"></param>
        // /// <param name="responseStream"></param>
        // /// <param name="context"></param>
        // /// <param name="continuation"></param>
        // /// <typeparam name="TRequest"></typeparam>
        // /// <typeparam name="TResponse"></typeparam>
        // /// <returns></returns>
        // public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
        //     IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream,
        //     ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        // {
        //     var clientId = context.RequestHeaders?.GetClientId() ?? "";
        //     CheckRepeat(clientId, ServiceProvider.GetJsonProvider().Serializer(context), context.GetGenericTypeName());
        //     return continuation(requestStream, responseStream, context);
        // }
        //
        // #endregion

        #region private methods

        #region 查重复

        /// <summary>
        /// 查重复
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="content">内容</param>
        /// <param name="name"></param>
        /// <exception cref="AuthException"></exception>
        /// <exception cref="RepeatException"></exception>
        private void CheckRepeat(string clientId, string content, string name)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new AuthException();
            }

            if (_requestRepository.Any(clientId))
            {
                throw new RepeatException();
            }

            try
            {
                _requestRepository.AddAsync(new ClientRequest(clientId, name, IdentityType.Request, content),CancellationToken.None);
            }
            catch (Exception ex)
            {
                ServiceProvider.GetLogService().Error($"重复校验失败：" + ex);
            }
        }

        #endregion

        #endregion
    }
}