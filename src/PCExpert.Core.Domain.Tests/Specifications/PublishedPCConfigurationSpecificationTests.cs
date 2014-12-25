using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class PublishedPCConfigurationSpecificationTests
	{
		private PublishedPCConfigurationSpecification _specification;

		private PCConfiguration _configuration;

		private readonly ComponentType[] _exactlyOneComponentTypes =
		{
			ComponentType.Motherboard,
			ComponentType.PowerSupply
		};

		private readonly ComponentType[] _requiredAndCanHaveMoreThanOneComponentTypes =
		{
			ComponentType.CentralProcessingUnit,
			ComponentType.HardDiskDrive,
			ComponentType.RandomAccessMemory,
			ComponentType.VideoCard
		};

		[SetUp]
		public void EstablishContext()
		{
			_specification = new PublishedPCConfigurationSpecification();
			_configuration = new PCConfiguration();
		}

		private PCComponent CreateValidComponent(ComponentType type)
		{
			return new PCComponent("some name", type);
		}

		private void AddValidName()
		{
			_configuration.WithName("some name");
		}

		private void AddRequiredComponents()
		{
			AddRequiredComponentsExcept(null);
		}

		private void AddRequiredWithAllowedDuplicatesComponents()
		{
			AddRequiredWithAllowedDuplicatesComponentsExcept(null);
		}

		private void AddRequiredWithAllowedDuplicatesComponentsExcept(ComponentType? exceptComponentType)
		{
			AddComponentsFromListExcept(_requiredAndCanHaveMoreThanOneComponentTypes, exceptComponentType);
		}

		private void AddRequiredComponentsExcept(ComponentType? exceptComponentType)
		{
			AddComponentsFromListExcept(_exactlyOneComponentTypes, exceptComponentType);
		}

		private void AddComponentsFromListExcept(ComponentType[] componentTypes, ComponentType? exceptComponentType)
		{
			var typesToAdd = componentTypes.AsEnumerable();
			if (exceptComponentType.HasValue)
				typesToAdd = typesToAdd.Where(x => x != exceptComponentType.Value);

			foreach (var type in typesToAdd)
				_configuration.WithComponent(CreateValidComponent(type));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void IsSatisfied_PCConfigurationWithEmptyName_ShouldNotPass(string name)
		{
			//Arrange
			_configuration.WithName(name);
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(!_specification.IsSatisfied(_configuration));
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredComponentsNotAdded_ShouldNotPass()
		{
			foreach(var componentType in _exactlyOneComponentTypes)
			{
				//Arrange
				_configuration = new PCConfiguration();
				AddValidName();
				AddRequiredWithAllowedDuplicatesComponents();
				AddRequiredComponentsExcept(componentType);

				//Assert
				Assert.That(!_specification.IsSatisfied(_configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfExactlyOneComponentsOccursTwice_ShouldNotPass()
		{
			foreach (var componentType in _exactlyOneComponentTypes)
			{
				//Arrange
				_configuration = new PCConfiguration();
				AddValidName();
				AddRequiredComponents();
				AddRequiredWithAllowedDuplicatesComponents();
				_configuration.WithComponent(CreateValidComponent(componentType));

				//Assert
				Assert.That(!_specification.IsSatisfied(_configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsNotAdded_ShouldNotPass()
		{
			foreach (var componentType in _requiredAndCanHaveMoreThanOneComponentTypes)
			{
				//Arrange
				_configuration = new PCConfiguration();
				AddValidName();
				AddRequiredComponents();
				AddRequiredWithAllowedDuplicatesComponentsExcept(componentType);

				//Assert
				Assert.That(!_specification.IsSatisfied(_configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsAddedTwice_ShouldPass()
		{
			//Arrange
			_configuration = new PCConfiguration();
			AddValidName();
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(_specification.IsSatisfied(_configuration));
		}

		[Test]
		public void IsSatisfied_AllRequiredComponentsAddedAndValidName_ShouldPass()
		{
			//Arrange
			_configuration = new PCConfiguration();
			AddValidName();
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(_specification.IsSatisfied(_configuration));
		}
	}
}
