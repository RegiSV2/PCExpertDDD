using FluentValidation;
using PCExpert.Core.Domain.Specifications;
using PCExpert.Core.DomainFramework.Validation;

namespace PCExpert.Core.Domain.Validation
{
	public class PCConfigurationValidator : AbstractValidator<PCConfiguration>
	{
		public PCConfigurationValidator(PublishedPCConfigurationSpecification publishedSpecification)
		{
			RuleFor(x => x.Status).NotEqual(PCConfigurationStatus.Undefined);
			When(x => x.Name != null, () =>
			{
				this.RuleForNameLength(x => x.Name, 3, 250);
			});
			When(x => x.Status == PCConfigurationStatus.Published, () =>
			{
				AddRule(new SpecificationValidationRule<PCConfiguration>(publishedSpecification,
					"Configuration does not satisfy requirements to be published"));
			});
		}
	}
}