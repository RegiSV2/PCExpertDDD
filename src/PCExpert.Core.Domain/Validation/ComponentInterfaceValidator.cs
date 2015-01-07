using FluentValidation;

namespace PCExpert.Core.Domain.Validation
{
	public class ComponentInterfaceValidator : AbstractValidator<ComponentInterface>
	{
		public ComponentInterfaceValidator()
		{
			this.RuleForNameLength(x => x.Name, 3, 250);
		}
	}
}