using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class PublishedPCConfigurationSpecificationTests 
		: PCConfigurationSpecificationsTests<PublishedPCConfigurationSpecification>
	{
		private PublishedPCConfigurationSpecification _specification;

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
		public override void EstablishContext()
		{
			base.EstablishContext();
			_specification = new PublishedPCConfigurationSpecification();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void IsSatisfied_PCConfigurationWithEmptyName_ShouldNotPass(string name)
		{
			//Arrange
			Configuration.WithName(name);
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_PCConfigurationWithTooLargeName_ShouldNotPass()
		{
			//Arrange
			Configuration.WithName("".PadLeft(PublishedPCConfigurationSpecification.NameMaxLength + 1, '*'));
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredComponentsNotAdded_ShouldNotPass()
		{
			foreach (var componentType in _exactlyOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddValidName();
				AddRequiredWithAllowedDuplicatesComponents();
				AddRequiredComponentsExcept(componentType);

				//Assert
				Assert.That(!_specification.IsSatisfiedBy(Configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfExactlyOneComponentsOccursTwice_ShouldNotPass()
		{
			foreach (var componentType in _exactlyOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddAllRequiredComponentsAndValidName();
				Configuration.WithComponent(CreateValidComponent(componentType));

				//Assert
				Assert.That(!_specification.IsSatisfiedBy(Configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsNotAdded_ShouldNotPass()
		{
			foreach (var componentType in _requiredAndCanHaveMoreThanOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddValidName();
				AddRequiredComponents();
				AddRequiredWithAllowedDuplicatesComponentsExcept(componentType);

				//Assert
				Assert.That(!_specification.IsSatisfiedBy(Configuration));
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsAddedTwice_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_AllRequiredComponentsAddedAndValidName_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			//Assert
			Assert.That(_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_SomeRequiredComponentsAreContainedInOtherAddedComponents_ShouldPass()
		{
			//Arrange
			AddValidName();
			AddRequiredComponents();
			var parentComponent = ConfigComponentAt(0);
			foreach (var type in _requiredAndCanHaveMoreThanOneComponentTypes)
			{
				var component = CreateValidComponent(type);
				parentComponent.WithContainedComponent(component);
				parentComponent = component;
			}

			//Assert
			Assert.That(_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_SomeRootComponentsHavePlugSlotsThatOtherComponentsDoNotProvide_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();

			ConfigComponentAt(0).WithPlugSlot(plugSlots[0]);
			ConfigComponentAt(2).WithPlugSlot(plugSlots[1]);
			ConfigComponentAt(3).WithPlugSlot(plugSlots[2]);
			ConfigComponentAt(4)
				.WithContainedSlot(plugSlots[0])
				.WithContainedSlot(plugSlots[1]);

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_RootComponentCanBePluggedOnlyToItself_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlot = DomainObjectsCreator.CreateInterface(1);
			ConfigComponentAt(0).WithPlugSlot(plugSlot)
				.WithContainedComponent(
					CreateValidComponent(ComponentType.SolidStateDrice)
						.WithContainedSlot(plugSlot));

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_PlugSlotsCountIsLargerThanContainedSlotsOfTheSameType_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlot = DomainObjectsCreator.CreateInterface(1);

			ConfigComponentAt(0).WithPlugSlot(plugSlot);
			ConfigComponentAt(2).WithPlugSlot(plugSlot);
			ConfigComponentAt(3).WithPlugSlot(plugSlot);
			ConfigComponentAt(4).WithContainedComponent(
				CreateValidComponent(ComponentType.CentralProcessingUnit)
					.WithContainedSlot(plugSlot)
					.WithContainedSlot(plugSlot));

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_OnlyCyclicPluggingIsPossible_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();

			ConfigComponentAt(0).WithPlugSlot(plugSlots[0]).WithContainedSlot(plugSlots[1]);
			ConfigComponentAt(2).WithPlugSlot(plugSlots[1]).WithContainedSlot(plugSlots[2]);
			ConfigComponentAt(3).WithPlugSlot(plugSlots[2])
				.WithContainedComponent(CreateValidComponent(ComponentType.SolidStateDrice)
					.WithContainedSlot(plugSlots[0]));

			//Assert
			Assert.That(!_specification.IsSatisfiedBy(Configuration));
		}

		[Test]
		public void IsSatisfied_AllRootComponentsCanBePluggedToOtherComponents_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();
			ConfigComponentAt(0).WithPlugSlot(plugSlots[0]).WithPlugSlot(plugSlots[1]);
			ConfigComponentAt(2).WithPlugSlot(plugSlots[1])
				.WithContainedSlot(plugSlots[1])
				.WithContainedSlot(plugSlots[2]);
			ConfigComponentAt(3).WithPlugSlot(plugSlots[2]);
			ConfigComponentAt(4)
				.WithContainedComponent(CreateValidComponent(ComponentType.CentralProcessingUnit)
					.WithContainedSlot(plugSlots[0])
					.WithContainedSlot(plugSlots[1]));

			//Assert
			Assert.That(_specification.IsSatisfiedBy(Configuration));
		}

		#region Private methods

		private void AddAllRequiredComponentsAndValidName()
		{
			AddValidName();
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();
		}

		private IList<ComponentInterface> CreateInterfaces()
		{
			return new[]
			{
				DomainObjectsCreator.CreateInterface(1),
				DomainObjectsCreator.CreateInterface(2),
				DomainObjectsCreator.CreateInterface(3)
			};
		}

		private PCComponent ConfigComponentAt(int index)
		{
			return Configuration.Components.ElementAt(index);
		}

		private PCComponent CreateValidComponent(ComponentType type)
		{
			return new PCComponent("some name", type);
		}

		private void AddValidName()
		{
			Configuration.WithName("some name");
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
				Configuration.WithComponent(CreateValidComponent(type));
		}

		#endregion
	}
}