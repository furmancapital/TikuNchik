using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public interface IStep
    {
        Task<Integration> PerformStepExecutionAsync(Integration integration);
    }

    public interface IStep<TBody> : IStep
    {
        Task<Integration> PerformStepExecutionAsync(Integration integration, TBody message);
    }

    public interface IStep<TBody, TOutput> : IStep<TBody>
    {
        Task<Integration<TOutput>> PerformStepExecutionAsyncWithResult(Integration integration, TBody message);
    }



}
