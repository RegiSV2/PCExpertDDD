using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Validation
{
	/// <summary>
	///     Adapter for Specification that allows to use specifications in the context of validation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SpecificationValidationRule<T> : SpecificationValidationRuleBase<T>
		where T : class
	{
		private readonly string _errorMessage;
		private readonly Specification<T> _specification;

		public SpecificationValidationRule(Specification<T> specification, string errorMessage)
		{
			Argument.NotNull(specification);
			Argument.NotNull(errorMessage);

			_specification = specification;
			_errorMessage = errorMessage;
		}

		protected override IEnumerable<ValidationFailure> DoValidate(T instanceToValidate)
		{
			return _specification.IsSatisfiedBy(instanceToValidate)
				? Enumerable.Empty<ValidationFailure>()
				: new[] {new ValidationFailure(null, _errorMessage)};
		}
	}
}