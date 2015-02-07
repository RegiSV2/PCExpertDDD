using System;
using System.Linq;
using LightInject;
using Moq;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Web.Api.Common.Tests
{
	[TestFixture]
	public class LightInjectDependencyResolverTests
	{
		private Mock<IServiceContainer> _serviceContainerMock;

		private LightInjectDependencyResolver _resolver;

		[SetUp]
		public void EstablishContext()
		{
			_serviceContainerMock = new Mock<IServiceContainer>();
			_resolver = new LightInjectDependencyResolver(_serviceContainerMock.Object);
		}

		[Test]
		public void Create_NullContainer_ShouldFail()
		{
			Assert.That(() => new LightInjectDependencyResolver(null), Throws.InstanceOf<ArgumentNullException>());
		}

		[Test]
		public void Dispose_ShouldDisposeServiceContainer()
		{
			_serviceContainerMock.Setup(x => x.Dispose()).Verifiable();
			
			_resolver.Dispose();

			_serviceContainerMock.Verify(x => x.Dispose(), Times.Once);
		}

		[Test]
		public void GetService_NotRegisteredType_ShouldReturnNull()
		{
			_serviceContainerMock.Setup(x => x.TryGetInstance(It.IsAny<Type>())).Returns(null);

			Assert.That(_resolver.GetService(typeof(string)) == null);
		}

		[Test]
		public void GetService_RegisteredType_ShouldReturnResolvedInstance()
		{
			const string registeredInstance = "";
			_serviceContainerMock.Setup(x => x.TryGetInstance(typeof(string))).Returns(registeredInstance);

			Assert.That((string) _resolver.GetService(typeof(string)) == registeredInstance);
		}

		[Test]
		public void GetServices_NotRegisteredType_ShouldReturnEmptyEnumerable()
		{
			_serviceContainerMock.Setup(x => x.GetAllInstances(It.IsAny<Type>())).Returns(Enumerable.Empty<Type>());

			Assert.That(_resolver.GetServices(typeof(string)).IsEmpty());
		}

		[Test]
		public void GetServices_RegisteredType_ShouldReturnAllResolvedInstances()
		{
			var resolvedInstances = new[] {"", "a"};
			_serviceContainerMock.Setup(x => x.GetAllInstances(typeof (string))).Returns(resolvedInstances);

			Assert.That(UtilsAssert.CollectionsEqual(
				_resolver.GetServices(typeof (string)).Cast<string>().ToArray(), resolvedInstances,
				(a, b) => a == b));
		}
	}
}