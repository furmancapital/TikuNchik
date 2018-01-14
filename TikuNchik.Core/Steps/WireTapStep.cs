using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to execute asynchronous "wire-tapping" of received messages. This is extremelly useful
    /// for finding bugs in production.
    /// </summary>
    public class WireTapStep : IStep
    {
        public WireTapStep (ILogger<WireTapStep> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ILogger<WireTapStep> Logger { get; }

        public Task PerformStepExecutionAsync(Integration integration)
        {
            return Task.Run(() =>
            {
                Logger.LogInformation($"WireTap: [{integration}]");
            });
        }
    }
}
