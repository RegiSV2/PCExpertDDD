using System;
using System.Linq.Expressions;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public class ConfigurationNameNotEmptySpecification : PersistenceAwareSpecification<PCConfiguration>
	{
		public override Expression<Func<PCConfiguration, bool>> GetConditionExpression()
		{
			return x => !string.IsNullOrEmpty(x.Name);
		}
	}
}