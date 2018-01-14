using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public interface IStep
    {
        Task PerformStepExecutionAsync(Integration integration);
    }

    public interface IStep<TBody> : IStep
    {
        Task PerformStepExecutionAsync(Integration integration, TBody message);
    }

    public interface IStep<TBody, TOutput> : IStep<TBody>
    {
        Task<TOutput> PerformStepExecutionAsyncWithResult(Integration integration, TBody message);
    }



}
