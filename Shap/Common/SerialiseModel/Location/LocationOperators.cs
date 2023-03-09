namespace Shap.Common.SerialiseModel.Location
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a location's operator.
    /// </summary>
    public class LocationOperators
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationOperators"/> class.
        /// </summary> 
        public LocationOperators()
        {
        }

        /// <summary>
        /// Gets or sets operator name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
