using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Builders;
using TikuNchik.Core.Exceptions;
using Xunit;

namespace TikuNchik.Core.Builders
{
    public class ExceptionHandlerBuilderTests
    {
        private Mock<IBuildableFlow> BuildableFlow
        {
            get;set;
        }

        private Mock<IServiceProvider> Provider
        {
            get;set;
        }

        private Mock<ILogger<ExceptionHandler>> Logger
        {
            get;set;
        }

        private IntegrationFlowBuilder<string> GetFlowBuilder()
        {
            return new IntegrationFlowBuilder<string>(this.Provider.Object);
        }

        ExceptionHandlerBuilder<string> GetExceptionHandlerBuilder()
        {
            return new ExceptionHandlerBuilder<string>(this.BuildableFlow.Object, this.GetFlowBuilder(), this.Provider.Object);
        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        public ExceptionHandlerBuilderTests()
        {
            this.BuildableFlow = new Mock<IBuildableFlow>();
            this.Provider = new Mock<IServiceProvider>();
            this.Logger = new Mock<ILogger<ExceptionHandler>>();

            this.Provider.Setup(x => x.GetService(typeof(ILogger<ExceptionHandler>)))
                .Returns(this.Logger.Object);
        }

        [Fact]
        public void AddExceptionHandlerIgnoreException_Verify_Exception_Should_Be_Ignored()
        {
            var integration = this.CreateIntegration();
            var builder = this.GetExceptionHandlerBuilder();
            builder.AddExceptionHandlerIgnoreException<InvalidOperationException>();

            builder.ExceptionHander.HandleAsync(new InvalidOperationException(), integration).Wait();
        }


        [Fact]
        public void AddExceptionHandlerUnhandledException_Verify_Exception_Not_Ignored()
        {
            var integration = this.CreateIntegration();
            var builder = this.GetExceptionHandlerBuilder();
            builder.AddExceptionHandlerUnhandledException<InvalidOperationException>();

            Assert.Throws<AggregateException>(() =>
            {
                builder.ExceptionHander.HandleAsync(new InvalidOperationException(), integration).Wait();
            });
        }
    }
}
