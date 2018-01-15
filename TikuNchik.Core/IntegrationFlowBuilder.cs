using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Builders;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core
{
    public class IntegrationFlowBuilder<TBody>
    {
        public IntegrationFlowBuilder<TTo> Translate<TTo>(Func<TBody, TTo> translator)
        {
            return FromExisting<TTo>(TranslatorStepBuilder<TBody, TTo>
                .Translate(translator, this.CreatedFlow, this));
        }


        public ExceptionHandlerBuilder<TBody> ExceptionHandler()
        {
            return new ExceptionHandlerBuilder<TBody>(this.CreatedFlow, this, this.DependencyInjection);
        }

        public FilterBuilder<TBody> Filter(Func<Integration, bool> filter)
        {
            return FilterBuilder<TBody>.Filter(filter, this.CreatedFlow, this);
        }

        public ChoiceBuilder<TBody> Choice()
        {
            return ChoiceBuilder<TBody>.Choice(this.CreatedFlow, this);
        }

        public IntegrationFlowBuilder<TBody> WireTap()
        {
            return WireTapBuilder.WireTap(this.CreatedFlow, this, this.DependencyInjection);
        }


        public IntegrationFlowBuilder<TBody> Log(Action<Integration, ILogger> actionToPerform)
        {
            return LogStepBuilder.Log(this.CreatedFlow, this, actionToPerform, this.DependencyInjection);
        }

        private static IntegrationFlowBuilder<TBodyType> FromExisting<TBodyType>(IntegrationFlowBuilder<TBody> sourceBuilder)
        {
            return new IntegrationFlowBuilder<TBodyType>(sourceBuilder.DependencyInjection)
            {
                CreatedFlow = sourceBuilder.CreatedFlow
            };
        }

        public Flow CreatedFlow
        {
            get;
            private set;
        }

        protected ILoggerFactory LoggerFactory
        {
            get;
            private set;
        }

        public IServiceProvider DependencyInjection
        {
            get;
            private set;
        }

        public IntegrationFlowBuilder(IServiceProvider serviceProvider)
        {
            this.DependencyInjection = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.CreatedFlow = new Flow();
        }

        public IFlow Build()
        {
            //clear out the created flow so that this instance can be used again for
            //subsequent flow creations
            var createdFlow = this.CreatedFlow;
            this.CreatedFlow = null;
            return createdFlow;
        }
    }
}
