using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class ChoiceStepBuilder<TBody> : BaseChoiceStepBuilder<TBody>
    {

        public ChoiceStepBuilder(IBuildableFlow sourceFlow, ChoiceBuilder<TBody> builder, Func<Integration, bool> matcher)
            : base(sourceFlow, builder, matcher)
        {
        }

        public ChoiceStepBuilder<TBody> AddChoiceStep(IStep step)
        {
            base.AddStep(step);
            return this;
        }

        public ChoiceStepBuilder<TBody> AddChoiceStep(Action<Integration> action)
        {
            base.AddStep(action);
            return this;
        }

        public ChoiceStepBuilder<TBody> Log (Action<Integration<TBody>, ILogger> logAction)
        {
            var logger = this.SourceFlow.ServiceProvider.GetRequiredService<ILogger<LogStep<TBody>>>();
            base.AddStep(new LogStep<TBody>(logger, logAction));
            return this;
        }

        public ChoiceBuilder<TBody> EndWhen()
        {
            EndChoiceAction();
            return this.Builder;
        }

        protected override ChoiceBuilder<TBody> EndChoiceAction()
        {
            this.Builder.AddStep(this.Matcher, StepBuilderHelpers.WrapMultipleSequentialSteps(this.StepsToExecute));
            return this.Builder;

        }
    }
}
