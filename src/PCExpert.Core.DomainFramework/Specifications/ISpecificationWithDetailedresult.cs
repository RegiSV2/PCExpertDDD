namespace PCExpert.Core.DomainFramework.Specifications
{
	/// <summary>
	///     Represents specification that can provide some detailed feedback about entity check
	/// </summary>
	/// <typeparam name="TEntity">Type, the specification is defined for</typeparam>
	/// <typeparam name="TCheckData">Type of check feedback</typeparam>
	public interface ISpecificationWithDetailedresult<in TEntity, TCheckData>
		where TEntity : Entity
	{
		SpecificationDetailedCheckResult<TCheckData> IsSatisfiedBy(TEntity entity);
	}
}