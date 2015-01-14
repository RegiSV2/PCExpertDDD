using System.Web.Mvc;
using FluentValidation;
using LightInject;
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
using PCExpert.DomainFramework.Validation;

namespace PCExpert.WebApp
{
	public static class IoCConfig
	{
		public static void InitIoC()
		{
			var container = new ServiceContainer();
			container.Register<IValidatorFactory>(
				sf => InitValidators(new DomainValidatorFactory(), sf), new PerContainerLifetime());
			container.Register(
				sf => new HttpContextPCExpertContextProvider("DefaultConnection", sf.GetInstance<IValidatorFactory>()),
				new PerContainerLifetime());

			container.Register<PersistenceWorkplace, EfWorkplace>(new PerContainerLifetime());
			container.Register<IUnitOfWork, EntityFrameworkUnitOfWork>(new PerContainerLifetime());

			//Specifications
			container.Register<PublishedPCConfigurationSpecification>();
			container.Register<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>,
				PublishedPCConfigurationCheckDetailsInterpreter>();

			//Repositories
			container.Register<IComponentInterfaceRepository, ComponentInterfaceRepository>();
			container.Register<IPCComponentRepository, PCComponentRepository>();
			container.Register<IPCConfigurationRepository, PCConfigurationRepository>();

			//Services
			container.Register<IComponentInterfaceService, ComponentInterfaceService>();

			ControllerBuilder.Current.SetControllerFactory(new LightInjectControllerFactory(container));
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
				.AddValidator(new PCConfigurationValidator(serviceFactory.GetInstance<PublishedPCConfigurationSpecification>(),
					serviceFactory.GetInstance<ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>>()));
		}
	}
}