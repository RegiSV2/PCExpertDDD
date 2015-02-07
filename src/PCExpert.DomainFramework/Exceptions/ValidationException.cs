using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	/// Represents entity validation exception
	/// </summary>
	public class ValidationException : Exception
	{
		public ValidationException(string message)
			: base(message)
		{
		}

		public ValidationException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected ValidationException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}