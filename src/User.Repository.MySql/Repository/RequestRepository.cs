// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.MySql.Common;
using User.Domain.AggregatesModel.Idempotency;

namespace User.Repository.MySql.Repository
{
    public class RequestRepository : RepositoryBase<ClientRequest, string>,
        IRequestRepository, IPerRequest
    {
        public RequestRepository(IUnitOfWork context) : base(context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Any(string id)
        {
            return base.Dbcontext.Set<ClientRequest>().Any(x => x.Id == id);
        }

        /// <summary>
        /// 异步添加
        /// </summary>
        /// <param name="clientRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddAsync(ClientRequest clientRequest,CancellationToken cancellationToken)
        {
            await base.Dbcontext.Set<ClientRequest>().AddAsync(clientRequest, cancellationToken);
        }
    }
}