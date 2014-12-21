﻿using System;
using System.Runtime.Serialization;

namespace PCExpert.Core.Domain.Exceptions
{
	public class DuplicateElementException : Exception, ISerializable
	{
		public DuplicateElementException()
		{
		}

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