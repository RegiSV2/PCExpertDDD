using FluentValidation;
using PCExpert.Core.Domain.Specifications;
using PCExpert.DomainFramework.Validation;

namespace PCExpert.Core.Domain.Validation
{
	public sealed class PCConfigurationValidator : AbstractValidator<PCConfiguration>
	{
		public PCConfigurationValidator(PublishedPCConfigurationSpecification publishedSpecification,
			ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails> publishedDetailsInterpreter)
		{
			RuleFor(x => x.Status).NotEqual(PCConfigurationStatus.Undefined);
			When(x => x.Name != null, () => { this.RuleForNameLength(x => x.Name, 3, 250); });
			When(x => x.Status == PCConfigurationStatus.Published, () =>
			{
				AddRule(new DetailedSpecificationValidationRule<PCConfiguration, IPublishedPCConfigurationCheckDetails>(
					publishedSpecification, publishedDetailsInterpreter));
			});
		}
	}
}