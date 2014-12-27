using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Specifications.Logic
{
	/// <summary>
	/// Extensions for specification logic
	/// </summary>
	public static class SpecificationLogic
	{
		public static Specification<TEntity> And<TEntity>(params Specification<TEntity>[] specifications)
			where TEntity : Entity
		{
			Argument.NotNull(specifications);

			return new AndSpecification<TEntity>(specifications);
		}

		public static PersistenceAndSpecification<TEntity> And<TEntity>(
			params PersistenceAwareSpecification<TEntity>[] specifications)
			where TEntity : Entity
		{
			Argument.NotNull(specifications);

			return new PersistenceAndSpecification<TEntity>(specifications);
		}
	}
}
