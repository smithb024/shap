namespace Shap.Interfaces.Locations.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Manages the locations
    /// </summary>
    public interface ILocationManager
    {
        /// <summary>
        /// Gets a collection of all available locations.
        /// </summary>
        /// <returns>A list of locations</returns>
        List<string> GetLocations();

        /// <summary>
        /// Build/Rebuild the xaml files to ensure a complete set is available.
        /// </summary>
        void Initialise();
    }
}