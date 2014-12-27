using NUnit.Framework;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Tests.Specifications
{
	[TestFixture]
	public class PCConfigurationSpecificationsTests<TSpec>
		where TSpec : Specification<PCConfiguration>
	{
		protected PCConfiguration Configuration;
		protected TSpec Specification;

		[SetUp]
		public virtual void EstablishContext()
		{
			Configuration = new PCConfiguration();
		}
	}
}