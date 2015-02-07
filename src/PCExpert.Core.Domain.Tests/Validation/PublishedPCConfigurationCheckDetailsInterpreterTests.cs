using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Domain.Resources;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Domain.Validation;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Domain.Tests.Validation
{
	[TestFixture]
	public class PublishedPCConfigurationCheckDetailsInterpreterTests
	{
		private Mock<IPublishedPCConfigurationCheckDetails> _detailsMock;
		private PublishedPCConfigurationCheckDetailsInterpreter _interpreter;

		private static List<ComponentType> ComponentTypesList
		{
			get { return new[] {ComponentType.HardDiskDrive, ComponentType.PowerSupply}.ToList(); }
		}

		[SetUp]
		public void EstablishContext()
		{
			_interpreter = new PublishedPCConfigurationCheckDetailsInterpreter();
			CreateValidDetailsMock();
		}

		[Test]
		public void Interpret_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _interpreter.Interpret(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Interpret_DetailsWithNoFailures_ShouldReturnEmptyEnumeration()
		{
			CreateValidDetailsMock();
			Assert.That(_interpreter.Interpret(_detailsMock.Object).IsEmpty());
		}

		[Test]
		public void Interpret_AnyFlagIsRaised_ShouldReturnCorrespondingFailures()
		{
			var testCases = new Dictionary<Expression<Func<IPublishedPCConfigurationCheckDetails, bool>>, string>
			{
				{x => x.NameNotEmptyFailure, ValidationMessages.ConfigNameCannotBeEmptyMsg},
				{x => x.NameMaxLengthFailure, ValidationMessages.ConfigNameTooLongMsg},
				{x => x.ComponentsCycleFailure, ValidationMessages.ConfigCyclicPluggingMsg}
			};

			foreach (var testCase in testCases)
			{
				//Arrange
				CreateValidDetailsMock();
				RaiseFlag(testCase.Key);

				//Act
				var result = InterpretDetails();

				//Assert
				Assert.That(result.Count == 1);
				Assert.That(result.First().ErrorMessage == testCase.Value);
			}
		}

		[Test]
		public void Interpret_RequiredButNotAddedTypesNotEmpty_ShouldReturnCorrespondingFailure()
		{
			//Arrange
			_detailsMock.SetupGet(x => x.RequiredButNotAddedTypes).Returns(ComponentTypesList);

			AssertExactlyOneInterpretedFailure();
		}

		[Test]
		public void Interpret_TypesViolatedUniqueConstraintNotEmpty_ShouldReturnCorrespondingFailure()
		{
			//Arrange
			_detailsMock.SetupGet(x => x.TypesViolatedUniqueConstraint).Returns(ComponentTypesList);

			AssertExactlyOneInterpretedFailure();
		}

		[Test]
		public void Interpret_NotFoundInterfacesNotEmpty_ShouldReturnCorrespondingFailuresForEachProblemInterfaces()
		{
			//Arrange
			_detailsMock.SetupGet(x => x.NotFoundInterfaces)
				.Returns(new List<InterfaceDeficitInfo>
				{
					new InterfaceDeficitInfo(DomainObjectsCreator.CreateInterface(1), 1, new List<PCComponent>
					{
						DomainObjectsCreator.CreateComponent(1, ComponentType.HardDiskDrive)
					}),
					new InterfaceDeficitInfo(DomainObjectsCreator.CreateInterface(2), 2, new List<PCComponent>
					{
						DomainObjectsCreator.CreateComponent(1, ComponentType.HardDiskDrive)
					})
				});

			AssertInterpretedFailuresCount(2);
		}

		private void CreateValidDetailsMock()
		{
			_detailsMock = new Mock<IPublishedPCConfigurationCheckDetails>();
			_detailsMock.SetupGet(x => x.NotFoundInterfaces).Returns(new List<InterfaceDeficitInfo>());
			_detailsMock.SetupGet(x => x.RequiredButNotAddedTypes).Returns(new List<ComponentType>());
			_detailsMock.SetupGet(x => x.TypesViolatedUniqueConstraint).Returns(new List<ComponentType>());
		}

		private void RaiseFlag(Expression<Func<IPublishedPCConfigurationCheckDetails, bool>> flagExpression)
		{
			_detailsMock.SetupGet(flagExpression).Returns(true);
		}

		private List<ValidationFailure> InterpretDetails()
		{
			return _interpreter.Interpret(_detailsMock.Object).ToList();
		}

		private void AssertExactlyOneInterpretedFailure()
		{
			AssertInterpretedFailuresCount(1);
		}

		private void AssertInterpretedFailuresCount(int expectedCount)
		{
			var result = InterpretDetails();

			Assert.That(result.Count == expectedCount);
		}
	}
}