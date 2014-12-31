using PCExpert.Core.DomainFramework;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	public class ComponentCharacteristic : Entity
	{
		#region Constructors

		public ComponentCharacteristic(string name, ComponentType type)
		{
			Argument.NotNullAndNotEmpty(name);
			Argument.ValidEnumItem(type);

			Name = name;
			ComponentType = type;
		}

		#endregion

		#region Properties

		public string Name { get; private set; }

		public ComponentType ComponentType { get; private set; }

		#endregion

	}
}