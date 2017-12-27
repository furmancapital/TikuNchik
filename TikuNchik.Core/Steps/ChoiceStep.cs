using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to provide the ability to specify individual criteria for a series of steps 
    /// and indicate which one will be called
    /// </summary>
    public class ChoiceStep : Step
    {
        public ChoiceStep (IEnumerable<KeyValuePair<Func<Integration, bool>, IStep>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        public IEnumerable<KeyValuePair<Func<Integration, bool>, IStep>> Criteria { get; }

        public override async Task PerformStepExecutionAync(Integration integration)
        {
            foreach (var criteria in this.Criteria)
            {
                if (criteria.Key(integration))
                {
                    await criteria.Value.PerformStepExecutionAync(integration);
                    break;
                }
            }
        }

    }
}
