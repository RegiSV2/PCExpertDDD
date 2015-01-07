using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Validation
{
	public class DetailedSpecificationValidationRule<T, TDetails> : SpecificationValidationRuleBase<T>
		where T : class
		where TDetails : class

	{
		private readonly ISpecificationDetailsInterpreter<TDetails> _interpreter;
		private readonly IDetailedSpecification<T, TDetails> _specification;

		public DetailedSpecificationValidationRule(IDetailedSpecification<T, TDetails> detailedSpecification,
			ISpecificationDetailsInterpreter<TDetails> interpreter)
		{
			Argument.NotNull(detailedSpecification);
			Argument.NotNull(interpreter);

			_specification = detailedSpecification;
			_interpreter = interpreter;
		}

		protected override IEnumerable<ValidationFailure> DoValidate(T instanceToValidate)
		{
			var specResult = _specification.IsSatisfiedBy(instanceToValidate);
			return specResult.IsSatisfied
				? Enumerable.Empty<ValidationFailure>()
				: _interpreter.InterpretSpecificationResultDetails(specResult.FailureDetails);
		}
	}
}