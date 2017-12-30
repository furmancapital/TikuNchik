using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TikuNchik.Core
{
    /// <summary>
    /// This class indicates that an exception that was not handled has been thrown by
    /// either the logic or the engine.
    /// </summary>
    public class IntegrationException : Exception
    {
        public IntegrationException()
        {
        }

        public IntegrationException(string message) : base(message)
        {
        }

        public IntegrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IntegrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
