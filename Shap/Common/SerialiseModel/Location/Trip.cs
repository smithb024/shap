namespace Shap.Common.SerialiseModel.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the details about a specific trip for the location.
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Trip"/> class.
        /// </summary> 
        public Trip()
        {
        }

        /// <summary>
        /// Gets or sets the first unit.
        /// </summary>
        [XmlAttribute("Ut1")]
        public string Unit1 { get; set; }

        /// <summary>
        /// Gets or sets the second unit.
        /// </summary>
        [XmlAttribute("Ut2")]
        public string Unit2 { get; set; }

        /// <summary>
        /// Gets or sets the third unit.
        /// </summary>
        [XmlAttribute("Ut3")]
        public string Unit3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth unit.
        /// </summary>
        [XmlAttribute("Ut4")]
        public string Unit4 { get; set; }

        /// <summary>
        /// Gets or sets origin.
        /// </summary>
        [XmlAttribute("From")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        [XmlAttribute("To")]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        [XmlAttribute("Rte")]
        public string Route { get; set; }

        /// <summary>
        /// Gets or sets the date of the trip.
        /// </summary>
        [XmlAttribute("Date")]
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the trip distance.
        /// </summary>
        [XmlAttribute("Dist")]
        public string Distance { get; set; }
    }
}
