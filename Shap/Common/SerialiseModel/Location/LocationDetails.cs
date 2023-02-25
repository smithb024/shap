namespace Shap.Common.SerialiseModel.Location
{
    using Shap.Common.SerialiseModel.Family;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the location details.
    /// </summary>
    [XmlRoot("LocationDetails")]
    public class LocationDetails
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationDetails"/> class.
        /// </summary> 
        public LocationDetails()
        {
        }

        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code of the location.
        /// </summary>
        [XmlAttribute("Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the size of the location.
        /// </summary>
        [XmlAttribute("Size")]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the year that the location opened.
        /// </summary>
        [XmlAttribute("Opened")]
        public int Opened { get; set; }

        /// <summary>
        /// Gets or sets the location of the location.
        /// </summary>
        [XmlAttribute("Shire")]
        public string Shire { get; set; }

        /// <summary>
        /// Gets or sets the type of the location.
        /// </summary>
        [XmlAttribute("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the collection of operators.
        /// </summary>
        [XmlElement("Operator")]
        public List<LocationOperators> Operators { get; set; }

        /// <summary>
        /// Gets or sets the collection of photos.
        /// </summary>
        [XmlElement("Photo")]
        public List<LocationPhotos> Photos { get; set; }
    }
}