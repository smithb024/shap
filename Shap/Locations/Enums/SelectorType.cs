namespace Shap.Locations.Enums
{
    /// <summary>
    /// Enumeration which is used to determine the type of selector being used on the locations
    /// window.
    /// </summary>
    public enum SelectorType
    {
        /// <summary>
        /// The locations are being arranged alphabetically.
        /// </summary>
        Alphabetical,

        /// <summary>
        /// The locations are restricted to a specific operator
        /// </summary>
        Operator,

        /// <summary>
        /// The locations are restricated to a specific region.
        /// </summary>
        Region,

        /// <summary>
        /// The locations are restricted to a specific line.
        /// </summary>
        Lines
    }
}