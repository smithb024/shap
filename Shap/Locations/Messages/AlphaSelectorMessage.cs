namespace Shap.Locations.Messages
{
    /// <summary>
    /// Message to request that the selector view model is populated with locations started 
    /// with the attached value.
    /// </summary>
    public class AlphaSelectorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AlphaSelectorMessage"/> class.
        /// </summary>
        /// <param name="character">the character to select</param>
        public AlphaSelectorMessage(
            string character)
        {
            this.Character = character;
        }

        /// <summary>
        /// Gets the character to display.
        /// </summary>
        public string Character { get; }
    }
}
