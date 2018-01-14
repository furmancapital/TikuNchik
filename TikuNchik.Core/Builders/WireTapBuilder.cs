using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class WireTapBuilder
    {
        public static IntegrationFlowBuilder<TBody> WireTap<TBody>(IBuildableFlow sourceFlow, IntegrationFlowBuilder<TBody> builder, IServiceProvider serviceProvider)
        {
            var wireTapStep = new WireTapStep(serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<WireTapStep>());
            sourceFlow.AddStep(wireTapStep);
            return builder;
        }
    }
}
