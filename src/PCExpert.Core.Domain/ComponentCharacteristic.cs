using PCExpert.DomainFramework;
using PCExpert.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	public abstract class ComponentCharacteristic : Entity
	{
		#region Public Methods

		public ComponentCharacteristic WithPattern(string pattern)
		{
			Argument.NotNullAndNotEmpty(pattern);

			FormattingPattern = pattern;

			return this;
		}

		#endregion

		#region Constructors

		protected ComponentCharacteristic()
		{
		}

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

		/// <summary>
		///     A pattern that should be used for formatting CharacteristicValue to user-friendly view
		/// </summary>
		public string FormattingPattern { get; private set; }

		#endregion
	}
}