using System;

namespace PCExpert.Core.Domain
{
	public abstract class Entity
	{
		protected Entity(Guid id)
		{
			Id = id;
		}

		protected Entity()
			: this(Guid.Empty)
		{
		}

		public virtual Guid Id { get; private set; }

		public bool SameIdentityAs(Entity otherEntity)
		{
			return otherEntity != null && otherEntity.Id == Id;
		}
	}
}