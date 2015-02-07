using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	///     Thrown when some logical failure occurs in business logic.
	///     Should provide user-friendly description of the failure.
	/// </summary>
	public class BusinessLogicException : Exception
	{
		public BusinessLogicException(string message)
			: base(message)
		{
		}

		public BusinessLogicException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected BusinessLogicException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}