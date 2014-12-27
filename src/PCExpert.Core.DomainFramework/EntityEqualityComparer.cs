using System.Collections.Generic;

namespace PCExpert.Core.DomainFramework
{
	public class EntityEqualityComparer<TEntity> : IEqualityComparer<TEntity>
		where TEntity : Entity
	{
		public bool Equals(TEntity x, TEntity y)
		{
			return x.SameIdentityAs(y);
		}

		public int GetHashCode(TEntity obj)
		{
			return obj.GetHashCode();
		}
	}
}