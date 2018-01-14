using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public abstract class StepWithResult<TBody, TOutput>
        : Step<TBody>, IStep<TBody, TOutput>
    {
        protected StepWithResult()
        {
        }

        public virtual async Task<Integration<TOutput>> PerformStepExecutionAsyncWithResult(Integration integration, TBody message)
        {
            await this.PerformStepExecutionAsync(integration, message);
            return (Integration<TOutput>)integration.Body;
        }
    }
}
