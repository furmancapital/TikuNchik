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

        private IntegrationFlowBuilder<string> FlowBuilder
        {
            get;set;
        }

        public ChoiceBuilderTests()
        {
            this.ServiceProvider = new Mock<IServiceProvider>();
            this.FlowBuilder = new IntegrationFlowBuilder<string>(this.ServiceProvider.Object);
        }

        private Integration<string> CreateIntegration()
        {
            return new Integration<string>("A");
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

        [Fact]
        public void EndChoice_Verify_Matching_Default_Properly_Triggered()
        {
            using (var choiceTriggered = new ManualResetEventSlim())
            {
                var flow = this.FlowBuilder
                    .Choice()
                        .When((x) => x.Headers.ContainsKey("A"))
                        //do nothing
                        .EndWhen()
                        .Default()
                            .AddDefaultStep((x) => choiceTriggered.Set())
                        .EndDefault()
                    .EndChoice()
                .Build();

                var integration = this.CreateIntegration();
                integration.AddHeader("B", null);
                flow.ExecuteCurrentIntegration(integration);

                Assert.True(choiceTriggered.Wait(TimeSpan.FromSeconds(0)));

            }
        }


        [Fact]
        public void EndChoice_Verify_Matcher_Properly_Triggered_Even_If_Added_After_Default()
        {
            using (var choiceTriggered = new ManualResetEventSlim())
            {
                var flow = this.FlowBuilder
                    .Choice()
                        .Default()
                        .EndDefault()
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
    }
}
