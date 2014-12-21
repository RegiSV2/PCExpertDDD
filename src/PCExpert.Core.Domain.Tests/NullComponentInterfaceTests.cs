using NUnit.Framework;

namespace PCExpert.Core.Domain.Tests
{
	[TestFixture]
	public class NullComponentInterfaceTests
	{
		[Test]
		public void NullInterface_ShouldReturnComponentInterfaceWithEmptyProperties()
		{
			//Arrange
			var nullInterface = ComponentInterface.NullObject;

			//Assert
			Assert.That(nullInterface.Name, Is.EqualTo(string.Empty));
		}
	}
}