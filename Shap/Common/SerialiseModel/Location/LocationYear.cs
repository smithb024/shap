namespace Shap.Common.SerialiseModel.Location
{
    using Shap.Types.Enum;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the details about a specific year for the location.
    /// </summary>
    public class LocationYear
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationOperator"/> class.
        /// </summary> 
        public LocationYear()
        {
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        [XmlAttribute("Yr")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets number from.
        /// </summary>
        [XmlAttribute("From")]
        public int TotalFrom { get; set; }

        /// <summary>
        /// Gets or sets the number to.
        /// </summary>
        [XmlAttribute("To")]
        public int TotalTo { get; set; }
    }
}