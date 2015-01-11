namespace PCExpert.DomainFramework.Specifications
{
	/// <summary>
	///     Represents specification that can provide some detailed feedback about entity check
	/// </summary>
	/// <typeparam name="TEntity">Type, the specification is defined for</typeparam>
	/// <typeparam name="TCheckData">Type of check feedback</typeparam>
	public interface IDetailedSpecification<in TEntity, TCheckData>
		where TEntity : class
	{
		SpecificationDetailedCheckResult<TCheckData> IsSatisfiedBy(TEntity entity);
	}
}