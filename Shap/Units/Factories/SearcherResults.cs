namespace Shap.Units.Factories
{
    using System;
    using System.Collections.Generic;

    using Shap.Interfaces.Types;

    /// <summary>
    /// Class which provides a way of returning the results from the searcher factory.
    /// </summary>
    public class SearcherResults
    {
        public SearcherResults(
            List<IJourneyDetailsType> foundJourneys,
            DateTime lastDateChecked)
        {
            this.FoundJourneys = foundJourneys;
            this.LastDateChecked = lastDateChecked;
        }

        /// <summary>
        /// Gets or sets a list of found journeys.
        /// </summary>
        /// <remarks>
        /// Journeys are in chronolocal order.The first one is the oldest.
        /// </remarks>
        public List<IJourneyDetailsType> FoundJourneys { get; }

        /// <summary>
        /// Gets the date of the last checked item.
        /// </summary>
        public DateTime LastDateChecked { get; }
    }
}
