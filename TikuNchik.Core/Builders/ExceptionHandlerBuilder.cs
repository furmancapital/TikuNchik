using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Builders
{
    public class ExceptionHandlerBuilder
    {


        private IBuildableFlow SourceFlow
        {
            get; set;
        }

        private IntegrationFlowBuilder Builder
        {
            get; set;
        }

        private ExceptionHandlerBuilder(IBuildableFlow sourceFlow, IntegrationFlowBuilder builder)
        {
            this.SourceFlow = sourceFlow;
            this.Builder = builder;
        }

        public ExceptionHandlerBuilder AddExceptionHandler(Func<Exception, bool> handlerToAdd)
        {
            throw new NotImplementedException();
        }
    }
}
