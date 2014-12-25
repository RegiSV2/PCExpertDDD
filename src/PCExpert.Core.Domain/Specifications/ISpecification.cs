namespace PCExpert.Core.Domain.Specifications
{
	/// <summary>
	/// Represents some business rule
	/// </summary>
	/// <typeparam name="TEntity">Type, the specification is defined for</typeparam>
	public interface ISpecification<in TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Checks if the entity satisfies the specification
		/// </summary>
		/// <returns>True, if the entity satisfies the specification</returns>
		bool IsSatisfied(TEntity entity);
	}
}