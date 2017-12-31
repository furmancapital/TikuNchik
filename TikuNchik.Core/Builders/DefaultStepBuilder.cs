using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class DefaultStepBuilder : BaseChoiceStepBuilder
    {

        public DefaultStepBuilder(IBuildableFlow sourceFlow, ChoiceBuilder builder, Func<Integration, bool> matcher)
            : base(sourceFlow, builder, matcher)
        {
        }

        public DefaultStepBuilder AddDefaultStep(IStep step)
        {
            base.AddStep(step);
            return this;
        }

        public DefaultStepBuilder AddDefaultStep(Action<Integration> action)
        {
            base.AddStep(action);
            return this;
        }


        public ChoiceBuilder EndDefault()
        {
            base.EndChoiceAction();
            return this.Builder;
        }
    }
}
