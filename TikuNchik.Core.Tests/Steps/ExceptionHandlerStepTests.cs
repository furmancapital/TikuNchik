using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class ExceptionHandlerStepTests
    {
        private Mock<IStep> TargetStep
        {
            get; set;
        }

        private Mock<ILogger> Logger
        {
            get; set;
        }

        public ExceptionHandlerStepTests()
        {
            this.TargetStep = new Mock<IStep>();
            this.Logger = new Mock<ILogger>();
        }

        private ExceptionHandlerStep GetStep()
        {
            return new ExceptionHandlerStep(this.TargetStep.Object, this.Logger.Object);
        }

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Unhandled_DueToNoHandlers_Throw_Exception()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();

            Assert.Throws<AggregateException>(() =>
            {
                step.PerformStepExecutionAync(new Integration()).Wait();
            });

        }

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Unhandled_DueToNoMatch_Throw_Exception()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();
            //different exception being handled than the one the step will throw
            step.AddExceptionHandler<FieldAccessException>((x) => true);

            Assert.Throws<AggregateException>(() =>
            {
                step.PerformStepExecutionAync(new Integration()).Wait();
            });

        }


        [Fact]
        public void PerformStepExecutionAync_If_Exception_Unhandled_DueToHandlerReturningFalse_Throw_Exception()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();
            step.AddExceptionHandler<FieldAccessException>((x) => false);

            Assert.Throws<AggregateException>(() =>
            {
                step.PerformStepExecutionAync(new Integration()).Wait();
            });

        }

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Handled_DoNotThrow_Exception()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();
            step.AddExceptionHandler<InvalidOperationException>((x) => true);

            step.PerformStepExecutionAync(new Integration()).Wait();

        }

        /// <summary>
        /// This test verifies that if we register a handler for Exception and then an exception is fired but there are NO
        /// "better matching" handlers, then it should handle them
        /// </summary>

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Of_TypeException_Registered_Should_Correctly_Catch_SUbClasses_If_TheyDoNotHaveHandlers()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();
            step.AddExceptionHandler<Exception>((x) => true);

            step.PerformStepExecutionAync(new Integration()).Wait();

        }

        /// <summary>
        /// This test verifies that if we register a handler for Exception and then an exception is fired BUT there ARE
        /// "better matching" handlers, then THEY should handle them
        /// </summary>

        [Fact]
        public void PerformStepExecutionAync_If_Exception_Combined_With_Specific_Handlers_Verify_SpecificHandler_Properly_Used_To_HandleException()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Throws<InvalidOperationException>();

            var step = this.GetStep();
            step.AddExceptionHandler<Exception>((x) => false);
            step.AddExceptionHandler<InvalidOperationException>((x) => true);

            step.PerformStepExecutionAync(new Integration()).Wait();

        }
    }
}
