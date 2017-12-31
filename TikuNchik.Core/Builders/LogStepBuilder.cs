using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class LogStepBuilder
    {
        public static IntegrationFlowBuilder Log(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder, Action<Integration, ILogger> action, IServiceProvider serviceProvider)
        {
            var logStep = new LogStep(serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<LogStep>(), action);
            sourceFlow.AddStep(logStep);
            return builder;
        }
    }
}
