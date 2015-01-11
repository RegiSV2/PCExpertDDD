using System.Linq;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.DomainFramework.Specifications.Logic
{
	/// <summary>
	///     And specification
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class AndSpecification<TEntity> : Specification<TEntity>
		where TEntity : class
	{
		private readonly Specification<TEntity>[] _conjuctedSpecifications;

		public AndSpecification(params Specification<TEntity>[] conjuctedSpecifications)
		{
			Argument.NotNullAndNotEmpty(conjuctedSpecifications);
			_conjuctedSpecifications = conjuctedSpecifications;
		}

		public override bool IsSatisfiedBy(TEntity entity)
		{
			return _conjuctedSpecifications.All(x => x.IsSatisfiedBy(entity));
		}
	}
}