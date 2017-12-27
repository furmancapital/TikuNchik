using System;
using System.Collections.Generic;

namespace TikuNchik.Core
{
    public class Integration
    {
        public Guid Id
        {
            get;
            private set;
        } = Guid.NewGuid();

        private Stack<StepExecution> _stepExecutions = new Stack<StepExecution>();

        public void AddStepExecutionResult (StepExecution stepExecution)
        {
            _stepExecutions.Push(stepExecution);
        }

    }


}
