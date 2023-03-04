namespace Shap.Locations.Messages
{
    /// <summary>
    /// A message which indicates that a new configuration mode has been selected.
    /// </summary>
    public class ConfigurationModeMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationModeMessage"/> message.
        /// </summary>
        /// <param name="inConfigurationMode">
        /// The new configuration mode value.
        /// </param>
        public ConfigurationModeMessage(
            bool inConfigurationMode)
        {
            this.InConfigurationModel = inConfigurationMode;
        }

        /// <summary>
        /// Indicates whether configuration mode has been set or not.
        /// </summary>
        public bool InConfigurationModel { get; }
    }
}