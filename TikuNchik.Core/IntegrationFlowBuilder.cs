using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TikuNchik.Core.Builders;
using TikuNchik.Core.Steps;

namespace TikuNchik.Core
{
    public class IntegrationFlowBuilder
    {
        private Flow CreatedFlow
        {
            get;set;
        }

        private ILoggerFactory LoggerFactory
        {
            get;
            set;
        }

        private IServiceProvider DependencyInjection
        {
            get;set;
        }

        public static IntegrationFlowBuilder Create(IServiceProvider dependencyInjection)
        {
            if (dependencyInjection == null)
            {
                throw new ArgumentNullException(nameof(dependencyInjection));
            }

            return new IntegrationFlowBuilder()
            {
                DependencyInjection = dependencyInjection
            };
        }

        private IntegrationFlowBuilder()
        {
            this.CreatedFlow = new Flow();
        }

        public ChoiceBuilder Choice()
        {
            return ChoiceBuilder.Choice(this.CreatedFlow, this);
        }

        public FilterBuilder Filter(Func<Integration, bool> filter)
        {
            return FilterBuilder.Filter(filter, this.CreatedFlow, this);
        }

        public IntegrationFlowBuilder WireTap()
        {
            return WireTapBuilder.WireTap(this.CreatedFlow, this, this.DependencyInjection);
        }

        public IntegrationFlowBuilder Log(Action<Integration, ILogger> actionToPerform)
        {
            return LogStepBuilder.Log(this.CreatedFlow, this, actionToPerform, this.DependencyInjection);
        }

        public ExceptionHandlerBuilder ExceptionHandler()
        {
            return new ExceptionHandlerBuilder(this.CreatedFlow, this, this.DependencyInjection);
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
