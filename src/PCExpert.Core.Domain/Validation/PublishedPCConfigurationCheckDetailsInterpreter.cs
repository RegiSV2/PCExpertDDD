using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentValidation.Results;
using PCExpert.Core.Domain.Resources;
using PCExpert.Core.Domain.Specifications;
using PCExpert.DomainFramework.Utils;
using PCExpert.DomainFramework.Validation;

namespace PCExpert.Core.Domain.Validation
{
	public sealed class PublishedPCConfigurationCheckDetailsInterpreter
		: ISpecificationDetailsInterpreter<IPublishedPCConfigurationCheckDetails>
	{
		private static string ConfigNameProperty
		{
			get { return ExpressionReflection.Property<PCConfiguration>(x => x.Name).Name; }
		}

		public IEnumerable<ValidationFailure> Interpret(IPublishedPCConfigurationCheckDetails details)
		{
			if (details.NameNotEmptyFailure)
				yield return ConfigNameFailure(ValidationMessages.ConfigNameCannotBeEmptyMsg);
			if (details.NameMaxLengthFailure)
				yield return ConfigNameFailure(ValidationMessages.ConfigNameTooLongMsg);
			if (details.ComponentsCycleFailure)
				yield return Failure(ValidationMessages.ConfigCyclicPluggingMsg);
			if (details.RequiredButNotAddedTypes.Any())
				yield return CreateRequiredButNotAddedTypesFailure(details.RequiredButNotAddedTypes);
			if (details.TypesViolatedUniqueConstraint.Any())
				yield return CreateTypesViolatedUniqueConstraintFailure(details);
			foreach (var failure in details.NotFoundInterfaces.Select(CreateNotFoundInterfaceFailure))
				yield return failure;
		}

		private static ValidationFailure CreateTypesViolatedUniqueConstraintFailure(
			IPublishedPCConfigurationCheckDetails details)
		{
			var message = string.Format(ValidationMessages.ConfigTypesViolatedUniqueConstraintMsg,
				details.TypesViolatedUniqueConstraint.Select(x => x.ToString())
					.ConcatToFriendlyEnumeration(CultureInfo.CurrentCulture));
			return Failure(message);
		}

		private static ValidationFailure CreateNotFoundInterfaceFailure(InterfaceDeficitInfo notFoundInfo)
		{
			var message =
				notFoundInfo.Deficit == 1
					? string.Format("Configuration lacks one \"{0}\" interface, required by {1}",
						notFoundInfo.ProblemInterface.Name,
						ComposeComponentsToString(notFoundInfo.RequiredByComponents))
					: string.Format("Configuration lacks {0} \"{1}\" interfaces, required by {2}",
						notFoundInfo.Deficit, notFoundInfo.ProblemInterface.Name,
						ComposeComponentsToString(notFoundInfo.RequiredByComponents));
			return Failure(message);
		}

		private static ValidationFailure CreateRequiredButNotAddedTypesFailure(List<ComponentType> componentTypes)
		{
			var message =
				componentTypes.Count == 1
					? string.Format(ValidationMessages.ConfigHasRequiredButNotAddedTypeMsg,
						componentTypes.First())
					: string.Format(ValidationMessages.ConfigHasRequiredButNotAddedTypesMsg,
						componentTypes.Select(x => x.ToString())
							.ConcatToFriendlyEnumeration(CultureInfo.CurrentCulture));
			return Failure(message);
		}

		private static ValidationFailure ConfigNameFailure(string message)
		{
			return new ValidationFailure(ConfigNameProperty, message);
		}

		private static ValidationFailure Failure(string message)
		{
			return new ValidationFailure(null, message);
		}

		private static string ComposeComponentsToString(List<PCComponent> list)
		{
			return list.Select(x => x.Type).Distinct()
				.Select(x => x.ToString())
				.ConcatToFriendlyEnumeration(CultureInfo.CurrentCulture);
		}
	}
}