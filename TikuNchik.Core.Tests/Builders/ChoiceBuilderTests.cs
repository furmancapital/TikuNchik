using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace TikuNchik.Core.Builders
{
    public class ChoiceBuilderTests
    {

 
        private Mock<IServiceProvider> ServiceProvider
        {
            get;set;
        }

        private IntegrationFlowBuilder FlowBuilder
        {
            get;set;
        }

        public ChoiceBuilderTests()
        {
            this.ServiceProvider = new Mock<IServiceProvider>();
            this.FlowBuilder = IntegrationFlowBuilder.Create(this.ServiceProvider.Object);
        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        [Fact]
        public void EndChoice_Verify_Matching_Choice_Correctly_Processed()
        {
            using (var choiceTriggered = new ManualResetEventSlim())
            {
                var flow = this.FlowBuilder
                    .Choice()
                        .When((x) => x.Headers.ContainsKey("A"))
                            .AddChoiceStep((x) => choiceTriggered.Set())
                        .EndWhen()
                    .EndChoice()
                .Build();

                var integration = this.CreateIntegration();
                integration.AddHeader("A", null);
                flow.ExecuteCurrentIntegration(integration);

                Assert.True(choiceTriggered.Wait(TimeSpan.FromSeconds(0)));

            }
        }


        [Fact]
        public void EndChoice_Verify_Matching_Choice_Correctly_Processed_When_Multiple_Conditions()
        {
            using (var choiceTriggered = new ManualResetEventSlim())
            {
                var flow = this.FlowBuilder
                    .Choice()
                        .When((x) => x.Headers.ContainsKey("A"))
                            //do nothing
                        .EndWhen()
                        .When((x) => x.Headers.ContainsKey("B"))
                            .AddChoiceStep((x) => choiceTriggered.Set())
                        .EndWhen()
                    .EndChoice()
                .Build();

                var integration = this.CreateIntegration();
                integration.AddHeader("B", null);
                flow.ExecuteCurrentIntegration(integration);

                Assert.True(choiceTriggered.Wait(TimeSpan.FromSeconds(0)));

            }
        }
    }
}
