namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the number details.
    /// </summary>
    public class Number
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Number"/> class.
        /// </summary>
        public Number()
        { 
        }

        /// <summary>
        /// Gets or sets the current number.
        /// </summary>
        [XmlAttribute("No")]
        public int CurrentNumber { get; set; }

        /// <summary>
        /// Gets or sets the historical numbers.
        /// </summary>
        [XmlElement("OldNumber")]
        public List<OldNumber> Historical { get; set; }
    }
}
