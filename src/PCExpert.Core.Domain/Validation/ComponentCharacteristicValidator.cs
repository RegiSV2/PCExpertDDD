using FluentValidation;

namespace PCExpert.Core.Domain.Validation
{
	public sealed class ComponentCharacteristicValidator<T> : AbstractValidator<T>
		where T : ComponentCharacteristic
	{
		public ComponentCharacteristicValidator()
		{
			this.RuleForNameLength(x => x.Name, 3, 250);
		}
	}
}