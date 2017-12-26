using System;
using TikuNchik.Core;

namespace TikuNchik
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IntegrationFlowBuilder
                .Create()
                .WireTap()
                .Filter(x => x.Id == null)
                    .AddStepToExecute(null)
                    .AddStepToExecute(null)
                .EndFilter()
                .Choice()
                    .When(x => x.Id == null)
                        .AddStep(null)
                        .AddStep(null)
                    .EndWhen()
                .EndChoice()
                .Build();
                
                
                
        }
    }
}
