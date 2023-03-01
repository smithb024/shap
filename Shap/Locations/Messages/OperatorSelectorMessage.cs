namespace Shap.Locations.Messages
{
    /// <summary>
    /// Message to request that the selector view model is populated with locations managed by 
    /// the indicated operator.
    /// </summary>
    public class OperatorSelectorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorSelectorMessage"/> class.
        /// </summary>
        /// <param name="operator">the operator to select</param>
        public OperatorSelectorMessage(
            string @operator)
        {
            this.Operator = @operator;
        }

        /// <summary>
        /// Gets the operator to display.
        /// </summary>
        public string Operator { get; }
    }
}
