using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TikuNchik.Core.Helpers;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to specify a filter and associated "sub" steps. If the filter passes, then
    /// the sub-steps get executed. Otherwise, the execution is "short-circuited".
    /// </summary>
    public class FilterStep : IStep
    {
        public FilterStep(Func<Integration, bool> filterToApply, IEnumerable<IStep> stepsToExecute)
        {
            StepsToExecute = stepsToExecute ?? throw new ArgumentNullException(nameof(stepsToExecute));
            FilterToApply = filterToApply ?? throw new ArgumentNullException(nameof(filterToApply));
        }

        public FilterStep(Func<Integration, bool> filterToApply, Action<Integration> actionToExecute)
        {
            if (actionToExecute == null)
            {
                throw new ArgumentNullException(nameof(actionToExecute));
            }

            FilterToApply = filterToApply ?? throw new ArgumentNullException(nameof(filterToApply));
            this.StepsToExecute = new[]
            {
                StepBuilderHelpers.FromLambda(actionToExecute)
            };
        }

        public IEnumerable<IStep> StepsToExecute { get; }
        public Func<Integration, bool> FilterToApply { get; }

        public async Task PerformStepExecutionAync(Integration integration)
        {
            if (this.FilterToApply(integration))
            {
                foreach (var stepToExecute in this.StepsToExecute)
                {
                    await stepToExecute.PerformStepExecutionAync(integration);
                }
            }
            else
            {
                integration.SetFilteredOut("Filtered out by filter");
            }

        }
    }
}
