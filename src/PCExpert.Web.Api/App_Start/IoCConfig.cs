using System.Linq;
using System.Web.Http;
using System.Web.Http.Dependencies;
using FluentValidation;
using LightInject;
using LightInject.Web;
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
using PCExpert.Web.Api.Common;
using PCExpert.Web.Api.Common.WebModel;
using PCExpert.Web.Api.Controllers;
using PCExpert.Web.Api.LinkSetters;
using PCExpert.Web.Model.Core;
using MappersConfig = PCExpert.Web.Model.Core.MappersConfig;

namespace PCExpert.Web.Api
{
	public static class IoCConfig
	{
		public static void InitIoC(HttpConfiguration configuration)
		{
			Argument.NotNull(configuration);
			var container = new ServiceContainer();

			ConfigureWebApiInfrastructure(container, configuration);
			ConfigureValidators(container);
			ConfigureDBInfrastructure(container);
			ConfigureDomainModel(container);
			ConfigureApplicationServices(container);
			ConfigureMappers(container);
			ConfigureWebLinkSetters(container);
			ConfigureMvcControllers(container);

		}

		private static void ConfigureWebApiInfrastructure(IServiceContainer container, HttpConfiguration configuration)
		{
			configuration.DependencyResolver = new LightInjectDependencyResolver(container);
			container.Register<CurrentRequest>(new PerRequestLifeTime());
		}

		private static void ConfigureValidators(IServiceContainer container)
		{
			container.Register<IValidatorFactory>(
				sf => InitValidators(new DomainValidatorFactory(), sf), new PerContainerLifetime());
		}

		private static DomainValidatorFactory InitValidators(DomainValidatorFactory validatorFactory,
			IServiceFactory serviceFactory)
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
					serviceFactory.GetInstance<PublishedPCConfigurationSpecification>(),
					serviceFactory.GetInstance<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>>()));
		}

		private static void ConfigureDBInfrastructure(ServiceContainer container)
		{
			container.Register<IDbContextProvider>(
				sf => new HttpContextPCExpertContextProvider("DefaultConnection", sf.GetInstance<IValidatorFactory>()),
				new PerContainerLifetime());
			container.Register<PersistenceWorkplace, EfWorkplace>(new PerContainerLifetime());
			container.Register<IUnitOfWork, EntityFrameworkUnitOfWork>(new PerContainerLifetime());
		}

		private static void ConfigureDomainModel(ServiceContainer container)
		{
			//Specifications
			container.Register<PublishedPCConfigurationSpecification>();
			container.Register<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>,
				PublishedPCConfigurationCheckDetailsInterpreter>();

			//Repositories
			container.Register<IComponentInterfaceRepository, ComponentInterfaceRepository>();
			container.Register<IPCComponentRepository, PCComponentRepository>();
			container.Register<IPCConfigurationRepository, PCConfigurationRepository>();
		}

		private static void ConfigureApplicationServices(ServiceContainer container)
		{
			//Services
			container.Register<IComponentInterfaceService, ComponentInterfaceService>();
		}

		private static void ConfigureWebLinkSetters(ServiceContainer container)
		{
			container.Register<ILinkSetter, ComponentInterfaceModelLinkSetter>(new PerContainerLifetime());

			var setters = container.GetAllInstances<ILinkSetter>().ToDictionary(x => x.ModelType);
			container.Register(sf => new LinkSettingEngine(sf.GetInstance<CurrentRequest>(), setters));
		}

		private static void ConfigureMappers(IServiceContainer container)
		{
			Web.Model.Core.MappersConfig.Configure(container.Create<LinkSettingEngine>);
			Core.Application.MappersConfig.Configure();
		}

		private static void ConfigureMvcControllers(IServiceContainer container)
		{
			container.Register<ComponentInterfaceController>();
		}
	}
}