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

            var flow = IntegrationFlowBuilder
                .Create<int>(serviceCollection)
                .Translate((x) => "A")

                .ExceptionHandler()
                    .AddExceptionHandler<InvalidOperationException>((exc, integration) => false)
                .EndExceptionHandler()

                .Filter((integration) => integration.Id != null)
                    .AddStepToExecute((x) => { })
                .EndFilter()
                
                .Choice()
                    .When(x =>  x.Id != null)
                        .AddChoiceStep((x) =>
                        {
                            System.Console.WriteLine("AAAAA");
                       })
                    .EndWhen()
                    .Default()
                        .AddDefaultStep((x) =>
                        {
                            System.Console.WriteLine("AAAAA");
                        })
                     .EndDefault()
                .EndChoice()
                .WireTap()
                .Log((x, y) => y.LogInformation("This is only a test!"))
                .Build();

            var result = await flow.CreateIntegration<int>(1);
            if (result == null)
            {

            }
                
        }
    }
}
