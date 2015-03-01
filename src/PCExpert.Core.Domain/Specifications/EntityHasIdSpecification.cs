using System;
using System.Linq.Expressions;
using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Specifications;

namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	///     Specifies that entity should have specified id
	/// </summary>
	public class EntityHasIdSpecification<TEntity> : PersistenceAwareSpecification<TEntity>
		where TEntity : Entity
	{
		private readonly Guid _id;

		public EntityHasIdSpecification(Guid id)
		{
			_id = id;
		}

		public override Expression<Func<TEntity, bool>> GetConditionExpression()
		{
			return x => x.Id == _id;
		}
	}
}