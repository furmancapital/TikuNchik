using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class ChoiceStepBuilder : BaseChoiceStepBuilder
    {

        public ChoiceStepBuilder(IBuildableFlow sourceFlow, ChoiceBuilder builder, Func<Integration, bool> matcher)
            : base(sourceFlow, builder, matcher)
        {
        }

        public ChoiceStepBuilder AddChoiceStep(IStep step)
        {
            base.AddStep(step);
            return this;
        }

        public ChoiceStepBuilder AddChoiceStep(Action<Integration> action)
        {
            base.AddStep(action);
            return this;
        }


        public ChoiceBuilder EndWhen()
        {
            EndChoiceAction();
            return this.Builder;
        }

        protected override ChoiceBuilder EndChoiceAction()
        {
            this.Builder.AddStep(this.Matcher, StepBuilderHelpers.WrapMultipleSequentialSteps(this.StepsToExecute));
            return this.Builder;

        }
    }
}
