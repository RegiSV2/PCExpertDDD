using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Supertype for persistence-aware specifications
	/// </summary>
	/// <remarks>Not thread-safe</remarks>
	[ContractClass(typeof(PersistenceAwareSpecificationContracts<>))]
	public abstract class PersistenceAwareSpecification<TEntity> : Specification<TEntity>
		where TEntity : class
	{
		private Func<TEntity, bool> _compiledFunc;

		public override bool IsSatisfiedBy(TEntity entity)
		{
			if (_compiledFunc == null)
				_compiledFunc = GetConditionExpression().Compile();
			return _compiledFunc.Invoke(entity);
		}

		public abstract Expression<Func<TEntity, bool>> GetConditionExpression();
	}
}