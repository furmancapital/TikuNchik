using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Builders
{
    public class TargetStepBuilder
    {
        private TargetStepBuilder()
        {

        }

        public IStep To (IStep to)
        {
            return to;
        }
    }
}
