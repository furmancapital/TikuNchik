using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TikuNchik.Core
{
    public interface IFlow
    {
        Task<Integration> CreateIntegration<T>(T sourceMessage);
        Task ExecuteCurrentIntegration(Integration integration);
    }
}
