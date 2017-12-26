using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core
{
    public class FilterBuilder
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

        private Flow SourceFlow
        {
            get;set;
        }

        private IntegrationFlowBuilder Builder
        {
            get; set;
        }

        private FilterBuilder()
        {

        }

        public static FilterBuilder Filter(Func<Integration, bool> filterFunction, Flow sourceFlow, IntegrationFlowBuilder builder)
        {
            if (filterFunction == null)
            {
                throw new ArgumentNullException(nameof(filterFunction));
            }

            return new FilterBuilder()
            {
                FilterFunction = filterFunction,
                Builder = builder,
                SourceFlow = sourceFlow
            };
        }

        public FilterBuilder AddStepToExecute (IStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            this.StepsToExecute.Add(step);
            return this;
        }

        public IntegrationFlowBuilder EndFilter()
        {
            this.SourceFlow.AddStep(new FilterStep(this.StepsToExecute, this.FilterFunction));
            return this.Builder;
        }
    }
}
