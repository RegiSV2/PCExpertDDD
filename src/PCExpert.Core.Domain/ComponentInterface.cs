﻿using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Utils;

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

		protected ComponentInterface()
		{
		}

		public ComponentInterface(string name)
		{
			Argument.NotNullAndNotEmpty(name);

			Name = name;
		}

		#endregion
	}
}