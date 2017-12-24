using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public interface IStep
    {
        Task<StepExecution> PerformStepExecutionAync(Integration integration);
    }
}
