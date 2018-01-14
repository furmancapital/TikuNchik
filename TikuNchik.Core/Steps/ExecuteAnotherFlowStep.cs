using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    public class ExecuteAnotherFlowStep<TBody> : IStep
    {
        public ExecuteAnotherFlowStep (IFlow targetFlow)
        {
            TargetFlow = targetFlow ?? throw new ArgumentNullException(nameof(targetFlow));
        }

        public IFlow TargetFlow { get; }

        public Task<Integration> PerformStepExecutionAsync(Integration integration)
        {
            //TODO: confirm behavior
            return this.TargetFlow.ExecuteCurrentIntegration((Integration<TBody>)integration);
        }

    }
}
