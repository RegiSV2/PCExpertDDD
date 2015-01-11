using System.Collections.Generic;

namespace PCExpert.Core.Domain.Specifications
{
	public sealed class InterfaceDeficitInfo
	{
		public InterfaceDeficitInfo(ComponentInterface problemInterface, int deficit,
			IEnumerable<PCComponent> requiredByComponents)
		{
			ProblemInterface = problemInterface;
			Deficit = deficit;
			RequiredByComponents = new List<PCComponent>(requiredByComponents);
		}

		public ComponentInterface ProblemInterface { get; private set; }
		public int Deficit { get; private set; }
		public List<PCComponent> RequiredByComponents { get; private set; }
	}
}