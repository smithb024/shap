namespace Shap.Common.SerialiseModel.Location
{
    using NynaeveLib.Types;
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
        [XmlAttribute("dd")]
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the first unit.
        /// </summary>
        [XmlAttribute("MM")]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the first unit.
        /// </summary>
        [XmlAttribute("YY")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the first unit.
        /// </summary>
        [XmlAttribute("Id")]
        public string Id { get; set; }

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
