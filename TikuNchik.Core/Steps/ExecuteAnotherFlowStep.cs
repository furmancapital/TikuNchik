using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    public class ExecuteAnotherFlowStep : IStep
    {
        public ExecuteAnotherFlowStep (IFlow targetFlow)
        {
            TargetFlow = targetFlow ?? throw new ArgumentNullException(nameof(targetFlow));
        }

        public IFlow TargetFlow { get; }

        public async Task<StepExecution> PerformStepExecutionAync(Integration integration)
        {
            await this.TargetFlow.ExecuteCurrentIntegration(integration);
            return new StepExecution();

        }
    }
}
