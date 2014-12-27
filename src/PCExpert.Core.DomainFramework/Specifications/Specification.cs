namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Represents some business rule
	/// </summary>
	/// <typeparam name="TEntity">Type, the specification is defined for</typeparam>
	public abstract class Specification<TEntity>
		where TEntity : class
	{
		/// <summary>
		///     Checks if the entity satisfies the specification
		/// </summary>
		/// <returns>True, if the entity satisfies the specification</returns>
		public abstract bool IsSatisfiedBy(TEntity entity);
	}
}