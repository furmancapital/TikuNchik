using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core.Steps
{
    /// <summary>
    /// This step is used to perform a conversion between message types
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    public class TranslatorStep<TBody, TTo> : Step<TBody>
    {
        public TranslatorStep (Func<TBody, TTo> conversion)
        {
            Conversion = conversion ?? throw new ArgumentNullException(nameof(conversion));
        }

        public Func<TBody, TTo> Conversion { get; }

        public override Task PerformStepExecutionAsync(Integration integration, TBody message)
        {
            var convertedValue = this.Conversion(message);
            integration.Body = convertedValue;
            return Task.FromResult(convertedValue);
        }
    }
}
