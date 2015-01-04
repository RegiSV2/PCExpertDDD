using FluentValidation;
using PCExpert.Core.Domain.Resources;

namespace PCExpert.Core.Domain.Validation
{
	public class ComponentCharacteristicValidator<T> : AbstractValidator<T>
		where T : ComponentCharacteristic
	{
		public ComponentCharacteristicValidator()
		{
			RuleFor(x => x.Name).Must(x => x.Length < 250)
				.WithLocalizedMessage(() => ValidationMessages.CharacteristicNameTooLongMsg);
			RuleFor(x => x.Name).Must(x => x.Length >= 10)
				.WithLocalizedMessage(() => ValidationMessages.CharacteristicNameTooShortMsg);
		}
	}
}