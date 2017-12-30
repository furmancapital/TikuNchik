using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to provide the ability to gracefully handle exceptions while also
    /// allowing for the integration to abort execution based on custom business logic
    /// </summary>
    public class ExceptionHandlerStep : IStep
    {
        public static readonly Type RootExceptionType = typeof(Exception);

        private Func<Exception, bool> RootExceptionHandler
        {
            get;set;
        }

        private  IDictionary<Type, Func<Exception, bool>> Handlers
        {
            get; set;
        } = new Dictionary<Type, Func<Exception, bool>>();

        public ExceptionHandlerStep (IStep step, ILogger logger)
        {
            Step = step ?? throw new ArgumentNullException(nameof(step));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a handler for a specific exception type. The handler should return false if for some
        /// reason it does NOT want to handle the particular exception. If the handler does want to handle
        /// the exception and stop the propagation, it should return true.
        /// Note: The handler are applied on a "last in" model. If you add a handler for InvalidOperationException
        /// and then add another one, the last one in will be the only handler available.
        /// </summary>
        /// <typeparam name="TExceptionType"></typeparam>
        /// <param name="handler"></param>
        public void AddExceptionHandler<TExceptionType>(Func<Exception, bool> handler)
            where TExceptionType : Exception
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var exceptionType = typeof(TExceptionType);
            if (RootExceptionType == exceptionType)
            {
                this.RootExceptionHandler = handler;
            }
            else
            {
                this.Handlers[exceptionType] = handler;
            }
        }

        public IStep Step { get; }
        public ILogger Logger { get; }

        public async Task PerformStepExecutionAync(Integration integration)
        {
            try
            {
                await this.Step.PerformStepExecutionAync(integration);
            }
            catch (Exception ex)
            {
                Logger.LogDebug("Attempting to apply custom exception eandling");
                AttemptToHandleException(ex);
            }
        }

        private void AttemptToHandleException (Exception ex)
        {
            var exceptionType = ex.GetType();
            Func<Exception, bool> handler;
            if (!this.Handlers.TryGetValue(exceptionType, out handler))
            {
                if (this.RootExceptionHandler != null)
                {
                    Logger.LogDebug("Falling back to Root exception handler");
                    handler = this.RootExceptionHandler;
                }
                else
                {
                    Logger.LogDebug($"No exception handler found for {exceptionType}");
                    throw new IntegrationException(ex.Message, ex);
                }
            }

            if (!handler(ex))
            {
                Logger.LogDebug($"Custom exception handler for {exceptionType} indicated that it cannot handle the exception");
                throw new IntegrationException(ex.Message, ex);
            }
        }
    }
}
