using System;
using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Specifications;

namespace PCExpert.Core.Tests.Utils
{
	public class TestSpec<TTestEntity> : Specification<TTestEntity>
		where TTestEntity : Entity
	{
		private readonly Func<TTestEntity, bool> _expression;

		public TestSpec(Func<TTestEntity, bool> expression)
		{
			_expression = expression;
		}

		public override bool IsSatisfiedBy(TTestEntity entity)
		{
			return _expression(entity);
		}
	}
}