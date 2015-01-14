using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class ComponentInterfaceNameContainsSpecificationTests
	{
		private List<ComponentInterface> _interfaces;
		private ComponentInterfaceNameContainsSpecification _specification;

		[SetUp]
		public void EstablishContext()
		{
			_interfaces = Enumerable.Range(1, 3).Select(DomainObjectsCreator.CreateInterface).ToList();
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void IsSpecified_NullOrEmptyArgument_ShouldNotFilterData(string arg)
		{
			//Arrange
			_specification = new ComponentInterfaceNameContainsSpecification(arg);

			//Assert
			Assert.That(_interfaces.Count == FilterInterfaces().Count);
		}

		[Test]
		public void IsSpecified_NotEmptyArgument_ShouldFilterData()
		{
			//Arrange
			_specification = new ComponentInterfaceNameContainsSpecification("2");

			//Assert
			Assert.That(FilterInterfaces().Count == 1);
		}

		private List<ComponentInterface> FilterInterfaces()
		{
			return _interfaces.Where(_specification.IsSatisfiedBy).ToList();
		}
	}
}