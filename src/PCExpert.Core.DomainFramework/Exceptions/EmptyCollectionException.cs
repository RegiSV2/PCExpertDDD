using System;
using System.Runtime.Serialization;

namespace PCExpert.Core.DomainFramework.Exceptions
{
	/// <summary>
	///     Thrown when empty collection is passed to context that doesn't allow empty collection
	/// </summary>
	public class EmptyCollectionException : ApplicationException, ISerializable
	{
		public EmptyCollectionException(string message)
			: base(message)
		{
		}

		public EmptyCollectionException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected EmptyCollectionException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}