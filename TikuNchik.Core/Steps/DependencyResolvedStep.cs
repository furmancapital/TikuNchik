using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step allows for the resolution - and execution - of an object whose lifecycle
    /// will be controlled by the configured dependency injection provider
    /// </summary>
    public class DependencyResolvedStep<TItem> : IStep
    {
        public DependencyResolvedStep (IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }


        public IServiceProvider ServiceProvider { get; }

        public Task PerformStepExecutionAync(Integration integration)
        {
            throw new NotImplementedException();
        }
    }
}
