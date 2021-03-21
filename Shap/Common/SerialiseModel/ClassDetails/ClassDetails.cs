namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the class details.
    /// </summary>
    [XmlRoot("ClassDetails")]
    public class ClassDetails
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ClassDetails"/> class.
        /// </summary> 
        public ClassDetails()
        {
        }

        /// <summary>
        /// Gets or sets the id of the class details.
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version of the latest version of this file.
        /// </summary>
        [XmlElement("ClassVersion")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the year the class was introduced.
        /// </summary>
        [XmlElement("Year")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the formation of the class.
        /// </summary>
        [XmlElement("Formation")]
        public string Formation { get; set; }

        /// <summary>
        /// Gets or sets the alpha id associated with the class.
        /// </summary>
        [XmlElement("AlphaId")]
        public string AlphaId { get; set; }

        /// <summary>
        /// Gets or sets the collection of subclasses.
        /// </summary>
        [XmlElement("Subclass")]
        public List<Subclass> Subclasses { get; set; }
    }
}
