using System;
using System.Linq.Expressions;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public class ConfigurationNameMaxLengthSpecification : PersistenceAwareSpecification<PCConfiguration>
	{
		private readonly int _maxLength;

		public ConfigurationNameMaxLengthSpecification(int maxLength)
		{
			_maxLength = maxLength;
		}

		public override Expression<Func<PCConfiguration, bool>> GetConditionExpression()
		{
			return x => x.Name == null || x.Name.Length <= _maxLength;
		}
	}
}