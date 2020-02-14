// Copyright (c) zhenlei520 All rights reserved.

using System;
using System.Text.RegularExpressions;
using EInfrastructure.Core.Config.EnumerationExtensions;
using EInfrastructure.Core.Config.ExceptionExtensions;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using User.Infrastructure.Core;
using User.Infrastructure.Configuration.Api;
using User.Infrastructure.Configuration.Enum;
using User.Infrastructure.Configuration.Exceptions;
using User.Infrastructure.Extension.Exceptions;

namespace User.ApplicationService.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var exceptionResponse = FormatException(context.Exception);

            var errResult = new ApiErrResult() {Code = exceptionResponse.SubCode, Msg = exceptionResponse.Msg};
            ContentResult result = new ContentResult
            {
                StatusCode = exceptionResponse.HttpStatus,
                ContentType = "application/json;charset=utf-8",
                Content = ServiceProvider.GetJsonProvider().Serializer(errResult)
            };
            context.Result = result;
            context.ExceptionHandled = true;
        }

        #region 得到Rpc的异常信息

        /// <summary>
        /// 得到Rpc的异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string GetRpcException(Exception ex)
        {
            return new Regex("BusinessException`(\\d*: )(.*)\"\\)").Match(ex.Message).Groups[2].Value;//Status(StatusCode=Unknown, Detail="Exception was thrown by handler. BusinessException`1: {"code":"The Data is Repeat","content":"用户账户已存在"}")
        }

        #endregion

        #region 处理异常信息

        /// <summary>
        /// 处理异常信息
        /// </summary>
        /// <param name="ex"></param>
        private ExceptionResponse FormatRpcException(Exception ex)
        {
            ExceptionResponse exceptionResponse;
            string message = GetRpcException(ex);
            if (!string.IsNullOrEmpty(message))
            {
                if (message == "unauthorized")
                {
                    exceptionResponse = new ExceptionResponse(HttpStatus.Unauthorized.Id, ErrCode.Unauthorized.Code,
                        ex.Message);
                }
                else
                {
                    var result = ServiceProvider.GetJsonProvider().Deserialize<BusinessResponse>(message);
                    if (result == null)
                    {
                        exceptionResponse = new ExceptionResponse(HttpStatus.Err.Id, ErrCode.SystemError.Code,
                            "系统繁忙，请稍后再试");
                    }
                    else
                    {
                        exceptionResponse = new ExceptionResponse(HttpStatus.Err.Id, result.Code, result.Content);
                    }
                }
            }
            else
            {
                exceptionResponse = new ExceptionResponse(HttpStatus.Err.Id, ErrCode.UnDisposed.Code,
                    ErrCode.UnDisposed.Name);
            }

            return exceptionResponse;
        }

        /// <summary>
        /// 格式化异常
        /// </summary>
        /// <param name="ex"></param>
        private ExceptionResponse FormatException(Exception ex)
        {
            ExceptionResponse exceptionResponse;
            if (ex is BusinessException<string>)
            {
                var data = ServiceProvider.Deserialize<dynamic>(ex.Message);
                exceptionResponse = new ExceptionResponse(HttpStatus.Err.Id,
                    data.code == null || data.code == "" ? ErrCode.SystemError.Code : data.code.ToString(),
                    data.content.ToString());
            }
            else if (ex is BusinessException)
            {
                var data = ServiceProvider.Deserialize<dynamic>(ex.Message);
                exceptionResponse = new ExceptionResponse(HttpStatus.Err.Id,
                    data.code.ToString(), data.content.ToString());
            }
            else if (ex is AuthException)
            {
                exceptionResponse = new ExceptionResponse(HttpStatus.Unauthorized.Id, ErrCode.Unauthorized.Code,
                    ex.Message);
            }
            else if (ex is RepeatException)
            {
                exceptionResponse = new ExceptionResponse(HttpStatus.Unauthorized.Id, ErrCode.Unauthorized.Code,
                    ex.Message);
            }
            else
            {
                if (ex is RpcException || ex.InnerException is RpcException)
                {
                    exceptionResponse = FormatRpcException(ex);
                }
                else
                {
                    exceptionResponse =
                        new ExceptionResponse(HttpStatus.Err.Id, ErrCode.SystemError.Code, "系统繁忙，请稍后再试");
                }

                ServiceProvider.GetLogService().Error("未捕获的异常", ex);
            }

            return exceptionResponse;
        }

        #endregion

        /// <summary>
        /// 异常信息
        /// </summary>
        private class ExceptionResponse
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="httpStatus">Http状态码</param>
            /// <param name="subCode">异常码</param>
            /// <param name="msg">异常信息</param>
            public ExceptionResponse(int httpStatus, string subCode, string msg)
            {
                HttpStatus = httpStatus;
                SubCode = subCode;
                Msg = msg;
            }

            /// <summary>
            /// Http状态码
            /// </summary>
            public int HttpStatus { get; }

            /// <summary>
            /// 异常码
            /// </summary>
            public string SubCode { get; }

            /// <summary>
            /// 异常信息
            /// </summary>
            public string Msg { get; }
        }
    }
}