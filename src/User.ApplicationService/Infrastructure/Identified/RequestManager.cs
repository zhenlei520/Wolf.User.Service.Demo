// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using User.Domain.AggregatesModel.Idempotency;

namespace User.ApplicationService.Infrastructure.Identified
{
    public class RequestManager
    {
        public RequestManager(IRequestRepository requestRepository)
        {
            RequestRepository = requestRepository;
        }

        public IRequestRepository RequestRepository;

        #region 判断是否存在记录

        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistAsync(string id)
        {
            return RequestRepository.Any(id);
        }

        #endregion

        #region 创建并且保存请求

        /// <summary>
        /// 创建并且保存请求
        /// </summary>
        /// <param name="clientRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task CreateRequestForCommandAsync<T>(ClientRequest clientRequest,
            CancellationToken cancellationToken)
        {
            await RequestRepository.AddAsync(clientRequest, cancellationToken);
        }

        #endregion
    }
}