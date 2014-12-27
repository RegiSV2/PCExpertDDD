using System;

namespace PCExpert.Core.DomainFramework
{
	public abstract class Entity
	{
		public virtual Guid Id { get; private set; }

		public bool IsPersisted
		{
			get { return Id != Guid.Empty; }
		}

		public bool SameIdentityAs(Entity otherEntity)
		{
			return otherEntity != null &&
			       (this == otherEntity || (IsPersisted && otherEntity.Id == Id));
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}