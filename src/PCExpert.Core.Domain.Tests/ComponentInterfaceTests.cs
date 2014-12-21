using NUnit.Framework;
using PCExpert.Core.Domain.Tests.Utils;

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
	}
}