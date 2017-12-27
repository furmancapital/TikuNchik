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
    }
}
