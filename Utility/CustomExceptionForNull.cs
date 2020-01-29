using System;
using System.Runtime.Serialization;

namespace BankTransfer.Utility
{
    [Serializable]
    internal class CustomExceptionForNull : Exception
    {
        public CustomExceptionForNull()
        {
        }

        public CustomExceptionForNull(string message) : base(message)
        {
        }

        public CustomExceptionForNull(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomExceptionForNull(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}