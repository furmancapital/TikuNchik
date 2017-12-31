using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TikuNchik.Core.Helpers
{
    public class FilterHelpersTests
    {
        private Integration CreateIntegration()
        {
            return new Integration();
        }

        [Fact]
        public void WasFilteredOut_ReturnTrue_If_FilteredOut()
        {
            var integration = this.CreateIntegration();
            integration.SetFilteredOut("A");
            Assert.True(integration.WasFilteredOut());
        }

        [Fact]
        public void WasFilteredOut_ReturnFalse_If_NOT_FilteredOut()
        {
            var integration = this.CreateIntegration();
            Assert.False(integration.WasFilteredOut());
        }

        [Fact]
        public void GetFilteredOutReason_Return_Reason_If_Exists()
        {
            var integration = this.CreateIntegration();
            integration.SetFilteredOut("A");
            Assert.Equal("A", integration.GetFilteredOutReason());

        }

        [Fact]
        public void GetFilteredOutReason_Return_Null_If_Does_Not_Exist()
        {
            var integration = this.CreateIntegration();
            Assert.Null(integration.GetFilteredOutReason());

        }

    }
}
