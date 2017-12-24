using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to specify a filter and associated "sub" steps. If the filter passes, then
    /// the sub-steps get executed. Otherwise, the execution is "short-circuited".
    /// </summary>
    public class FilterStep : IStep
    {
        public FilterStep (IEnumerable<Step> stepsToExecute, Func<Integration, bool> filterToApply)
        {
            StepsToExecute = stepsToExecute ?? throw new ArgumentNullException(nameof(stepsToExecute));
            FilterToApply = filterToApply ?? throw new ArgumentNullException(nameof(filterToApply));
        }

        public IEnumerable<Step> StepsToExecute { get; }
        public Func<Integration, bool> FilterToApply { get; }

        public async Task<StepExecution> PerformStepExecutionAync(Integration integration)
        {
            if (this.FilterToApply(integration))
            {
                return new StepExecution();
            }

            foreach (var stepToExecute in this.StepsToExecute)
            {
                var result = await stepToExecute.PerformStepExecutionAync(integration);
                integration.AddStepExecutionResult(result);
            }

            return new StepExecution();
        }
    }
}
