using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	/// Represents situation when nothing was found, when the operation is expected to find something
	/// </summary>
	public class NotFoundException : Exception
	{
		public NotFoundException(string message)
			: base(message)
		{
		}

		public NotFoundException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected NotFoundException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}