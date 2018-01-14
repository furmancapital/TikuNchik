using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class ExecuteAnotherFlowStepTests
    {
        private Mock<IFlow> TargetFlow
        {
            get; set;
        }


        private ExecuteAnotherFlowStep GetAnotherFlowStep()
        {
            return new ExecuteAnotherFlowStep(this.TargetFlow.Object);
        }


        public ExecuteAnotherFlowStepTests()
        {
            this.TargetFlow = new Moq.Mock<IFlow>();

        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        [Fact]
        public void PerformStepExecutionAync_Ensure_Flow_Executed()
        {
            this.TargetFlow.Setup(x => x.ExecuteCurrentIntegration(It.IsAny<Integration>()))
                .Returns(Task.FromResult(0));

            Func<Integration, bool> lambda = (x) => true;

            var step = this.GetAnotherFlowStep();

            step.PerformStepExecutionAsync(this.CreateIntegration()).Wait();

            this.TargetFlow.VerifyAll();
        }
    }
}
