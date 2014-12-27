using System;
using System.Runtime.Serialization;

namespace PCExpert.Core.DomainFramework.Exceptions
{
	public class DuplicateElementException : ApplicationException, ISerializable
	{
		public DuplicateElementException(string message)
			: base(message)
		{
		}

		public DuplicateElementException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected DuplicateElementException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}