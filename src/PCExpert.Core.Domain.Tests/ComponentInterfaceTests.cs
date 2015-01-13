using System;
using NUnit.Framework;
using PCExpert.Core.Tests.Utils;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class ComponentInterfaceTests
	{
		[Test]
		public void Constructor_Called_CanCreateComponentInterface()
		{
			//Arrange
			var name = NamesGenerator.ComponentInterfaceName();
			var componentInterface = new ComponentInterface(name);

			//Assert
			Assert.That(componentInterface.Name, Is.EqualTo(name));
		}

		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Constructor_NullOrEmptyName_ShouldThrowArgumentNullException(string argument)
		{
			Assert.That(() => new ComponentInterface(argument), Throws.InstanceOf<ArgumentNullException>());
		}
	}
}