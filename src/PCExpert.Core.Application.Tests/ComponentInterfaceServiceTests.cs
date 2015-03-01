using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Application.Impl;
using PCExpert.Core.Application.ViewObjects;
using PCExpert.Core.Domain;
using PCExpert.Core.Domain.Repositories;
using PCExpert.Core.Tests.Utils;
using PCExpert.Core.Tests.Utils.FakeAsyncQuery;
using PCExpert.DomainFramework.Exceptions;
using PCExpert.DomainFramework.Specifications;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Application.Tests
{
	public class ComponentInterfaceServiceTests : BaseServiceTests
	{
		protected Mock<IComponentInterfaceRepository> RepositoryMock;
		protected IComponentInterfaceService Service;
		protected FakeUnitOfWork UnitOfWork;

		[SetUp]
		public override void EstablishContext()
		{
			base.EstablishContext();
			RepositoryMock = new Mock<IComponentInterfaceRepository>();
			UnitOfWork = new FakeUnitOfWork();
			Service = new ComponentInterfaceService(UnitOfWork, RepositoryMock.Object);
		}
	}

	[TestFixture]
	public class ComponentInterfaceService_CreateComponentInterfaceTests : ComponentInterfaceServiceTests
	{
		[Test]
		public void CreateComponentInterface_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => Service.CreateComponentInterface(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public async void CreateComponentInterface_ArgumentNameIsAlreadyUsed_ShouldThrowBusinessLogicException()
		{
			//Arrange
			UnitOfWork.ShouldThrowPersistenceException = true;
			var interfaceVO = CreateInterfaceVO();

			try
			{
				await Service.CreateComponentInterface(interfaceVO);
			}
			catch (BusinessLogicException)
			{
				return;
			}
			Assert.Fail();
		}

		[Test]
		public async void CreateComponentInterface_ArgumentNameNotused_ShouldAddNewInterface()
		{
			//Arrange
			var interfaceVO = CreateInterfaceVO();
			ComponentInterface savedInterface = null;
			RepositoryMock.Setup(x => x.Save(It.IsAny<ComponentInterface>()))
				.Callback(new Action<ComponentInterface>(x => { savedInterface = x; }));

			//Act
			await Service.CreateComponentInterface(interfaceVO);

			//Assert
			Assert.That(savedInterface != null);
			Assert.That(savedInterface.Name == interfaceVO.Name);
		}

		private static ComponentInterfaceVO CreateInterfaceVO()
		{
			return new ComponentInterfaceVO {Name = NamesGenerator.ComponentInterfaceName()};
		}
	}

	[TestFixture]
	public class ComponentInterfaceService_GetComponentInterfacesTests : ComponentInterfaceServiceTests
	{
		private const int ListSize = 15;
		private const int PageSize = 5;

		[Test]
		public void GetComponentInterfaces_NullParameters_ShouldThrowArgumentNullException()
		{
			Assert.That(() => Service.GetComponentInterfaces(null),
				Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void GetComponentInterfaces_NotNullParameters_ShouldReturnResultFilteredBySpecifiedParameters()
		{
			//Arrange
			var intsList = FillRepositoryWithFakes(ListSize);
			var requestParameters = CreateRequestForPage(1);
			var expectedResult = CreatePagedResult(1, intsList.OrderByDescending(x => x.Name));

			//Act
			var actualResult = Service.GetComponentInterfaces(requestParameters).Result;

			//Assert
			AssertResultsEqual(expectedResult, actualResult);
		}

		[Test]
		public void GetComponentInterfaces_RequestedPageGreaterThanTotalNumberOfPages_ShouldReturnLastPage()
		{
			//Arrange
			var intsList = FillRepositoryWithFakes(ListSize);
			var requestParameters = CreateRequestForPage(5);
			var expectedResult = CreatePagedResult(2, intsList.OrderByDescending(x => x.Name));

			//Act
			var actualResult = Service.GetComponentInterfaces(requestParameters).Result;

			//Assert
			AssertResultsEqual(expectedResult, actualResult);
		}

		[Test]
		public void GetComponentInterfaces_NoInterfacesExist_ShouldReturnEmptyResult()
		{
			//Arrange
			FillRepositoryWithFakes(0);
			var requestParameters = CreateRequestForPage(5);
			var expectedResult = new PagedResult<ComponentInterface>(new PagingParameters(0, 0), 0,
				new List<ComponentInterface>());

			//Act
			var actualResult = Service.GetComponentInterfaces(requestParameters).Result;

			//Assert
			AssertResultsEqual(expectedResult, actualResult);
		}

		[Test]
		public void GetComponentInterfaces_SortByNotExistingColumn_ShouldThrowInvalidInputException()
		{
			//Arrange
			FillRepositoryWithFakes(ListSize);
			var requestParameters = new TableParameters(new PagingParameters(0, 5),
				new OrderingParameters(Guid.NewGuid().ToString(), SortDirection.Ascending));

			//Assert
			UtilsAssert.AssertThrowsAggregateException<InvalidInputException>(
				() => Service.GetComponentInterfaces(requestParameters).Wait());
		}

		private static PagedResult<T> CreatePagedResult<T>(int pageNumber, IEnumerable<T> intsList)
		{
			return new PagedResult<T>(new PagingParameters(pageNumber, PageSize), ListSize,
				intsList.Skip(pageNumber*PageSize).Take(PageSize).ToList());
		}

		private static void AssertResultsEqual(PagedResult<ComponentInterface> expectedResult,
			PagedResult<ComponentInterfaceVO> actualResult)
		{
			Assert.That(expectedResult.CountTotal == actualResult.CountTotal);
			Assert.That(expectedResult.PagingParameters.PageNumber == actualResult.PagingParameters.PageNumber);
			Assert.That(expectedResult.PagingParameters.PageSize == actualResult.PagingParameters.PageSize);

			Assert.That(UtilsAssert.CollectionsEqual(
				new ReadOnlyCollection<ComponentInterface>(expectedResult.Items),
				new ReadOnlyCollection<ComponentInterfaceVO>(actualResult.Items),
				(a, b) => a.Name == b.Name));
		}

		private static TableParameters CreateRequestForPage(int pageNumber)
		{
			var requestParameters = new TableParameters(
				new PagingParameters(pageNumber, PageSize),
				new OrderingParameters(ExpressionReflection.Property<ComponentInterface>(x => x.Name).Name, SortDirection.Descending));
			return requestParameters;
		}

		private List<ComponentInterface> FillRepositoryWithFakes(int fakesCount)
		{
			var intsList = new List<ComponentInterface>();
			for (var i = 0; i < fakesCount; i++)
				intsList.Add(DomainObjectsCreator.CreateInterface(i));
			RepositoryMock.Setup(x => x.Query(It.IsAny<PersistenceAwareSpecification<ComponentInterface>>()))
				.Returns(intsList.AsAsyncQueryable());
			return intsList;
		}
	}

	[TestFixture]
	public class ComponentInterfaceService_GetComponentInterfaceTests : ComponentInterfaceServiceTests
	{
		[Test]
		public void GetComponentInterface_EmptyId_ShouldThrowArgumentException()
		{
			Assert.That(() => Service.GetComponentInterface(Guid.Empty), Throws.ArgumentException);
		}

		[Test]
		public void GetComponentInterface_ComponentExists_ShouldReturnVOOfThatComponent()
		{
			//Arrange
			var dbComponent = DomainObjectsCreator.CreateInterface(1).WithId(Guid.NewGuid());
			RepositoryMock.Setup(x => x.Query(It.IsAny<PersistenceAwareSpecification<ComponentInterface>>()))
				.Returns(new List<ComponentInterface> {dbComponent}.AsAsyncQueryable());

			//Act
			var componentVO = Service.GetComponentInterface(dbComponent.Id).Result;

			//Assert
			Assert.That(componentVO.Id == dbComponent.Id);
		}

		[Test]
		public void GetComponentInterface_ComponentExists_ShouldThrowNotFoundException()
		{
			//Arrange
			RepositoryMock.Setup(x => x.Query(It.IsAny<PersistenceAwareSpecification<ComponentInterface>>()))
				.Returns(new List<ComponentInterface>().AsAsyncQueryable());

			//Assert
			UtilsAssert.AssertThrowsAggregateException<NotFoundException, ComponentInterfaceVO>(
				() => Service.GetComponentInterface(Guid.NewGuid()).Result);
		}
	}
}