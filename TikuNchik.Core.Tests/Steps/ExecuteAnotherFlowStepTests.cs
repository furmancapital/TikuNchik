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


        private ExecuteAnotherFlowStep<string> GetAnotherFlowStep()
        {
            return new ExecuteAnotherFlowStep<string>(this.TargetFlow.Object);
        }


        public ExecuteAnotherFlowStepTests()
        {
            this.TargetFlow = new Moq.Mock<IFlow>();

        }

        private Integration<string> CreateIntegration(string value)
        {
            return new Integration<string>(value);
        }

        [Fact]
        public void PerformStepExecutionAync_Ensure_Flow_Executed()
        {
            var integration = this.CreateIntegration("BODY");

            this.TargetFlow.Setup(x => x.ExecuteCurrentIntegration<string>(It.IsAny<Integration<string>>()))
                .Returns(Task.FromResult((Integration)integration));

            Func<Integration, bool> lambda = (x) => true;

            var step = this.GetAnotherFlowStep();

            step.PerformStepExecutionAsync(integration).Wait();

            this.TargetFlow.VerifyAll();
        }
    }
}
