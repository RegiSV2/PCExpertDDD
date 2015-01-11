using System;
using System.Linq.Expressions;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Tests.Utils
{
	public class TestPersistenceAwareSpec<TTestEntity> : PersistenceAwareSpecification<TTestEntity>
		where TTestEntity : Entity
	{
		private readonly Expression<Func<TTestEntity, bool>> _expression;

		public TestPersistenceAwareSpec(Expression<Func<TTestEntity, bool>> expression)
		{
			_expression = expression;
		}

		public override Expression<Func<TTestEntity, bool>> GetConditionExpression()
		{
			return _expression;
		}
	}
}