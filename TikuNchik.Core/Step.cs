using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public abstract class Step : IStep
    {
        public abstract Task<StepExecution> PerformStepExecutionAync(Integration integration);
    }
}
