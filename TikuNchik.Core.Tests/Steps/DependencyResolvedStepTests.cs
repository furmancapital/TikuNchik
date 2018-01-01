using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class DependencyResolvedStepTests
    {
        public interface IMyTestInterface
        {
            void DoSomething();
        }

        private Mock<IServiceProvider> ServiceProvider
        {
            get;set;
        }

        private Mock<IMyTestInterface> MockDependency
        {
            get;set;
        }

        private DependencyResolvedStep<IMyTestInterface> GetStep(Action<IMyTestInterface, Integration> action)
        {
            return new DependencyResolvedStep<IMyTestInterface>(this.ServiceProvider.Object, action);
        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        public DependencyResolvedStepTests()
        {
            this.ServiceProvider = new Mock<IServiceProvider>();
            this.MockDependency = new Mock<IMyTestInterface>();
            this.ServiceProvider.Setup(x => x.GetService(typeof(IMyTestInterface)))
                .Returns(this.MockDependency.Object);

        }


        [Fact]
        public void PerformStepExecutionAync_Ensure_Resolved_Dependency_Called()
        {
            this.MockDependency.Setup(x => x.DoSomething());

            this.GetStep((x, y) => 
            {
                x.DoSomething();
            })
            .PerformStepExecutionAync(this.CreateIntegration())
            .Wait();

            this.MockDependency.VerifyAll();
        }
    }
}
