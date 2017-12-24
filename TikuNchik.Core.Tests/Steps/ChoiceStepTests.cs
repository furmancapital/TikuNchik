using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class ChoiceStepTests
    {
        private Mock<IStep> Step1
        {
            get;set;
        }

        private Mock<IStep> Step2
        {
            get; set;
        }


        private ChoiceStep GetChoiceStep(IEnumerable<KeyValuePair<Func<Integration, bool>, IStep>> choices)
        {
            return new ChoiceStep(choices);
        }


        public ChoiceStepTests()
        {
            this.Step1 = new Moq.Mock<IStep>();
            this.Step2 = new Moq.Mock<IStep>();

        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        [Fact]
        public void PerformStepExecutionAync_Match_Should_Trigger_Execution()
        {
            this.Step1.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Returns(Task.FromResult<StepExecution>(new StepExecution()));

            Func<Integration, bool> lambda = (x) => true;

            var choiceStep = this.GetChoiceStep(new[]
            {
                new KeyValuePair<Func<Integration, bool>, IStep>(lambda, this.Step1.Object)
            });

            choiceStep.PerformStepExecutionAync(this.CreateIntegration()).Wait();

            this.Step1.VerifyAll();
        }

        [Fact]
        public void PerformStepExecutionAync_Match_Should_Trigger_Execution_If_Not_First_Item()
        {
            this.Step2.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Returns(Task.FromResult(new StepExecution()));

            Func<Integration, bool> lambda1 = (x) => false;
            Func<Integration, bool> lambda2 = (x) => true;

            var choiceStep = this.GetChoiceStep(new[]
            {
                new KeyValuePair<Func<Integration, bool>, IStep>(lambda1, this.Step1.Object),
                new KeyValuePair<Func<Integration, bool>, IStep>(lambda2, this.Step2.Object)
            });

            choiceStep.PerformStepExecutionAync(this.CreateIntegration()).Wait();

            this.Step2.VerifyAll();
        }



        [Fact]
        public void PerformStepExecutionAync_NonMatch_Should_NOT_Trigger_Execution()
        {

            Func<Integration, bool> lambda = (x) => false;

            var choiceStep = this.GetChoiceStep(new[]
            {
                new KeyValuePair<Func<Integration, bool>, IStep>(lambda, this.Step1.Object),
                new KeyValuePair<Func<Integration, bool>, IStep>(lambda, this.Step2.Object)
            });

            choiceStep.PerformStepExecutionAync(this.CreateIntegration()).Wait();

            this.Step1.Verify(x => x.PerformStepExecutionAync(It.IsAny<Integration>()), Times.Never());
            this.Step2.Verify(x => x.PerformStepExecutionAync(It.IsAny<Integration>()), Times.Never());

        }
    }
}
