using System;
using System.Linq.Expressions;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public sealed class ConfigurationNameMaxLengthSpecification : PersistenceAwareSpecification<PCConfiguration>
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