using System;
using System.Runtime.Serialization;

namespace WorkMyFlight
{
    [Serializable]
    public class NoMoreTicketsException : Exception
    {
        public NoMoreTicketsException()
        {
        }

        public NoMoreTicketsException(string message) : base(message)
        {
        }

        public NoMoreTicketsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoMoreTicketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}