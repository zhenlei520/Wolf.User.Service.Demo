// Copyright (c) zhenlei520 All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using Autofac;
using EInfrastructure.Core.Config.EntitiesExtensions;
using EInfrastructure.Core.Configuration.Ioc;
using EInfrastructure.Core.MySql.Repository;

namespace User.ApplicationService.Infrastructure.AutofacModules
{
    public class  ApplicationModule: Autofac.Module
    {
        public ApplicationModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(QueryBase<,>)).As(typeof(IQuery<,>)).PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepository<,>)).PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(QueryBase<,,>)).As(typeof(IQuery<,,>)).PropertiesAutowired()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RepositoryBase<,,>)).As(typeof(IRepository<,,>)).PropertiesAutowired()
                .InstancePerLifetimeScope();

            var assemblys = AppDomain.CurrentDomain.GetAssemblies().ToArray();

            var perRequestType = typeof(IPerRequest);
            builder.RegisterAssemblyTypes(assemblys)
                .Where(t => perRequestType.IsAssignableFrom(t) && t != perRequestType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var perDependencyType = typeof(IDependency);
            builder.RegisterAssemblyTypes(assemblys)
                .Where(t => perDependencyType.IsAssignableFrom(t) && t != perDependencyType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            var singleInstanceType = typeof(ISingleInstance);
            builder.RegisterAssemblyTypes(assemblys)
                .Where(t => singleInstanceType.IsAssignableFrom(t) && t != singleInstanceType)
                .PropertiesAutowired()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}