using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Builders
{
    public class ChoiceBuilder
    {
        private ChoiceBuilder()
        {

        }

        public static ChoiceBuilder Choice()
        {
            return new ChoiceBuilder();
        }

        public TargetStepBuilder When (Func<Integration, bool> matcher)
        {
            throw new NotImplementedException();
        }

    }
}
