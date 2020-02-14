// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.AggregatesModel.Enumeration;

namespace User.Repository.MySql.Map
{
    public class VerifyStateMap : IEntityTypeConfiguration<VerifyState>
    {
        public void Configure(EntityTypeBuilder<VerifyState> builder)
        {
            builder.ToTable("wolf_verify_state");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").IsRequired().ValueGeneratedNever();

            builder.Property(t => t.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        }
    }
}
