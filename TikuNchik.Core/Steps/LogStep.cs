using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    public class LogStep : IStep
    {

        public LogStep (ILogger<LogStep> logger, Action<Integration, ILogger<LogStep>> actionToPerform)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ActionToPerform = actionToPerform ?? throw new ArgumentNullException(nameof(actionToPerform));
        }

        public ILogger<LogStep> Logger { get; }
        public Action<Integration, ILogger<LogStep>> ActionToPerform { get; }

        public Task PerformStepExecutionAync(Integration integration)
        {
            this.ActionToPerform(integration, this.Logger);
            return Task.FromResult(0);
        }
    }
}
