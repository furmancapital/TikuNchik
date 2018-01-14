using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Builders;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core
{
    public class IntegrationFlowBuilder<TBody> : IntegrationFlowBuilder
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

        private static IntegrationFlowBuilder<TBodyType> FromExisting<TBodyType>(IntegrationFlowBuilder sourceBuilder)
        {
            return new IntegrationFlowBuilder<TBodyType>()
            {
                CreatedFlow = sourceBuilder.CreatedFlow,
                DependencyInjection = sourceBuilder.DependencyInjection
            };
        }
    }


    public class IntegrationFlowBuilder
    {
        public Flow CreatedFlow
        {
            get;set;
        }

        protected ILoggerFactory LoggerFactory
        {
            get;
            set;
        }

        public IServiceProvider DependencyInjection
        {
            get;set;
        }

        public static IntegrationFlowBuilder<TBodyType> Create<TBodyType>(IServiceProvider dependencyInjection)
        {
            if (dependencyInjection == null)
            {
                throw new ArgumentNullException(nameof(dependencyInjection));
            }

            return new IntegrationFlowBuilder<TBodyType>()
            {
                DependencyInjection = dependencyInjection
            };
        }


        public IntegrationFlowBuilder()
        {
            this.CreatedFlow = new Flow();
        }
        



        

        /// <summary>
        /// Creates a step that will trigger the resolution of the target dependency on each call
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="actionToExecuteOnDependency"></param>
        /// <returns></returns>
        public IStep CreateFromServiceProvider<TItem>(Action<TItem, Integration> actionToExecuteOnDependency)
        {
            return new DependencyResolvedStep<TItem>(this.DependencyInjection, actionToExecuteOnDependency);
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
