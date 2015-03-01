using NUnit.Framework;
using PCExpert.Core.Application.Impl;

namespace PCExpert.Core.Application.Tests
{
	public class BaseServiceTests
	{
		[SetUp]
		public virtual void EstablishContext()
		{
			MappersConfig.Configure();
		}
	}
}