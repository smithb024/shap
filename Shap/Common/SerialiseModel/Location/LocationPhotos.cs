namespace Shap.Common.SerialiseModel.Location
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a location's photo path.
    /// </summary>
    public class LocationPhotos
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationPhotos"/> class.
        /// </summary> 
        public LocationPhotos()
        {
        }

        /// <summary>
        /// Gets or sets photo path.
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
}
