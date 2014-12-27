using System;
using System.Linq.Expressions;

namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Supertype for persistence-aware specifications
	/// </summary>
	/// <remarks>Not thread-safe</remarks>
	public abstract class PersistenceAwareSpecification<TEntity> : IPersistenceAwareSpecification<TEntity>
		where TEntity : class
	{
		private Func<TEntity, bool> _compiledFunc;

		public bool IsSatisfiedBy(TEntity entity)
		{
			if (_compiledFunc == null)
				_compiledFunc = GetConditionExpression().Compile();
			return _compiledFunc.Invoke(entity);
		}

		public abstract Expression<Func<TEntity, bool>> GetConditionExpression();
	}
}