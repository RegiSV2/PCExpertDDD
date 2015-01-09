using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FluentValidation.Results;
using PCExpert.Core.DomainFramework.Utils;

namespace PCExpert.Core.DomainFramework.Validation
{
	/// <summary>
	///     Interprets an instance
	///     <value>TDetails</value>
	///     into a sequence of validation failures
	/// </summary>
	/// <typeparam name="TDetails">Type that represents system validation result details</typeparam>
	[ContractClass(typeof (SpecificationDetailsInterpreterContracts<>))]
	public interface ISpecificationDetailsInterpreter<in TDetails>
	{
		IEnumerable<ValidationFailure> Interpret(TDetails details);
	}

	[ContractClassFor(typeof (ISpecificationDetailsInterpreter<>))]
	internal abstract class SpecificationDetailsInterpreterContracts<TDetails> : ISpecificationDetailsInterpreter<TDetails>
	{
		public IEnumerable<ValidationFailure> Interpret(TDetails details)
		{
			Argument.NotNull(details);
			return null;
		}
	}
}