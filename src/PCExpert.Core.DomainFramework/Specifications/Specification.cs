using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PCExpert.Core.DomainFramework.Specifications.Logic;

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

		public static Specification<TEntity> operator &(
			Specification<TEntity> spec1,
			Specification<TEntity> spec2)
		{
			return SpecificationLogic.And(spec1, spec2);
		}
	}
}