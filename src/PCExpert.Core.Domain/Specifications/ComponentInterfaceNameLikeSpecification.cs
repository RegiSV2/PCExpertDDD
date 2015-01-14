using System;
using System.Linq.Expressions;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	public class ComponentInterfaceNameContainsSpecification
		: PersistenceAwareSpecification<ComponentInterface>
	{
		private readonly string _substring;

		public ComponentInterfaceNameContainsSpecification(string substring)
		{
			_substring = substring ?? "";
		}

		public override Expression<Func<ComponentInterface, bool>> GetConditionExpression()
		{
			return x => x.Name.Contains(_substring);
		}
	}
}