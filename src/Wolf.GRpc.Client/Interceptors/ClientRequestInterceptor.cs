// Copyright (c) zhenlei520 All rights reserved.

using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Wolf.GRpc.Client.Interceptors
{
    /// <summary>
    /// 客户端请求调用
    /// </summary>
    public class ClientRequestInterceptor : Interceptor
    {
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
            LogCall(context.Method);
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
            LogCall(context.Method);
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
            LogCall(context.Method);
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
            LogCall(context.Method);
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
            LogCall(context.Method);
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
        //     return continuation(request, context);
        // }
        //
        // #endregion
        //
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
        //     return continuation(requestStream, responseStream, context);
        // }
        //
        // #endregion

        #region private methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method)
            where TRequest : class
            where TResponse : class
        {
        }

        #endregion
    }
}