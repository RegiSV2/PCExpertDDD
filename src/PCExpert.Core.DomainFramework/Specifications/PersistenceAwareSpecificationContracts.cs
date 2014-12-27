using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace PCExpert.Core.DomainFramework.Specifications
{
	[ContractClassFor(typeof(PersistenceAwareSpecification<>))]
	abstract class PersistenceAwareSpecificationContracts<TEntity> : PersistenceAwareSpecification<TEntity>
		where TEntity : class
	{
		public override Expression<Func<TEntity, bool>> GetConditionExpression()
		{
			Contract.Ensures(Contract.Result<Expression<Func<TEntity, bool>>>() != null);
			return default(Expression<Func<TEntity, bool>>);
		}
	}
}