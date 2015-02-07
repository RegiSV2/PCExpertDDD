using System.Collections.Generic;
using PCExpert.Core.Domain;

namespace PCExpert.Core.DataAccess.Tests
{
	public sealed class PCExpertModel
	{
		public List<PCComponent> Components { get; set; }
		public List<ComponentInterface> Interfaces { get; set; }
		public List<PCConfiguration> Configurations { get; set; }
		public List<ComponentCharacteristic> Characteristics { get; set; }
	}
}