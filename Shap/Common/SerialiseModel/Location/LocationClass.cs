namespace Shap.Common.SerialiseModel.Location
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the details about a specific class for the location.
    /// </summary>
    public class LocationClass
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationClass"/> class.
        /// </summary> 
        public LocationClass()
        {
        }

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        [XmlAttribute("Cls")]
        public string Name { get; set; }

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