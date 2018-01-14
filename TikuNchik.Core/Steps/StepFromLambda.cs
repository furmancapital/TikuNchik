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

        public async Task<Integration> PerformStepExecutionAsync(Integration integration)
        {
            var asyncFuncExecution = new Func<Task<Integration>>(() =>
            {
                Action(integration);
                return Task.FromResult(integration);
            });

            var result = await asyncFuncExecution();
            return result;
        }
    }
}
