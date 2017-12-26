using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Builders;

namespace TikuNchik.Core
{
    public class IntegrationFlowBuilder
    {
        private Flow CreatedFlow
        {
            get;set;
        }

        public static IntegrationFlowBuilder Create()
        {
            return new IntegrationFlowBuilder();
        }

        private IntegrationFlowBuilder()
        {
            this.CreatedFlow = new Flow();
        }

        public ChoiceBuilder Choice()
        {
            return ChoiceBuilder.Choice();
        }

        public FilterBuilder Filter(Func<Integration, bool> filter)
        {
            return FilterBuilder.Filter(filter, this.CreatedFlow, this);
        }

        public IntegrationFlowBuilder WireTap()
        {
            return WireTapBuilder.WireTap(this.CreatedFlow, this);
        }

        public Flow Build()
        {
            return this.CreatedFlow;
        }
    }
}
