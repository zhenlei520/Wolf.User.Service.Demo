// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.AggregatesModel.Idempotency;

namespace User.Repository.MySql.Map
{
    public class ClientRequestMap : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> builder)
        {
            builder.ToTable("wolf_client_request");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").HasMaxLength(300);

            #region 请求方式

            builder.Property<int>(x => x.IdentityTypeId).HasColumnName("identity_type_id").IsRequired();

            #endregion

            builder.Ignore(t => t.DomainEvents);

            builder.Property(t => t.Name).HasColumnName("name").HasMaxLength(500);
            builder.Property(t => t.Content).HasColumnName("content").HasMaxLength(Int32.MaxValue);
            builder.Property(t => t.Time).HasColumnName("time");
        }
    }
}
