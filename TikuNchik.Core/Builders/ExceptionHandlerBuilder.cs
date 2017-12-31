using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Exceptions;

namespace TikuNchik.Core.Builders
{
    public class ExceptionHandlerBuilder
    {
        public ExceptionHandler ExceptionHander
        {
            get;
            private set;
        }

        private IBuildableFlow SourceFlow
        {
            get; set;
        }

        private IntegrationFlowBuilder Builder
        {
            get; set;
        }
        public IServiceProvider ServiceProvider { get; }

        public ExceptionHandlerBuilder(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder, IServiceProvider serviceProvider)
        {
            this.SourceFlow = sourceFlow ?? throw new ArgumentNullException(nameof(sourceFlow));
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.ExceptionHander = new ExceptionHandler(this.ServiceProvider.GetRequiredService<ILogger<ExceptionHandler>>());
        }

        public ExceptionHandlerBuilder AddExceptionHandler<TException>(Func<Exception, bool> handlerToAdd)
            where TException : Exception
        {
            this.ExceptionHander.AddExceptionHandler<TException>(handlerToAdd);
            return this;
        }

        /// <summary>
        /// Adds a handler that will "ignore" an exception on reception
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public ExceptionHandlerBuilder AddExceptionHandlerIgnoreException<TException>()
            where TException : Exception
        {
            this.AddExceptionHandler<TException>((x) => true);
            return this;
        }

        /// <summary>
        /// Adds a handler that will cause the exception to be treated as unhandled on reception
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public ExceptionHandlerBuilder AddExceptionHandlerUnhandledException<TException>()
            where TException : Exception
        {
            this.AddExceptionHandler<TException>((x) => false);
            return this;
        }

        public IntegrationFlowBuilder EndExceptionHandler()
        {
            this.SourceFlow.AddExceptionHandler(this.ExceptionHander);
            return this.Builder;
        }
    }
}
