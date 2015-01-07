using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class PublishedPCConfigurationSpecificationTests
		: PCConfigurationSpecificationsTests<PublishedPCConfigurationDetailedSpecification>
	{
		private Mock<IPCConfigurationRepository> _configurationRepositoryMock;
		private IDetailedSpecification<PCConfiguration, PublishedPCConfigurationCheckDetails> _detailedSpecification;

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
			_configurationRepositoryMock = new Mock<IPCConfigurationRepository>();
			_detailedSpecification = new PublishedPCConfigurationDetailedSpecification(_configurationRepositoryMock.Object);
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

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			Assert.That(result.FailureDetails.NameNotEmptyFailure);
		}

		[Test]
		public void IsSatisfied_PCConfigurationWithTooLargeName_ShouldNotPass()
		{
			//Arrange
			Configuration.WithName("".PadLeft(PublishedPCConfigurationDetailedSpecification.NameMaxLength + 1, '*'));
			AddRequiredComponents();
			AddRequiredWithAllowedDuplicatesComponents();

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			Assert.That(result.FailureDetails.NameMaxLengthFailure);
		}

		[Test]
		public void IsSatisfied_NameNotUnique_ShouldNotPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();
			_configurationRepositoryMock.Setup(x => x.FindPublishedConfigurations(Configuration.Name))
				.Returns(new List<PCConfiguration> {new PCConfiguration()}.AsQueryable());

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			Assert.That(result.FailureDetails.NameUniqueFailure);
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredComponentsNotAdded_ShouldNotPass()
		{
			foreach (var exceptComponentType in _exactlyOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddValidName();
				AddRequiredWithAllowedDuplicatesComponents();
				AddRequiredComponentsExcept(exceptComponentType);

				//Act
				var result = _detailedSpecification.IsSatisfiedBy(Configuration);

				//Assert
				Assert.That(!result.IsSatisfied);
				Assert.That(result.FailureDetails.RequiredButNotAddedTypes.Count == 1);
				Assert.That(result.FailureDetails.RequiredButNotAddedTypes.First() == exceptComponentType);
			}
		}

		[Test]
		public void IsSatisfied_AnyOfExactlyOneComponentsOccursTwice_ShouldNotPass()
		{
			foreach (var duplicatedComponentType in _exactlyOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddAllRequiredComponentsAndValidName();
				Configuration.WithComponent(CreateValidComponent(duplicatedComponentType));

				//Act
				var result = _detailedSpecification.IsSatisfiedBy(Configuration);

				//Assert
				Assert.That(!result.IsSatisfied);
				Assert.That(result.FailureDetails.TypesViolatedUniqueConstraint.Count == 1);
				Assert.That(result.FailureDetails.TypesViolatedUniqueConstraint.First() == duplicatedComponentType);
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsNotAdded_ShouldNotPass()
		{
			foreach (var exceptComponentType in _requiredAndCanHaveMoreThanOneComponentTypes)
			{
				//Arrange
				Configuration = new PCConfiguration();
				AddValidName();
				AddRequiredComponents();
				AddRequiredWithAllowedDuplicatesComponentsExcept(exceptComponentType);

				//Act
				var result = _detailedSpecification.IsSatisfiedBy(Configuration);

				//Assert
				Assert.That(!result.IsSatisfied);
				Assert.That(result.FailureDetails.RequiredButNotAddedTypes.Count == 1);
				Assert.That(result.FailureDetails.RequiredButNotAddedTypes.First() == exceptComponentType);
			}
		}

		[Test]
		public void IsSatisfied_AnyOfRequiredWithDuplicatesComponentsAddedTwice_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();
			AddRequiredWithAllowedDuplicatesComponents();

			//Assert
			Assert.That(_detailedSpecification.IsSatisfiedBy(Configuration).IsSatisfied);
		}

		[Test]
		public void IsSatisfied_AllRequiredComponentsAddedAndValidName_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			//Assert
			Assert.That(_detailedSpecification.IsSatisfiedBy(Configuration).IsSatisfied);
		}

		[Test]
		public void IsSatisfied_SomeRequiredComponentsAreContainedInOtherAddedComponents_ShouldPass()
		{
			//Arrange
			AddValidName();
			AddRequiredComponents();
			var parentComponent = ComponentAt(0);
			foreach (var type in _requiredAndCanHaveMoreThanOneComponentTypes)
			{
				var component = CreateValidComponent(type);
				parentComponent.WithContainedComponent(component);
				parentComponent = component;
			}

			//Assert
			Assert.That(_detailedSpecification.IsSatisfiedBy(Configuration).IsSatisfied);
		}

		[Test]
		public void IsSatisfied_SomeRootComponentsHavePlugSlotsThatOtherComponentsDoNotProvide_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();

			ComponentAt(0).WithPlugSlot(plugSlots[0]);
			ComponentAt(2).WithPlugSlot(plugSlots[1]);
			ComponentAt(3).WithPlugSlot(plugSlots[2]);
			ComponentAt(4)
				.WithContainedSlot(plugSlots[0])
				.WithContainedSlot(plugSlots[1]);

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			AssertNotFoundInterfaceInfosEqual(result.FailureDetails.NotFoundInterfaces,
				new List<InterfaceDeficitInfo>
				{
					new InterfaceDeficitInfo(plugSlots[2], 1, new List<PCComponent> {ComponentAt(3)})
				});
		}

		[Test]
		public void IsSatisfied_RootComponentCanBePluggedOnlyToItself_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlot = DomainObjectsCreator.CreateInterface(1);
			ComponentAt(0).WithPlugSlot(plugSlot)
				.WithContainedComponent(
					CreateValidComponent(ComponentType.SolidStateDrice)
						.WithContainedSlot(plugSlot));

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			Assert.That(result.FailureDetails.ComponentPlugCycle.Count == 1);
			Assert.That(result.FailureDetails.ComponentPlugCycle.First().SameIdentityAs(ComponentAt(0)));
		}

		[Test]
		public void IsSatisfied_PlugSlotsCountIsLargerThanContainedSlotsOfTheSameType_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlot = DomainObjectsCreator.CreateInterface(1);

			ComponentAt(0).WithPlugSlot(plugSlot);
			ComponentAt(2).WithPlugSlot(plugSlot);
			ComponentAt(3).WithPlugSlot(plugSlot);
			ComponentAt(4).WithContainedComponent(
				CreateValidComponent(ComponentType.CentralProcessingUnit)
					.WithContainedSlot(plugSlot)
					.WithContainedSlot(plugSlot));

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			AssertNotFoundInterfaceInfosEqual(result.FailureDetails.NotFoundInterfaces,
				new List<InterfaceDeficitInfo>
				{
					new InterfaceDeficitInfo(plugSlot, 1, new[] {ComponentAt(0), ComponentAt(2), ComponentAt(3)})
				});
		}

		[Test]
		public void IsSatisfied_OnlyCyclicPluggingIsPossible_ShouldFail()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();

			ComponentAt(0).WithPlugSlot(plugSlots[0]).WithContainedSlot(plugSlots[1]);
			ComponentAt(2).WithPlugSlot(plugSlots[1]).WithContainedSlot(plugSlots[2]);
			ComponentAt(3).WithPlugSlot(plugSlots[2])
				.WithContainedComponent(CreateValidComponent(ComponentType.SolidStateDrice)
					.WithContainedSlot(plugSlots[0]));

			//Act
			var result = _detailedSpecification.IsSatisfiedBy(Configuration);

			//Assert
			Assert.That(!result.IsSatisfied);
			var cycle = result.FailureDetails.ComponentPlugCycle;
			Assert.That(cycle.Contains(ComponentAt(0)));
			Assert.That(cycle.Contains(ComponentAt(2)));
			Assert.That(cycle.Contains(ComponentAt(4)));
		}

		[Test]
		public void IsSatisfied_AllRootComponentsCanBePluggedToOtherComponents_ShouldPass()
		{
			//Arrange
			AddAllRequiredComponentsAndValidName();

			var plugSlots = CreateInterfaces();
			ComponentAt(0).WithPlugSlot(plugSlots[0]).WithPlugSlot(plugSlots[1]);
			ComponentAt(2).WithPlugSlot(plugSlots[1])
				.WithContainedSlot(plugSlots[1])
				.WithContainedSlot(plugSlots[2]);
			ComponentAt(3).WithPlugSlot(plugSlots[2]);
			ComponentAt(4)
				.WithContainedComponent(CreateValidComponent(ComponentType.CentralProcessingUnit)
					.WithContainedSlot(plugSlots[0])
					.WithContainedSlot(plugSlots[1]));

			//Assert
			Assert.That(_detailedSpecification.IsSatisfiedBy(Configuration).IsSatisfied);
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

		private PCComponent ComponentAt(int index)
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

		private void AssertNotFoundInterfaceInfosEqual(List<InterfaceDeficitInfo> actualList,
			List<InterfaceDeficitInfo> expectedList)
		{
			Assert.That(UtilsAssert.CollectionsEqual(expectedList, actualList, AssertNotFoundInterfaceInfosEqual));
		}

		private bool AssertNotFoundInterfaceInfosEqual(InterfaceDeficitInfo actualDeficitInfo,
			InterfaceDeficitInfo expectedDeficitInfo)
		{
			return actualDeficitInfo.ProblemInterface.SameIdentityAs(expectedDeficitInfo.ProblemInterface)
			       && actualDeficitInfo.Deficit == expectedDeficitInfo.Deficit
			       && UtilsAssert.CollectionsEqual(expectedDeficitInfo.RequiredByComponents,
				       actualDeficitInfo.RequiredByComponents);
		}

		#endregion
	}
}