using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	/// Thrown when the end-user specifies invalid parameters
	/// </summary>
	public class InvalidInputException : Exception
	{
		public InvalidInputException(string message)
			: base(message)
		{
		}

		public InvalidInputException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected InvalidInputException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}