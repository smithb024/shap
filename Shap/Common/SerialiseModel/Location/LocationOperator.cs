﻿namespace Shap.Common.SerialiseModel.Location
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a location's operator.
    /// </summary>
    public class LocationOperator
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationOperator"/> class.
        /// </summary> 
        public LocationOperator()
        {
        }

        /// <summary>
        /// Gets or sets operator name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a contemporary operator.
        /// </summary>
        [XmlAttribute("Ctp")]
        public bool IsContemporary { get; set; }
    }
}