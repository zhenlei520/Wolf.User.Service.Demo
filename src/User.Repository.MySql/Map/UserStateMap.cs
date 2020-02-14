// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.AggregatesModel.Enumeration;

namespace User.Repository.MySql.Map
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class UserStateMap : IEntityTypeConfiguration<UserState>
    {
        public void Configure(EntityTypeBuilder<UserState> builder)
        {
            builder.ToTable("wolf_user_state");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").IsRequired().ValueGeneratedNever();

            builder.Property(t => t.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        }
    }
}