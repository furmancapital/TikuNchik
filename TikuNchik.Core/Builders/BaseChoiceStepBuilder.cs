using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public abstract class BaseChoiceStepBuilder
    {
        protected List<IStep> StepsToExecute
        {
            get; set;
        } = new List<IStep>();

        protected IBuildableFlow SourceFlow
        {
            get; set;
        }

        protected ChoiceBuilder Builder
        {
            get; set;
        }

        protected Func<Integration, bool> Matcher { get; }

        protected BaseChoiceStepBuilder(IBuildableFlow sourceFlow, ChoiceBuilder builder, Func<Integration, bool> matcher)
        {
            this.SourceFlow = sourceFlow ?? throw new ArgumentNullException(nameof(sourceFlow));
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        protected void AddStep(IStep step)
        {
            this.StepsToExecute.Add(step);
        }

        protected void AddStep(Action<Integration> action)
        {
            this.StepsToExecute.Add(StepBuilderHelpers.FromLambda(action));
        }

        protected ChoiceBuilder EndChoiceAction()
        {
            this.Builder.AddStep(this.Matcher, StepBuilderHelpers.WrapMultipleSequentialSteps(this.StepsToExecute));
            return this.Builder;
        }
    }
}
