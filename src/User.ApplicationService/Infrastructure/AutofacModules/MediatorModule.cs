// Copyright (c) zhenlei520 All rights reserved.

using System;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using User.ApplicationService.Application.Behaviors;

namespace User.ApplicationService.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        private readonly Assembly[] _assembly;

        public MediatorModule()
        {
            _assembly = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            
            builder.RegisterAssemblyTypes(_assembly).Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();
            
            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return componentContext.TryResolve(t, out o) ? o : null;
                };
            });
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}