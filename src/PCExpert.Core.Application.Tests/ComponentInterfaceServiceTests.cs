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
	[TestFixture]
	public class ComponentInterfaceServiceTests
	{
		private const int ListSize = 15;
		private const int PageSize = 5;
		private Mock<IComponentInterfaceRepository> _repositoryMock;
		private IComponentInterfaceService _service;
		private FakeUnitOfWork _unitOfWork;

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
				.Callback(new Action<ComponentInterface>(x => { savedInterface = x; }));

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
			//Arrange
			var intsList = FillRepositoryWithFakes(ListSize);
			var requestParameters = CreateRequestForPage(1);
			var expectedResult = CreatePagedResult(1, intsList.OrderByDescending(x => x.Name));

			//Act
			var actualResult = _service.GetComponentInterfaces(requestParameters).Result;

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
			var actualResult = _service.GetComponentInterfaces(requestParameters).Result;

			//Assert
			AssertResultsEqual(expectedResult, actualResult);
		}

		[Test]
		public void GetComponentInterfaces_NoInterfacesExist_ShouldReturnEmptyResult()
		{
			//Arrange
			FillRepositoryWithFakes(0);
			var requestParameters = CreateRequestForPage(5);
			var expectedResult = new PagedResult<ComponentInterface>(new PagingParameters(0, 0), 0, new List<ComponentInterface>());

			//Act
			var actualResult = _service.GetComponentInterfaces(requestParameters).Result;

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
			try
			{
				_service.GetComponentInterfaces(requestParameters).Wait();
				Assert.Fail();
			}
			catch (AggregateException ex)
			{
				Assert.That(ex.InnerExceptions.First() is InvalidInputException);
			}
			catch (Exception)
			{
				Assert.Fail();
			}
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
			_repositoryMock.Setup(x => x.Query(It.IsAny<PersistenceAwareSpecification<ComponentInterface>>()))
				.Returns(intsList.AsAsyncQueryable());
			return intsList;
		}

		private static ComponentInterfaceVO CreateInterfaceVO()
		{
			return new ComponentInterfaceVO {Name = NamesGenerator.ComponentInterfaceName()};
		}
	}
}