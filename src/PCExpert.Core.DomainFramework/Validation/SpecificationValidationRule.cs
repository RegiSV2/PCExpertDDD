using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Resources;
using FluentValidation.Results;
using FluentValidation.Validators;
using PCExpert.Core.DomainFramework.Specifications;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Validation
{
	/// <summary>
	/// Adapter for Specification that allows to use specifications in the context of validation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SpecificationValidationRule<T> : IValidationRule
		where T : class
	{
		private readonly Specification<T> _specification;

		private readonly string _errorMessage;

		private Func<T, bool> _condition = x => true;

		public SpecificationValidationRule(Specification<T> specification, string errorMessage)
		{
			Argument.NotNull(specification);
			Argument.NotNull(errorMessage);

			_specification = specification;
			_errorMessage = errorMessage;
		}

		public IEnumerable<ValidationFailure> Validate(ValidationContext context)
		{
			var instanceToValidate = (T)context.InstanceToValidate;
			if (!ShouldValidate(instanceToValidate) || _specification.IsSatisfiedBy(instanceToValidate))
				return Enumerable.Empty<ValidationFailure>();
			return Enumerable.Repeat(new ValidationFailure(null, _errorMessage), 1);
		}

		private bool ShouldValidate(T instanceToValidate)
		{
			return _condition(instanceToValidate);
		}

		public Task<IEnumerable<ValidationFailure>> ValidateAsync(ValidationContext context)
		{
			throw new NotImplementedException();
		}

		public void ApplyCondition(Func<object, bool> predicate, ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
		{
			Argument.NotNull(predicate);
			_condition = predicate;
		}

		public IEnumerable<IPropertyValidator> Validators
		{
			get { return Enumerable.Empty<IPropertyValidator>(); }
		}

		public string RuleSet { get; set; }
	}
}
