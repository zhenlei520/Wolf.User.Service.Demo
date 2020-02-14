// Copyright (c) zhenlei520 All rights reserved.

using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;

namespace User.Domain.AggregatesModel.Idempotency
{
    public interface IRequestRepository : IRepository<ClientRequest, string>
    {
        /// <summary>
        /// string
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Any(string id);

        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="clientRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync (ClientRequest clientRequest, CancellationToken cancellationToken);
    }
}
