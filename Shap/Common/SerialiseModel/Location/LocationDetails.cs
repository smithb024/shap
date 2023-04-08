namespace Shap.Common.SerialiseModel.Location
{
    using Shap.Common.SerialiseModel.Family;
    using Shap.Types.Enum;
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
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the year that the location opened.
        /// </summary>
        [XmlAttribute("Opened")]
        public string Opened { get; set; }

        /// <summary>
        /// Gets or sets the year that the location closed.
        /// </summary>
        [XmlAttribute("Closed")]
        public string Closed { get; set; }

        /// <summary>
        /// Gets or sets the location of the location.
        /// </summary>
        [XmlAttribute("County")]
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the type of the location.
        /// </summary>
        [XmlAttribute("Cat")]
        public LocationCategories Category { get; set; }

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

        /// <summary>
        /// Gets or sets the collection of operators.
        /// </summary>
        [XmlElement("Operator")]
        public List<LocationOperator> Operators { get; set; }

        /// <summary>
        /// Gets or sets the collection of photos.
        /// </summary>
        [XmlElement("Photo")]
        public List<LocationPhotos> Photos { get; set; }

        /// <summary>
        /// Gets or sets the collection of years.
        /// </summary>
        [XmlElement("Years")]
        public List<LocationYear> Years { get; set; }

        /// <summary>
        /// Gets or sets the collection of classes.
        /// </summary>
        [XmlElement("Clss")]
        public List<LocationClass> Classes { get; set; }

        /// <summary>
        /// Gets or sets the collection of trips.
        /// </summary>
        [XmlElement("Trips")]
        public List<Trip> Trips { get; set; }
    }
}