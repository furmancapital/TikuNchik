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
        public static IntegrationFlowBuilder WireTap(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder, IServiceProvider serviceProvider)
        {
            var wireTapStep = new WireTapStep(serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<WireTapStep>());
            sourceFlow.AddStep(wireTapStep);
            return builder;
        }
    }
}
