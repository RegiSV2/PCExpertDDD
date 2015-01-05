using System;
using System.Linq.Expressions;
using FluentValidation;
using PCExpert.Core.Domain.Resources;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.Domain.Validation
{
	public static class ValidationExtensions
	{
		public static void RuleForNameLength<T>(this AbstractValidator<T> validator,
			Expression<Func<T, string>> property, int minLength, int maxLength)
		{
			Argument.NotNull(validator);

			validator.RuleFor(property).Must(x => x.Length >= minLength)
				.WithLocalizedMessage(() => ValidationMessages.NameTooShortMsg);
			validator.RuleFor(property).Must(x => x.Length < maxLength)
				.WithLocalizedMessage(() => ValidationMessages.NameTooLongMsg);
		}
	}
}