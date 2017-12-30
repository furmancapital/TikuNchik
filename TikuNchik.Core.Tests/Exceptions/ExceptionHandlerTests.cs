using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TikuNchik.Core.Exceptions
{
    public class ExceptionHandlerTests
    {

        private Mock<ILogger> Logger
        {
            get; set;
        }

        public ExceptionHandlerTests()
        {
            this.Logger = new Mock<ILogger>();
        }

        private ExceptionHandler GetHandler()
        {
            return new ExceptionHandler(this.Logger.Object);
        }

        [Fact]
        public void HandleException_If_Exception_Unhandled_DueToNoHandlers_Throw_Exception()
        {
            var handler = this.GetHandler();

            var exception = Assert.Throws<AggregateException>(() =>
            {
                handler.HandleAsync(new InvalidOperationException()).Wait();
            });

        }

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Unhandled_DueToNoMatch_Throw_Exception()
        {
            var handler = this.GetHandler();
            //different exception being handled than the one the handler will throw
            handler.AddExceptionHandler<FieldAccessException>((x) => true);

            var exception = Assert.Throws<AggregateException>(() =>
            {
                handler.HandleAsync(new InvalidOperationException()).Wait();
            });

            Assert.Equal("No Configured Exception Handler found for System.InvalidOperationException", exception.InnerException.Message);

        }


        [Fact]
        public void PerformStepExecutionAync_If_Exception_Unhandled_DueToHandlerReturningFalse_Throw_Exception()
        {
            var handler = this.GetHandler();
            handler.AddExceptionHandler<InvalidOperationException>((x) => false);

            var exception = Assert.Throws<AggregateException>(() =>
            {
                handler.HandleAsync(new InvalidOperationException()).Wait();
            });

            Assert.Equal("Custom exception handler for System.InvalidOperationException indicated that it should not handle the exception", exception.InnerException.Message);


        }

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Handled_DoNotThrow_Exception()
        {
            var handler = this.GetHandler();
            handler.AddExceptionHandler<InvalidOperationException>((x) => true);

            handler.HandleAsync(new InvalidOperationException()).Wait();

        }

        /// <summary>
        /// This test verifies that if we register a handler for Exception and then an exception is fired but there are NO
        /// "better matching" handlers, then it should handle them
        /// </summary>

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Of_TypeException_Registered_Should_Correctly_Catch_SUbClasses_If_TheyDoNotHaveHandlers()
        {
            var handler = this.GetHandler();
            handler.AddExceptionHandler<Exception>((x) => true);

            handler.HandleAsync(new InvalidOperationException()).Wait();

        }

        /// <summary>
        /// This test verifies that if we register a handler for Exception and then an exception is fired BUT there ARE
        /// "better matching" handlers, then THEY should handle them
        /// </summary>

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Combined_With_Specific_Handlers_Verify_SpecificHandler_Properly_Used_To_HandleException()
        {
            var handler = this.GetHandler();
            handler.AddExceptionHandler<Exception>((x) => false);
            handler.AddExceptionHandler<InvalidOperationException>((x) => true);

            handler.HandleAsync(new InvalidOperationException()).Wait();

        }
    }
}
