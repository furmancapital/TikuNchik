using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core
{
    /// <summary>
    /// This interface is used to represent a flow that is buildable from various steps.
    /// </summary>
    public interface IBuildableFlow
    {
        void AddStep(IStep step);
    }
}
