using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TikuNchik.Core
{
    /// <summary>
    /// This exception is used to indicate that a configuration exception has been
    /// made and that the integration system cannot function in this state.
    /// </summary>
    public class FlowConfigurationException : Exception
    {
        public FlowConfigurationException()
        {
        }

        public FlowConfigurationException(string message) : base(message)
        {
        }

        public FlowConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlowConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
