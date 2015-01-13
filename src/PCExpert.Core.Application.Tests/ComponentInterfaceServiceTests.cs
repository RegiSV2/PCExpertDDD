using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Application.Impl;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Domain;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Tests.Utils;
using PCExpert.Core.Tests.Utils.FakeAsyncQuery;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Exceptions;
using PCExpert.DomainFramework.Specifications;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Application.Tests
{
	[TestFixture]
	public class ComponentInterfaceServiceTests
	{
		private Mock<IComponentInterfaceRepository> _repositoryMock;
		private FakeUnitOfWork _unitOfWork;
		private IComponentInterfaceService _service;

		[SetUp]
		public void EstablishContext()
		{
			_repositoryMock = new Mock<IComponentInterfaceRepository>();
			_unitOfWork = new FakeUnitOfWork();
			_service = new ComponentInterfaceService(_unitOfWork, _repositoryMock.Object);
		}

		[Test]
		public void CreateComponentInterface_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _service.CreateComponentInterface(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public async void CreateComponentInterface_ArgumentNameIsAlreadyUsed_ShouldThrowBusinessLogicException()
		{
			//Arrange
			_unitOfWork.ShouldThrowPersistenceException = true;
			var interfaceVO = CreateInterfaceVO();

			try
			{
				await _service.CreateComponentInterface(interfaceVO);
			}
			catch (BusinessLogicException)
			{
			}
		}

		[Test]
		public async void CreateComponentInterface_ArgumentNameNotused_ShouldAddNewInterface()
		{
			//Arrange
			var interfaceVO = CreateInterfaceVO();
			ComponentInterface savedInterface = null;
			_repositoryMock.Setup(x => x.Save(It.IsAny<ComponentInterface>()))
				.Callback(new Action<ComponentInterface>(x =>
				{
					savedInterface = x;
				}));

			//Act
			await _service.CreateComponentInterface(interfaceVO);

			//Assert
			Assert.That(savedInterface != null);
			Assert.That(savedInterface.Name == interfaceVO.Name);
		}

		[Test]
		public void GetComponentInterfaces_NullParameters_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _service.GetComponentInterfaces(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void GetComponentInterfaces_NotNullParameters_ShouldReturnResultFilteredBySpecifiedParameters()
		{
			const int listSize = 15;
			const int pageSize = 5;
			//Arrange
			var intsList = new List<ComponentInterface>();
			for (var i = 0; i < listSize; i++)
				intsList.Add(DomainObjectsCreator.CreateInterface(i));
			_repositoryMock.Setup(x => x.Query(It.IsAny<PersistenceAwareSpecification<ComponentInterface>>()))
				.Returns(intsList.AsAsyncQueryable());

			var requestParameters = new TableParameters(
				new PagingParameters(1, pageSize),
				new OrderingParameters(ExpressionReflection.Property<ComponentInterface>(x => x.Name).Name, SortDirection.Descending));
			var expectedResult = intsList.OrderByDescending(x => x.Name).Skip(pageSize).Take(pageSize).ToList();

			//Act
			var actualResult = _service.GetComponentInterfaces(requestParameters).Result;

			//Assert
			Assert.That(actualResult.CountTotal == listSize);
			Assert.That(actualResult.Items.Count == expectedResult.Count);
			for (var i = 0; i < actualResult.Items.Count; i++)
				Assert.That(actualResult.Items[i].Name, Is.EqualTo(expectedResult[i].Name));
		}

		[Test]
		public void GetComponentInterfaces_RequestedPageGreaterThanTotalNumberOfPages()
		{
			Assert.Fail();
		}

		private static ComponentInterfaceVO CreateInterfaceVO()
		{
			return new ComponentInterfaceVO { Name = NamesGenerator.ComponentInterfaceName() };
		}
	}
}