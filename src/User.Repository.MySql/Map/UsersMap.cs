// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.AggregatesModel.UserAggregate;

namespace User.Repository.MySql.Map
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UsersMap : IEntityTypeConfiguration<Users>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("wolf_user");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasMaxLength(50).HasColumnName("id").IsRequired();

            builder.Property(t => t.Name).HasMaxLength(20).HasColumnName("name").IsRequired();
            builder.Property(t => t.Account).HasMaxLength(20).HasColumnName("account").IsRequired();
            builder.Property(t => t.Avatar).HasMaxLength(200).HasColumnName("avatar");
            builder.Property(t => t.Password).HasMaxLength(100).HasColumnName("password").IsRequired();
            builder.Property(t => t.SecretKey).HasMaxLength(100).HasColumnName("secret_key").IsRequired();

            //性别
            builder
                .Property<int>("_genderId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("gender")
                .IsRequired();
            builder.HasOne(t => t.Gender).WithMany().HasForeignKey("_genderId");

            //状态
            builder.Property<int>("_userStateId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("state")
                .IsRequired();
            builder.HasOne(t => t.UserState).WithMany().HasForeignKey("_userStateId");

            builder.Property(t => t.BirthDay).HasColumnName("birth_day");
            builder.Property(t => t.RegisterTime).HasColumnName("register_time").IsRequired()
                .HasDefaultValue(DateTime.Now);

            #region 邀请人信息

            builder.OwnsOne(t => t.UserSources).Property(t => t.InviterUserId).HasMaxLength(50)
                .HasColumnName("inviter_user_id").IsRequired();
            builder.OwnsOne(t => t.UserSources).Property(t => t.AppleId).HasMaxLength(50).HasColumnName("apple_id")
                .IsRequired();
            builder.OwnsOne(t => t.UserSources).Property(t => t.Referer).HasMaxLength(200).HasColumnName("referer")
                .IsRequired();

            #endregion

            builder.HasMany(t => t.UserSafetyInformationItems).WithOne().HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}