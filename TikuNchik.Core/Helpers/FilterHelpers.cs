using System;
using System.Collections.Generic;
using System.Text;

namespace TikuNchik.Core.Helpers
{
    /// <summary>
    /// This helper class is used to provide helper methods that relate to filtering
    /// </summary>
    public static class FilterHelpers
    {
        public const string FilteredOut = "FilteredOut";
        public const string FilteredOutReason = "FilteredOutReason";

        /// <summary>
        /// This method is used to figure out whether an integration was filtered out
        /// </summary>
        /// <param name="sourceIntegration"></param>
        /// <returns></returns>
        public static bool WasFilteredOut (this Integration sourceIntegration)
        {
            return sourceIntegration.Headers.ContainsKey(FilteredOut);
        }

        /// <summary>
        /// This method is used to retrieve the reason an integration was filtered out (if any)
        /// </summary>
        /// <param name="integration"></param>
        /// <returns>The reason if one exists; otherwise, a null will be returned</returns>
        public static string GetFilteredOutReason (this Integration integration)
        {
            object filteredOutReason;
            integration.Headers.TryGetValue(FilteredOutReason, out filteredOutReason);
            return filteredOutReason as string;
        }

        /// <summary>
        /// This method is ued to flag the integration as having been filtered out
        /// </summary>
        /// <param name="integration"></param>
        /// <param name="filteredOutReason"></param>
        public static void SetFilteredOut (this Integration integration, string filteredOutReason)
        {
            integration.AddHeader(FilteredOut, null);
            integration.AddHeader(FilteredOutReason, filteredOutReason);
        }
    }
}
