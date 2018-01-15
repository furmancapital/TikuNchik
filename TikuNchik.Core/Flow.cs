using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TikuNchik.Core.Exceptions;

namespace TikuNchik.Core
{
    /// <summary>
    /// The flow serves as a "parent" to all of the steps that have been added to it.
    /// The flow is used to pass messages into the execution engine.
    /// </summary>
    public class Flow : IBuildableFlow, IFlow
    {
        public Flow(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IServiceProvider ServiceProvider { get; }
        private List<IStep> Steps
        {
            get;
            set;
        } = new List<IStep>();

        private IExceptionHandler ExceptionHandler
        {
            get;set;
        }

        public async Task<Integration> CreateIntegration<T>(T sourceMessage)
        {
            var integration = new Integration<T>(sourceMessage);

            Integration currentIntegration = integration;
            foreach (var step in this.Steps)
            {
                var integrationResult = await step.PerformStepExecutionAsync(currentIntegration);
                currentIntegration = integrationResult;
            }

            return currentIntegration;
        }

        public async Task<Integration> ExecuteCurrentIntegration<T>(Integration<T> integration)
        {
            Integration currentIntegration = integration;
            foreach (var step in this.Steps)
            {
                currentIntegration = await step.PerformStepExecutionAsync(currentIntegration);
            }
            return currentIntegration;

        }

        void IBuildableFlow.AddExceptionHandler(IExceptionHandler exceptionHandler)
        {
            this.ExceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        }

        void IBuildableFlow.AddStep(IStep step)
        {
            this.Steps.Add(step);
        }

    }
}
