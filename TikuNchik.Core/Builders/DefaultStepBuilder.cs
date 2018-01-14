using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class DefaultStepBuilder<TBody> : BaseChoiceStepBuilder<TBody>
    {

        public DefaultStepBuilder(IBuildableFlow sourceFlow, ChoiceBuilder<TBody> builder, Func<Integration, bool> matcher)
            : base(sourceFlow, builder, matcher)
        {
        }

        public DefaultStepBuilder<TBody> AddDefaultStep(IStep step)
        {
            base.AddStep(step);
            return this;
        }

        public DefaultStepBuilder<TBody> AddDefaultStep(Action<Integration> action)
        {
            base.AddStep(action);
            return this;
        }


        public ChoiceBuilder<TBody> EndDefault()
        {
            return EndChoiceAction();
        }

        protected override ChoiceBuilder<TBody> EndChoiceAction()
        {
            this.Builder.AddDefaultStep(StepBuilderHelpers.WrapMultipleSequentialSteps(this.StepsToExecute));
            return this.Builder;
        }
    }
}
