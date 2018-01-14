using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class ChoiceBuilder
    {
        private ChoiceBuilder(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder)
        {
            this.SourceFlow = sourceFlow;
            this.Builder = builder;
        }


        private IBuildableFlow SourceFlow
        {
            get; set;
        }

        private IntegrationFlowBuilder Builder
        {
            get; set;
        }

        private List<KeyValuePair<Func<Integration, bool>, IStep>> GeneratedSteps
        {
            get; set;
        } = new List<KeyValuePair<Func<Integration, bool>, IStep>>();

        private IStep DefaultHandler
        {
            get;set;
        }

        internal void AddStep (Func<Integration, bool> matcher, IStep step)
        {
            this.GeneratedSteps.Add(new KeyValuePair<Func<Integration, bool>, IStep>(matcher, step));
        }

        internal void AddDefaultStep (IStep step)
        {
            this.DefaultHandler = step;
        }

        public static ChoiceBuilder Choice(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder)
        {
            return new ChoiceBuilder(sourceFlow, builder);
        }

        public ChoiceStepBuilder When (Func<Integration, bool> matcher)
        {
            return new ChoiceStepBuilder(this.SourceFlow, this, matcher);
        }

        /// <summary>
        /// Adds a "fallback" clause that will get triggered if none of the other
        /// ones are triggered
        /// </summary>
        /// <returns></returns>
        public DefaultStepBuilder Default ()
        {
            return new DefaultStepBuilder(this.SourceFlow, this, (x) => true);
        }

        public IntegrationFlowBuilder EndChoice()
        {
            this.SourceFlow.AddStep(StepBuilderHelpers.FromLambda(async (x) => 
            {
                var defaultStep = this.DefaultHandler;
                foreach (var step in this.GeneratedSteps)
                {
                    if (step.Key(x))
                    {
                        await step.Value.PerformStepExecutionAsync(x);
                        break;
                    }
                }

                if (defaultStep != null)
                {
                    await defaultStep.PerformStepExecutionAsync(x);
                }
            }));
            return this.Builder;
        }


    }
}
