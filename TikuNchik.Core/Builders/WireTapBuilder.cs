using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class WireTapBuilder
    {
        public static IntegrationFlowBuilder WireTap(Flow sourceFlow, IntegrationFlowBuilder builder)
        {
            var wireTapStep = new WireTapStep(null);
            sourceFlow.AddStep(wireTapStep);
            return builder;
        }
    }
}
