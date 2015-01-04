using FluentValidation;
using PCExpert.Core.Domain.Resources;

namespace PCExpert.Core.Domain.Validation
{
	public class CharacteristicValueValidator<T> : AbstractValidator<T>
		where T : CharacteristicValue
	{
		public CharacteristicValueValidator()
		{
			RuleFor(x => x.Component).NotNull()
				.WithLocalizedMessage(() => ValidationMessages.CharacteristicValueShouldBeAttachedMsg);
		}
	}
}