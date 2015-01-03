using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain
{
	/// <summary>
	/// Represents a value of some characteristic for a <value>PCComponent</value>
	/// </summary>
	public abstract class CharacteristicValue
	{
		#region Constructors

		protected CharacteristicValue()
		{ }

		protected CharacteristicValue(ComponentCharacteristic characteristic)
		{
			Argument.NotNull(characteristic);

			Characteristic = characteristic;
			CharacteristicId = characteristic.Id;
		}

		#endregion

		#region Properties

		public PCComponent Component { get; private set; }

		public ComponentCharacteristic Characteristic { get; private set; }

		public Guid ComponentId { get; private set; }

		public Guid CharacteristicId { get; private set; }

		#endregion

		public string Format(CultureInfo cultureInfo)
		{
			Argument.NotNull(cultureInfo);
			return DoFormat(cultureInfo);
		}

		public void AttachToComponent(PCComponent component)
		{
			Argument.NotNull(component);
			if(Component != null)
				throw new InvalidOperationException();

			Component = component;
			ComponentId = component.Id;
		}

		protected abstract string DoFormat(CultureInfo cultureInfo);
	}
}