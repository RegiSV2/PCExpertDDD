using NUnit.Framework;

namespace PCExpert.DomainFramework.Tests.Specifications.Logic
{
	[TestFixture]
	public class LogicSpecificationsTests
	{
		protected TestEntity Entity;

		[SetUp]
		public void EstablishContext()
		{
			Entity = new TestEntity(1, 2);
		}

		protected class TestEntity : Entity
		{
			public TestEntity(int a, int b)
			{
				A = a;
				B = b;
			}

			public int A { get; private set; }
			public int B { get; private set; }
		}
	}
}