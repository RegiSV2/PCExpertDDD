using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	///     Thrown when trying to add element to collection that has already been added
	/// </summary>
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