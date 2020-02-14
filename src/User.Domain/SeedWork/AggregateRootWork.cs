// Copyright (c) zhenlei520 All rights reserved.

using System;
using System.Collections.Generic;
using EInfrastructure.Core.Config.EntitiesExtensions;
using MediatR;

namespace User.Domain.SeedWork
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AggregateRootWork<T> : EntityWork<T>, IAggregateRoot<T>
    {
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public bool IsTransient()
        {
            return this.Id.Equals(default(T));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is AggregateRootWork<T>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            AggregateRootWork<T> item = (AggregateRootWork<T>) obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            return this.Id.Equals(default(T));
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode =
                        this.Id.GetHashCode() ^
                        31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(AggregateRootWork<T> left, AggregateRootWork<T> right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(AggregateRootWork<T> left, AggregateRootWork<T> right)
        {
            return !(left == right);
        }
    }
}
