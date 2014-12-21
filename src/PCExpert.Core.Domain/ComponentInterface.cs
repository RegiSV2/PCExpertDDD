using System;

namespace PCExpert.Core.Domain
{
	public class ComponentInterface : Entity
	{
		#region Null Object

		public static readonly ComponentInterface NullObject = new NullComponentInterface();

		#endregion

		#region Properties

		public virtual string Name { get; private set; }

		#endregion

		#region Constructors

		protected ComponentInterface(Guid id)
			: base(id)
		{
		}

		public ComponentInterface(string name)
		{
			Name = name;
		}

		#endregion
	}
}