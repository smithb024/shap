namespace Shap.Locations.Messages
{
    /// <summary>
    /// Message to request that the selector view model is populated with locations in the 
    /// indicated region.
    /// </summary>
    public class RegionSelectorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="RegionSelectorMessage"/> class.
        /// </summary>
        /// <param name="region">the region to select</param>
        public RegionSelectorMessage(
            string region)
        {
            this.Region = region;
        }

        /// <summary>
        /// Gets the region to display.
        /// </summary>
        public string Region { get; }
    }
}
