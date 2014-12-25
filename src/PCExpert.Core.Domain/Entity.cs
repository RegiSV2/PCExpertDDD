using System;

namespace PCExpert.Core.Domain
{
	public abstract class Entity
	{
		public virtual Guid Id { get; private set; }

		public bool SameIdentityAs(Entity otherEntity)
		{
			return otherEntity != null &&
			       (this == otherEntity || (IsPersisted && otherEntity.Id == Id));
		}

		public bool IsPersisted
		{
			get { return Id != Guid.Empty; }
		}
	}
}