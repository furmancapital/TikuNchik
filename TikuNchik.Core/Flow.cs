using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core
{
    public class Flow
    {
        public Flow()
        {

        }

        private List<IStep> Steps
        {
            get;
            set;
        } = new List<IStep>();

        public void AddStep (IStep step)
        {
            this.Steps.Add(step);
        }
    }
}
