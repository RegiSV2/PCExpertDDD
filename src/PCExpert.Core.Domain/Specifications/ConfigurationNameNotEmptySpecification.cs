using System;
using System.Linq.Expressions;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public sealed class ConfigurationNameNotEmptySpecification : PersistenceAwareSpecification<PCConfiguration>
	{
		public override Expression<Func<PCConfiguration, bool>> GetConditionExpression()
		{
			return x => !string.IsNullOrEmpty(x.Name);
		}
	}
}