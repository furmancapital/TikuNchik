using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core.Builders
{
    public class TranslatorStepBuilder<TFrom, TTo>
    {
        public static IntegrationFlowBuilder Translate (Func<TFrom, TTo> translator, IBuildableFlow sourceFlow, IntegrationFlowBuilder sourceBuilder)
        {
            var generatedStep = new TranslatorStep<TFrom, TTo>(translator);
            sourceFlow.AddStep(generatedStep);
            return sourceBuilder;
        }
    }
}
