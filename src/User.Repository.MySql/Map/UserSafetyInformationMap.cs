// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.AggregatesModel.UserAggregate;

namespace User.Repository.MySql.Map
{
    /// <summary>
    /// 安全信息
    /// </summary>
    public class UserSafetyInformationMap : IEntityTypeConfiguration<UserSafetyInformations>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UserSafetyInformations> builder)
        {
            builder.ToTable("wolf_user_safety_information");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasMaxLength(50).HasColumnName("id").IsRequired();

            //认证类型 builder
            builder.Property<int>("_safetyTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("safety_type")
                .IsRequired();
            builder.HasOne(t => t.SafetyType).WithMany().HasForeignKey("_safetyTypeId");

            builder.Property(t => t.Content).HasMaxLength(30).HasColumnName("content").IsRequired();

            //验证状态
            builder.Property<int>("_verifyStateId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("verify_state")
                .IsRequired();
            builder.HasOne(t => t.VerifyState).WithMany().HasForeignKey("_verifyStateId");

            builder.Property(t => t.CreateTime).HasColumnName("create_time").HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(t => t.VerifyTime).HasColumnName("verify_time");
            builder.Property(t => t.UserId).HasMaxLength(50).HasColumnName("user_id").IsRequired();
        }
    }
}