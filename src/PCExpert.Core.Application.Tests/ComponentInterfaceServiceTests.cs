using System;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Application.Impl;
using PCExpert.Core.Domain.Repositories;

namespace PCExpert.Core.Application.Tests
{
	[TestFixture]
	public class ComponentInterfaceServiceTests
	{
		private Mock<IComponentInterfaceRepository> _repositoryMock;
		private IComponentInterfaceService _service;

		[SetUp]
		public void EstablishContext()
		{
			_repositoryMock = new Mock<IComponentInterfaceRepository>();
			_service = new ComponentInterfaceService(_repositoryMock.Object);
		}

		[Test]
		public void CreateComponentInterface_NullArgument_ShouldThrowArgumentNullException()
		{
			Assert.That(() => _service.CreateComponentInterface(null), Throws.InstanceOf<ArgumentNullException>());
		}
	}
}