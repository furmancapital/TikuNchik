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
        public static IntegrationFlowBuilder<TBody> Log<TBody>(IBuildableFlow sourceFlow, IntegrationFlowBuilder<TBody> builder, Action<Integration<TBody>, ILogger> action, IServiceProvider serviceProvider)
        {
            var logStep = new LogStep<TBody>(serviceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger(typeof(LogStep<TBody>)), action);

            sourceFlow.AddStep(logStep);
            return builder;
        }
    }
}
