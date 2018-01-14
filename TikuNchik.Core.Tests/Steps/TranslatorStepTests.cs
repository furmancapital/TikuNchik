using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TikuNchik.Core.Steps
{
    public class TranslatorStepTests
    {
        private class TestException : Exception
        {

        }

        public TranslatorStep<int, string> GetTranslatorWithValidConversion()
        {
            return new TranslatorStep<int, string>((x) => x.ToString());
        }

        public TranslatorStep<int, string> GetTranslator(Func<int, string> conversion)
        {
            return new TranslatorStep<int, string>(conversion);
        }

        private Integration CreateIntegration()
        {
            return new Integration();
        }

        [Fact]
        public void PerformStepExecutionAsync_Verify_Conversion_Applied_And_Stored_In_Body_Of_Integration()
        {
            var integration = this.CreateIntegration();
            var result = this.GetTranslatorWithValidConversion().PerformStepExecutionAsync(integration, 23);
            result.Wait();
            Assert.Equal("23", ((Integration<string>)result.Result).Message);
        }

        [Fact]
        public void PerformStepExecutionAsync_Verify_If_Exception_Thrown_Body_Not_Modified()
        {
            var integration = this.CreateIntegration();
            integration.Body = "A";

            Assert.Throws<TestException>(() =>
            {
                this.GetTranslator((x) => throw new TestException()).PerformStepExecutionAsync(integration, 23).Wait();
            });

            Assert.Equal("A", integration.Body);
        }
    }
}
