using NUnit.Framework;

namespace PCExpert.Core.Application.Tests
{
	[SetUpFixture]
	public class BaseServiceTests
	{
		[SetUp]
		public void SetUp()
		{
			MappersConfig.Configure();
		}
	}
}