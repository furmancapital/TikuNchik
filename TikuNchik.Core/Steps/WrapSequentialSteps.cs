﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This class is used to wrap multiple steps that need to be executed as one IStep instance
    /// </summary>
    public class WrapSequentialSteps : IStep
    {
        public WrapSequentialSteps (IEnumerable<IStep> steps)
        {
            Steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public IEnumerable<IStep> Steps { get; }

        public async Task<Integration> PerformStepExecutionAsync(Integration integration)
        {
            var currentIntegration = integration;

            foreach (var step in this.Steps)
            {
                currentIntegration = await step.PerformStepExecutionAsync(currentIntegration);
            }

            return currentIntegration;
        }
    }
}
