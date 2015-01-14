using System;
using System.Runtime.Serialization;

namespace PCExpert.DomainFramework.Exceptions
{
	/// <summary>
	///     Thrown when operation with
	///     <value>PersistenceWorkplace</value>
	///     fails
	/// </summary>
	public class PersistenceException : Exception, ISerializable
	{
		public PersistenceException(string message)
			: base(message)
		{
		}

		public PersistenceException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected PersistenceException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}
	}
}