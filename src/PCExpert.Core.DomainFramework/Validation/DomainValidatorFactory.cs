using System;
using System.Collections.Generic;
using FluentValidation;
using PCExpert.Core.DomainFramework.Exceptions;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Validation
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>Not thread-safe</remarks>
	public class DomainValidatorFactory : IValidatorFactory
	{
		private readonly IDictionary<Type, IValidator> _registeredValidators = new Dictionary<Type, IValidator>();
 
		public DomainValidatorFactory AddValidator<T>(IValidator<T> validator)
		{
			Argument.NotNull(validator);

			if (_registeredValidators.ContainsKey(typeof (T)))
				throw new DuplicateElementException(
					string.Format("Validator for type {0} has already been added", typeof (T).FullName));

			_registeredValidators.Add(typeof (T), validator);

			return this;
		}

		public IValidator<T> GetValidator<T>()
		{
			return (IValidator<T>)GetValidator(typeof(T));
		}

		public IValidator GetValidator(Type type)
		{
			IValidator validator = null;
			_registeredValidators.TryGetValue(type, out validator);
			return validator;
		}
	}
}