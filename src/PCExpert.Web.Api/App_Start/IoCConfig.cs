using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using PCExpert.Core.Application;
using PCExpert.Core.Application.Impl;
using PCExpert.Core.DataAccess;
using PCExpert.Core.Domain;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Domain.Validation;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.DataAccess;
using PCExpert.DomainFramework.EF;
using PCExpert.DomainFramework.Utils;
using PCExpert.DomainFramework.Validation;
using PCExpert.Web.Api.Common.WebModel;
using PCExpert.Web.Api.LinkSetters;

namespace PCExpert.Web.Api
{
	public static class IoCConfig
	{
		public static void InitIoC(HttpConfiguration configuration)
		{
			Argument.NotNull(configuration);
			var builder = new ContainerBuilder();


			ConfigureValidators(builder);
			ConfigureDbInfrastructure(builder);
			ConfigureDomainModel(builder);
			ConfigureApplicationServices(builder);
			ConfigureWebLinkSetters(builder);
			ConfigureWebApiInfrastructure(builder, configuration);

			var container = builder.Build();
			ConfigureMappers(container);
			configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}

		private static void ConfigureValidators(ContainerBuilder container)
		{
			container.Register<IValidatorFactory>(
				c => InitValidators(new DomainValidatorFactory(), c));
		}

		private static DomainValidatorFactory InitValidators(DomainValidatorFactory validatorFactory,
			IComponentContext context)
		{
			return validatorFactory
				.AddValidator(new CharacteristicValueValidator<NumericCharacteristicValue>())
				.AddValidator(new CharacteristicValueValidator<StringCharacteristicValue>())
				.AddValidator(new CharacteristicValueValidator<BoolCharacteristicValue>())
				.AddValidator(new ComponentCharacteristicValidator<NumericCharacteristic>())
				.AddValidator(new ComponentCharacteristicValidator<StringCharacteristic>())
				.AddValidator(new ComponentCharacteristicValidator<BoolCharacteristic>())
				.AddValidator(new ComponentInterfaceValidator())
				.AddValidator(new PCComponentValidator())
				.AddValidator(new PCConfigurationValidator(
					context.Resolve<PublishedPCConfigurationSpecification>(),
					context.Resolve<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>>()));
		}

		private static void ConfigureDbInfrastructure(ContainerBuilder builder)
		{
			builder.Register(
				c => new HttpContextPCExpertContextProvider("DefaultConnection", c.Resolve<IValidatorFactory>()))
				.As<IDbContextProvider>().SingleInstance();
			builder.RegisterType<EfWorkplace>().As<PersistenceWorkplace>().SingleInstance();
			builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>().SingleInstance();
		}

		private static void ConfigureDomainModel(ContainerBuilder builder)
		{
			//Specifications
			builder.RegisterType<PublishedPCConfigurationSpecification>();
			builder.RegisterType<PublishedPCConfigurationCheckDetailsInterpreter>()
				.As<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>>();

			//Repositories
			builder.RegisterType<ComponentInterfaceRepository>().As<IComponentInterfaceRepository>().SingleInstance();
			builder.RegisterType<PCComponentRepository>().As<IPCComponentRepository>().SingleInstance();
			builder.RegisterType<PCConfigurationRepository>().As<IPCConfigurationRepository>().SingleInstance();
		}

		private static void ConfigureApplicationServices(ContainerBuilder container)
		{
			//Services
			container.RegisterType<ComponentInterfaceService>().As<IComponentInterfaceService>().SingleInstance();
		}

		private static void ConfigureWebLinkSetters(ContainerBuilder builder)
		{
			builder.RegisterType<ComponentInterfaceModelLinkSetter>().As<ILinkSetter>().SingleInstance();
			builder.Register(c => c.Resolve<IEnumerable<ILinkSetter>>().ToDictionary(x => x.ModelType))
				.As<IDictionary<Type, ILinkSetter>>()
				.SingleInstance();
			builder.RegisterType<LinkSettingEngine>().InstancePerLifetimeScope();
		}

		private static void ConfigureWebApiInfrastructure(ContainerBuilder builder, HttpConfiguration configuration)
		{
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
			builder.RegisterWebApiFilterProvider(configuration);
		}

		private static void ConfigureMappers(IContainer container)
		{
			MappersConfig.Configure();
			Model.Core.MappersConfig.Configure(container.Resolve<LinkSettingEngine>);
		}
	}
}