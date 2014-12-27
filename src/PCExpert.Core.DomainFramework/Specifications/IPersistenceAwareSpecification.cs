using System;
using System.Linq.Expressions;

namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Specification that can be applied to persisted entities
	/// </summary>
	/// <typeparam name="TEntity">Type, the specification is defined for</typeparam>
	public interface IPersistenceAwareSpecification<TEntity> : ISpecification<TEntity>
		where TEntity : class
	{
		/// <summary>
		///     Gets an expression that represents the specification's condition
		/// </summary>
		Expression<Func<TEntity, bool>> GetConditionExpression();
	}
}