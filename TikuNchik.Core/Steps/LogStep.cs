using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    public class LogStep<TBody> : IStep
    {

        public LogStep (ILogger logger, Action<Integration<TBody>, ILogger> actionToPerform)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ActionToPerform = actionToPerform ?? throw new ArgumentNullException(nameof(actionToPerform));
        }

        public ILogger Logger { get; }
        public Action<Integration<TBody>, ILogger> ActionToPerform { get; }

        public Task<Integration> PerformStepExecutionAsync(Integration integration)
        {
            this.ActionToPerform((Integration<TBody>)integration, this.Logger);
            return Task.FromResult(integration);
        }
    }
}
