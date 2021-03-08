namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the historical number.
    /// </summary>
    public class OldNumber
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OldNumber"/> class.
        /// </summary>
        public OldNumber()
        {
        }

        /// <summary>
        /// Gets or sets the historical number.
        /// </summary>
        [XmlAttribute("on")]
        public string Number { get; set; }
    }
}
