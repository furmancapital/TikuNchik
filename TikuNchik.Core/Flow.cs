﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    /// <summary>
    /// The flow serves as a "parent" to all of the steps that have been added to it.
    /// The flow is used to pass messages into the execution engine.
    /// </summary>
    public class Flow : IBuildableFlow, IFlow
    {
        public Flow()
        {

        }

        private List<IStep> Steps
        {
            get;
            set;
        } = new List<IStep>();

        public async Task<Integration> CreateIntegration<T>(T sourceMessage)
        {
            var integration = new Integration();
            foreach (var step in this.Steps)
            {
                var stepResult = await step.PerformStepExecutionAync(integration);
            }

            return integration;
        }

        public async Task ExecuteCurrentIntegration(Integration integration)
        {
            foreach (var step in this.Steps)
            {
                var stepResult = await step.PerformStepExecutionAync(integration);
            }

        }

        void IBuildableFlow.AddStep(IStep step)
        {
            this.Steps.Add(step);
        }
    }
}
