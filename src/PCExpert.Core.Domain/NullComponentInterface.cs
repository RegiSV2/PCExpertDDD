using System;

namespace PCExpert.Core.Domain
{
	public class NullComponentInterface : ComponentInterface
	{
		public NullComponentInterface() : base(Guid.Empty)
		{
		}

		public override string Name
		{
			get { return ""; }
		}
	}
}