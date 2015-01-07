using System.Collections.Generic;
using FluentValidation.Results;

namespace PCExpert.Core.DomainFramework.Validation
{
	public interface ISpecificationDetailsInterpreter<in TDetails>
	{
		IEnumerable<ValidationFailure> InterpretSpecificationResultDetails(TDetails details);
	}
}