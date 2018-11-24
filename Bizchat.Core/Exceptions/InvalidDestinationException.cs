using System;
using System.Runtime.Serialization;

namespace Bizchat.Core.Exceptions
{
    [Serializable]
    public class InvalidDestinationException : Exception
    {
        public InvalidDestinationException()
        {
        }

        public InvalidDestinationException(string message) : base(message)
        {
        }

        public InvalidDestinationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDestinationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}