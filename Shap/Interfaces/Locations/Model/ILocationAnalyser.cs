namespace Shap.Interfaces.Locations.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a class which manages the location analysis.
    /// </summary>
    public interface ILocationAnalyser
    {
        /// <summary>
        /// Analyse all the locations in the list.
        /// </summary>
        /// <param name="locations">
        /// The locations to analyse.
        /// </param>
        void Analyse(List<string> locations);
    }
}