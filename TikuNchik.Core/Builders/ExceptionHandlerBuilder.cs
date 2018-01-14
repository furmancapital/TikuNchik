using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Exceptions;

namespace TikuNchik.Core.Builders
{
    public class ExceptionHandlerBuilder<TBody>
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

        private IntegrationFlowBuilder<TBody> Builder
        {
            get; set;
        }
        public IServiceProvider ServiceProvider { get; }

        public ExceptionHandlerBuilder(IBuildableFlow sourceFlow, IntegrationFlowBuilder<TBody> builder, IServiceProvider serviceProvider)
        {
            this.SourceFlow = sourceFlow ?? throw new ArgumentNullException(nameof(sourceFlow));
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.ExceptionHander = new ExceptionHandler(this.ServiceProvider.GetRequiredService<ILogger<ExceptionHandler>>());
        }

        public ExceptionHandlerBuilder<TBody> AddExceptionHandler<TException>(Func<Exception, Integration, bool> handlerToAdd)
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
        public ExceptionHandlerBuilder<TBody> AddExceptionHandlerIgnoreException<TException>()
            where TException : Exception
        {
            this.AddExceptionHandler<TException>((exception, body) => true);
            return this;
        }

        /// <summary>
        /// Adds a handler that will cause the exception to be treated as unhandled on reception
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public ExceptionHandlerBuilder<TBody> AddExceptionHandlerUnhandledException<TException>()
            where TException : Exception
        {
            this.AddExceptionHandler<TException>((exception, body) => false);
            return this;
        }

        public IntegrationFlowBuilder<TBody> EndExceptionHandler()
        {
            this.SourceFlow.AddExceptionHandler(this.ExceptionHander);
            return this.Builder;
        }
    }
}
