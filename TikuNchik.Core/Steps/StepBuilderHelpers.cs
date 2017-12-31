using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Steps
{
    public class StepBuilderHelpers
    {
        public static IStep FromLambda (Action<Integration> action)
        {
            return new StepFromLambda(action);
        }

        public static IStep WrapMultipleSequentialSteps (IEnumerable<IStep> steps)
        {
            return new WrapSequentialSteps(steps);
        }
    }
}
