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
        public DependencyResolvedStep (IServiceProvider serviceProvider, Action<TItem, Integration> action)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }


        public IServiceProvider ServiceProvider { get; }
        public Action<TItem, Integration> Action { get; }

        public Task PerformStepExecutionAsync(Integration integration)
        {
            var targetDependency = this.ServiceProvider.GetRequiredService<TItem>();
            Action(targetDependency, integration);
            return Task.FromResult(0);
        }
    }
}
