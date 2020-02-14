// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.MySql;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using Polly;
using User.Infrastructure.Core;
using User.Repository.MySql.Extension;

namespace User.Repository.MySql
{
    public sealed class WolfDbContext : DbContext, IUnitOfWork, IPerRequest
    {
        #region Property

        public const string DEFAULT_SCHEMA = "user";
        private readonly IMediator _mediator;

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        #endregion

        #region Ctor

        private WolfDbContext(DbContextOptions<WolfDbContext> options) : base(options)
        {
        }

        public WolfDbContext(DbContextOptions<WolfDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            new DbContextOptionsBuilder(options).EnableDebugTrace();
            // System.Diagnostics.Debug.WriteLine("UserContext::ctor ->" + HashCodeCommon.GetHashCode(this));
        }

        #endregion

        #region 设置表映射

        /// <summary>
        /// 设置表映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //自动映射
            AutoMap(modelBuilder, typeof(WolfDbContext));
        }

        #endregion

        #region 同步保存

        /// <summary>
        /// 同步保存
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            this.SaveChanges();
            return true;
        }

        #endregion

        #region (事务)异步保存实体

        /// <summary>
        /// 异步保存实体
        /// </summary>
        /// <param name="cancellationToken">事务Token</param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            /*
            调度域事件集合。
            选择:
            A)在将数据(EF SaveChanges)提交到数据库之前，将执行单个事务，包括使用与“InstancePerLifetimeScope”或“scoped”lifetime相同的DbContext的域事件处理程序的副作用
            B)在将数据(EF SaveChanges)提交到数据库后，将进行多个事务。
            您将需要处理任何处理程序失败时的最终一致性和补偿操作。
             */
            await _mediator.DispatchDomainEventsAsync<string>(this);

            return true;
        }

        #endregion

        #region 自动映射

        /// <summary>
        /// 添加自动映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assType"></param>
        public static void AutoMap(ModelBuilder modelBuilder, Type assType)
        {
            var mappingInterface = typeof(IEntityTypeConfiguration<>);

            // Types that do entity mapping
            var mappingTypes = assType.GetTypeInfo().Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(y =>
                    y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));

            foreach (var type in mappingTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            // foreach (var property in modelBuilder.Model.GetEntityTypes()
            //     .SelectMany(t => t.GetProperties())
            //     .Where(p => p.ClrType == typeof(decimal) && p.Relational().ColumnType == null))
            // {
            //     property.Relational().ColumnType = "decimal(18,2)";
            // }
        }

        #endregion

        #region 事务相关

        #region 异步创建事务

        /// <summary>
        /// 异步创建事务
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    return null;
                }

                if (Database.CurrentTransaction == null)
                {
                    _currentTransaction = await Policy.Handle<MySqlException>()
                        .RetryAsync()
                        .ExecuteAsync<IDbContextTransaction>(async () =>
                            await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken));
                }
            }
            catch (Exception ex)
            {
                ServiceProvider.GetLogService().Error($"异步创建事务异常:{ex}");
            }

            return _currentTransaction;
        }

        #endregion

        #region 异步提交事务

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != _currentTransaction)
            {
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
            }

            try
            {
                await SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion

        #region 事务回滚

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion

        #endregion
    }

    public class DbContextDesignFactory : IDesignTimeDbContextFactory<WolfDbContext>
    {
        public WolfDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WolfDbContext>().UseMySql(
                "Server=localhost;port=3306;database=wolf_user;uid=root;pwd=rootroot;Convert Zero Datetime=True;");

            return new WolfDbContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
            {
                throw new NotImplementedException();
            }

            public Task Publish<TNotification>(TNotification notification,
                CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
                CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }
    }
}