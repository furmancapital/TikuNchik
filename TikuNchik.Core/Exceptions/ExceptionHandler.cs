using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Exceptions
{
    /// <summary>
    /// This step is used to provide the ability to gracefully handle exceptions while also
    /// allowing for the integration to abort execution based on custom business logic
    /// </summary>
    public class ExceptionHandler : IExceptionHandler
    {
        public static readonly Type RootExceptionType = typeof(Exception);

        private Func<Exception, Integration, bool> RootExceptionHandler
        {
            get;set;
        }

        private  IDictionary<Type, Func<Exception, Integration, bool>> Handlers
        {
            get; set;
        } = new Dictionary<Type, Func<Exception, Integration, bool>>();

        public ExceptionHandler (ILogger logger)
        {
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
        public void AddExceptionHandler<TExceptionType>(Func<Exception, Integration, bool> handler)
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

        public ILogger Logger { get; }

        private async Task AttemptToHandleException (Exception ex, Integration sourceIntegration)
        {
            var exceptionType = ex.GetType();
            Func<Exception, Integration, bool> handler;
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
                    throw new IntegrationException($"No Configured Exception Handler found for {exceptionType}", ex);
                }
            }

            if (!await Task.FromResult(handler(ex, sourceIntegration)))
            {
                var message = $"Custom exception handler for {exceptionType} indicated that it should not handle the exception";
                Logger.LogDebug(message);
                throw new IntegrationException(message, ex);
            }
        }

        public async Task HandleAsync<TException>(TException ex, Integration sourceIntegration) where TException : Exception
        {
            await AttemptToHandleException(ex, sourceIntegration);
        }
    }
}
