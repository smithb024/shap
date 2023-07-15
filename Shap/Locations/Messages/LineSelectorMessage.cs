namespace Shap.Locations.Messages
{
    /// <summary>
    /// Message to request that the selector view model is populated with locations in the 
    /// indicated line.
    /// </summary>
    public class LineSelectorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LineSelectorMessage"/> class.
        /// </summary>
        /// <param name="line">the line to select</param>
        public LineSelectorMessage(
            string line)
        {
            this.Line = line;
        }

        /// <summary>
        /// Gets the line to display.
        /// </summary>
        public string Line { get; }
    }
}
