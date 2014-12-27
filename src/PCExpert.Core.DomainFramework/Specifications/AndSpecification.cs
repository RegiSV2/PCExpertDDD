using System;

namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     And specification
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class AndSpecification<TEntity> : ISpecification<TEntity>
		where TEntity : Entity
	{
		public bool IsSatisfiedBy(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}