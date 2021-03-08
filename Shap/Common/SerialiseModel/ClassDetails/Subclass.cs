namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the subclass details.
    /// </summary>
    public class Subclass
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Subclass"/> class.
        /// </summary> 
        public Subclass()
        {
        }

        /// <summary>
        /// Gets or sets the subclass id.
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the collection of images.
        /// </summary>
        [XmlElement("Image")]
        public List<Image> Images { get; set; }

        /// <summary>
        /// Gets or sets the collection of numbers.
        /// </summary>
        [XmlElement("Number")]
        public List<Number> Numbers { get; set; }
    }
}
