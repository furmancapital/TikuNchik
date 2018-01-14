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

        public abstract Task<Integration> PerformStepExecutionAsync(Integration integration, TBody message);

        public virtual async Task<Integration> PerformStepExecutionAsync(Integration integration)
        {
            return await PerformStepExecutionAsync(integration, ConversionRoute(integration.Body));
        }

        Func<object, TBody> ConversionRoute
        {
            get;
            set;
        }
    }

}
