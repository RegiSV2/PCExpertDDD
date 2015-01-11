using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using Moq;

namespace PCExpert.DomainFramework.Tests.Validation
{
	public class ValidationRuleTests
	{
		private const String ValidatedInstance = "some string";
		protected ValidationContext ValidationContext;

		public virtual void EstablishContext()
		{
			ValidationContext = new ValidationContext(ValidatedInstance, new PropertyChain(Enumerable.Empty<String>()),
				new Mock<IValidatorSelector>().Object);
		}
	}
}