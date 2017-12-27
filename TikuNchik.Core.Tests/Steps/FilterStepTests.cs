﻿using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class FilterStepTests
    {
        private Mock<IStep> TargetStep
        {
            get; set;
        }

        private FilterStep GetFilterStepWithoutLambda(Func<Integration, bool> filter)
        {
            return new FilterStep(filter, new[]
            {
                this.TargetStep.Object
            });
        }

        private FilterStep GetFilterStepWithLambda (Func<Integration, bool> filter, Action<Integration> actionToExecute)
        {
            return new FilterStep(filter, actionToExecute);
        }

        public FilterStepTests()
        {
            this.TargetStep = new Mock<IStep>();
        }

        [Fact]
        public void PerformStepExecutionAync_If_FilteredOut_DoNotExecute_Step()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Returns(Task.FromResult(0));

            var step = this.GetFilterStepWithoutLambda(((x) => false));

            step.PerformStepExecutionAync(new Integration()).Wait();

            this.TargetStep.Verify(x => x.PerformStepExecutionAync(It.IsAny<Integration>()), Times.Never());
        }


        [Fact]
        public void PerformStepExecutionAync_If_NotFilteredOut_Execute_Step()
        {
            this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                .Returns(Task.FromResult(0));

            var step = this.GetFilterStepWithoutLambda(((x) => true));

            step.PerformStepExecutionAync(new Integration()).Wait();

            this.TargetStep.VerifyAll();
        }

        [Fact]
        public void PerformStepExecutionAync_If_FilteredOut_DoNotExecute_Lambda()
        {
            using (var lambdaCalled = new ManualResetEventSlim())
            {

                this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                    .Returns(Task.FromResult(0));

                var step = this.GetFilterStepWithLambda(((x) => false), (x) => lambdaCalled.Set());

                step.PerformStepExecutionAync(new Integration()).Wait();

                Assert.False(lambdaCalled.Wait(TimeSpan.FromSeconds(0)), "SHould not trigger action lambda when filtered out");
            }
        }


        [Fact]
        public void PerformStepExecutionAync_If_NotFilteredOut_Execute_Lambda()
        {
            using (var lambdaCalled = new ManualResetEventSlim())
            {

                this.TargetStep.Setup(x => x.PerformStepExecutionAync(It.IsAny<Integration>()))
                    .Returns(Task.FromResult(0));

                var step = this.GetFilterStepWithLambda(((x) => true), (x) => lambdaCalled.Set());

                step.PerformStepExecutionAync(new Integration()).Wait();

                Assert.True(lambdaCalled.Wait(TimeSpan.FromSeconds(0)), "Should trigger action lambda when NOT filtered out");
            }
        }
    }
}