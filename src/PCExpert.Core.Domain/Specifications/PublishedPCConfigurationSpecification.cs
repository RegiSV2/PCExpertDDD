using System.Linq;

namespace PCExpert.Core.Domain.Specifications
{
	public class PublishedPCConfigurationSpecification : ISpecification<PCConfiguration>
	{
		private readonly ComponentType[] _exactlyOneComponentTypes =
		{
			ComponentType.Motherboard,
			ComponentType.PowerSupply
		};

		private readonly ComponentType[] _requiredAndCanHaveMoreThanOneComponentTypes =
		{
			ComponentType.CentralProcessingUnit,
			ComponentType.HardDiskDrive,
			ComponentType.RandomAccessMemory,
			ComponentType.VideoCard
		};

		public bool IsSatisfied(PCConfiguration configuration)
		{
			return !string.IsNullOrEmpty(configuration.Name)
			       && _exactlyOneComponentTypes.All(x => configuration.Components.Count(y => y.Type == x) == 1)
			       && _requiredAndCanHaveMoreThanOneComponentTypes.All(x => configuration.Components.Any(y => y.Type == x));
		}
	}
}