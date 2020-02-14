// Copyright (c) zhenlei520 All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using MediatR;
using User.Domain.SeedWork;

namespace User.Repository.MySql.Extension
{
    /// <summary>
    ///
    /// </summary>
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync<T>(this IMediator mediator, WolfDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<AggregateRootWork<T>>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => { entity.Entity.ClearDomainEvents(); });

            var tasks = domainEvents
                .Select(async domainEvent =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
