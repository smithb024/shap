namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the image path.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
