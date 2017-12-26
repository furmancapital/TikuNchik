using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Builders
{
    public class ChoiceStepBuilder
    {
        private List<IStep> StepsToExecute
        {
            get; set;
        } = new List<IStep>();

        private Flow SourceFlow
        {
            get; set;
        }

        private ChoiceBuilder Builder
        {
            get; set;
        }

        public ChoiceStepBuilder(Flow sourceFlow, ChoiceBuilder builder)
        {
            this.SourceFlow = sourceFlow;
            this.Builder = builder;
        }

        public ChoiceStepBuilder AddStep(IStep step)
        {
            this.StepsToExecute.Add(step);
            return this;
        }

        public ChoiceBuilder EndWhen()
        {
            return this.Builder;
        }
    }
}
