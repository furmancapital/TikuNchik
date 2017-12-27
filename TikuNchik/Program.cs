using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;
using TikuNchik.Core;
using TikuNchik.Core.Steps;

namespace TikuNchik
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var serviceCollection = new ServiceCollection()
                .AddLogging((x) =>
                {
                    x.AddConsole();
                })
                .BuildServiceProvider();

            var factory = serviceCollection.GetService<ILoggerFactory>();

            var flow = IntegrationFlowBuilder
                .Create(serviceCollection)
                .WireTap()
                .Filter(x => x.Id != null)
                    .AddStepToExecute((x) =>
                    {
                        System.Console.WriteLine(x);
                    })
                .EndFilter()
                .Choice()
                    .When(x => x.Id != null)
                        .AddStep((x) =>
                        {
                            System.Console.WriteLine(x);
                        })
                    .EndWhen()
                .EndChoice()
                .Log((x, y) => y.LogInformation("This is only a test!"))
                .Build();

            var result = await flow.CreateIntegration<string>("ABC");
            if (result == null)
            {

            }
                
        }
    }
}
