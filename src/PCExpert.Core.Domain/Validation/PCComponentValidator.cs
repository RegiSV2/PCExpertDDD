using FluentValidation;
using PCExpert.Core.Domain.Resources;

namespace PCExpert.Core.Domain.Validation
{
	public sealed class PCComponentValidator : AbstractValidator<PCComponent>
	{
		public PCComponentValidator()
		{
			this.RuleForNameLength(x => x.Name, 5, 250);
			RuleFor(x => x.AveragePrice)
				.Must(x => x >= 0).WithLocalizedMessage(() => ValidationMessages.NegativePriceMsg)
				.Must(x => x < 1000000).WithLocalizedMessage(() => ValidationMessages.PriceTooGreateMsg);
		}
	}
}