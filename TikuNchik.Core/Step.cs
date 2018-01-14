using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public abstract class Step<TBody> : IStep<TBody>
    {
        protected Step()
        {
            this.ConversionRoute = new Func<object, TBody>((x) => (TBody)x);
        }

        public abstract Task PerformStepExecutionAsync(Integration integration, TBody message);

        public virtual async Task PerformStepExecutionAsync(Integration integration)
        {
            await PerformStepExecutionAsync(integration, ConversionRoute(integration.Body));
        }

        Func<object, TBody> ConversionRoute
        {
            get;
            set;
        }
    }

}
