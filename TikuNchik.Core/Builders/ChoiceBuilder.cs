using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Builders
{
    public class ChoiceBuilder
    {
        private ChoiceBuilder(Flow sourceFlow, IntegrationFlowBuilder builder)
        {
            this.SourceFlow = sourceFlow;
            this.Builder = builder;
        }


        private Flow SourceFlow
        {
            get; set;
        }

        private IntegrationFlowBuilder Builder
        {
            get; set;
        }

        public static ChoiceBuilder Choice(Flow sourceFlow, IntegrationFlowBuilder builder)
        {
            return new ChoiceBuilder(sourceFlow, builder);
        }

        public ChoiceStepBuilder When (Func<Integration, bool> matcher)
        {
            return new ChoiceStepBuilder(this.SourceFlow, this);
        }

        public IntegrationFlowBuilder EndChoice()
        {
            return this.Builder;
        }


    }
}
