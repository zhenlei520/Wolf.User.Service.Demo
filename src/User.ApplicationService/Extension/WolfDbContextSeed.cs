// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions.SeedWork;
using EInfrastructure.Core.Config.EnumerationExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using User.Domain.AggregatesModel.Enumeration;
using User.Repository.MySql;

namespace User.ApplicationService.Extension
{
    public static class WolfDbContextSeed
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <param name="logService"></param>
        /// <returns></returns>
        public static async Task SeedAsync(WolfDbContext context, IHostingEnvironment env, ILogService logService)
        {
            try
            {
                await using (context)
                {
                    await context.CreateTable<IdentityType>();
                    await context.CreateTable<UserSafetyType>();
                    await context.CreateTable<VerifyState>();
                    await context.CreateTable<Gender>();
                    await context.CreateTable<UserState>();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region 创建数据库表

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static async Task CreateTable<T>(this DbContext context) where T : Enumeration
        {
            DbSet<T> enumerations = context.Set<T>();
            if (!EnumerableExtensions.Any(enumerations))
            {
                var capitalAccountType = Enumeration.GetAll<T>().ToList();
                foreach (var item in capitalAccountType.OrderBy(x => x.Id))
                {
                    var enumInstance = (T) Activator.CreateInstance(typeof(T), item.Id, item.Name);
                    await enumerations.AddAsync(enumInstance);
                }

                await context.SaveChangesAsync();
            }
        }

        #endregion
    }
}