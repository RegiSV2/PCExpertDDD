using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	internal sealed class ComponentPlugContraversions
	{
		public ComponentPlugContraversions(bool canPlugWithoutCycles, IEnumerable<InterfaceDeficitInfo> notFoundInterfaces)
		{
			CanPlugWithoutCycles = canPlugWithoutCycles;
			NotFoundInterfaces = new List<InterfaceDeficitInfo>(notFoundInterfaces);
		}

		public bool CanPlugWithoutCycles { get; private set; }
		public List<InterfaceDeficitInfo> NotFoundInterfaces { get; private set; }
	}
}