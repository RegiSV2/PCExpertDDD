using System;
using System.Linq;
using System.Linq.Expressions;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.Specifications.Logic
{
	/// <summary>
	///     And specification
	/// </summary>
	public class PersistenceAndSpecification<TEntity> : PersistenceAwareSpecification<TEntity>
		where TEntity : class
	{
		private readonly Expression<Func<TEntity, bool>> _cominedExpression;

		public PersistenceAndSpecification(params PersistenceAwareSpecification<TEntity>[] specifications)
		{
			Argument.NotNull(specifications);

			_cominedExpression = specifications
				.Select(x => x.GetConditionExpression())
				.Aggregate((combined, expr) => combined.And(expr));
		}

		public override Expression<Func<TEntity, bool>> GetConditionExpression()
		{
			return _cominedExpression;
		}
	}
}