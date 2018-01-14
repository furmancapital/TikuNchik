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

        public async Task PerformStepExecutionAsync(Integration integration)
        {
            var asyncFuncExecution = new Func<Task>(() =>
            {
                Action(integration);
                return Task.FromResult(0);
            });

            await asyncFuncExecution();
        }
    }
}
