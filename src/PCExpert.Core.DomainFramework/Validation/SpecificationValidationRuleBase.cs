using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace PCExpert.Core.DomainFramework.Validation
{
	/// <summary>
	///     Base class for specification-based validation rules
	/// </summary>
	/// <typeparam name="T">Validated type</typeparam>
	public abstract class SpecificationValidationRuleBase<T> : IValidationRule
		where T : class
	{
		private Func<T, bool> _condition = x => true;

		public IEnumerable<ValidationFailure> Validate(ValidationContext context)
		{
			var instanceToValidate = (T) context.InstanceToValidate;
			if (_condition(instanceToValidate))
				return DoValidate(instanceToValidate);
			return Enumerable.Empty<ValidationFailure>();
		}

		public Task<IEnumerable<ValidationFailure>> ValidateAsync(ValidationContext context)
		{
			throw new NotImplementedException();
		}

		public void ApplyCondition(Func<object, bool> predicate,
			ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
		{
			if (predicate == null)
				throw new ArgumentNullException();

			_condition = predicate;
		}

		public IEnumerable<IPropertyValidator> Validators
		{
			get { return Enumerable.Empty<IPropertyValidator>(); }
		}

		public string RuleSet { get; set; }
		protected abstract IEnumerable<ValidationFailure> DoValidate(T instanceToValidate);
	}
}