using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Exceptions;

namespace TikuNchik.Core
{
    /// <summary>
    /// This interface is used to represent a flow that is buildable from various steps.
    /// </summary>
    public interface IBuildableFlow
    {
        /// <summary>
        /// Adds a step to the flow
        /// </summary>
        /// <param name="step"></param>
        void AddStep(IStep step);
        /// <summary>
        /// Adds an exception handler to the flow
        /// </summary>
        /// <param name="exceptionHandler"></param>
        void AddExceptionHandler(IExceptionHandler exceptionHandler);
    }
}
