using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to wrap up lambdas into IStep instances
    /// </summary>
    public class StepFromLambda : IStep
    {
        public StepFromLambda (Action<Integration> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public Action<Integration> Action { get; }

        public async Task<StepExecution> PerformStepExecutionAync(Integration integration)
        {
            var asyncFuncExecution = new Func<Task<StepExecution>>(() =>
            {
                Action(integration);
                return Task.FromResult<StepExecution>(new StepExecution());
            });

            var result = await asyncFuncExecution();
            return result;
        }
    }
}
