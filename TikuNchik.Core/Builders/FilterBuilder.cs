using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core
{
    public class FilterBuilder<TBody>
    {
        private List<IStep> StepsToExecute
        {
            get;
            set;
        } = new List<IStep>();

        private Func<Integration, bool> FilterFunction
        {
            get;set;
        }

        private IBuildableFlow SourceFlow
        {
            get;set;
        }

        private IntegrationFlowBuilder<TBody> Builder
        {
            get; set;
        }

        private FilterBuilder()
        {

        }

        public static FilterBuilder<TBody> Filter(Func<Integration, bool> filterFunction, IBuildableFlow sourceFlow, IntegrationFlowBuilder<TBody> builder)
        {
            if (filterFunction == null)
            {
                throw new ArgumentNullException(nameof(filterFunction));
            }

            return new FilterBuilder<TBody>()
            {
                FilterFunction = filterFunction,
                Builder = builder,
                SourceFlow = sourceFlow
            };
        }

        public FilterBuilder<TBody> AddStepToExecute (IStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            this.StepsToExecute.Add(step);
            return this;
        }

        public FilterBuilder<TBody> AddStepToExecute (Action<Integration> actionToExecute)
        {
            if (actionToExecute == null)
            {
                throw new ArgumentNullException(nameof(actionToExecute));
            }

            this.StepsToExecute.Add(StepBuilderHelpers.FromLambda(actionToExecute));
            return this;
        }

        public IntegrationFlowBuilder<TBody> EndFilter()
        {
            this.SourceFlow.AddStep(new FilterStep(this.FilterFunction, this.StepsToExecute));
            return this.Builder;
        }
    }
}
