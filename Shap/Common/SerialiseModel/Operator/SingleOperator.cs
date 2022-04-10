namespace Shap.Common.SerialiseModel.Operator
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a single operator.
    /// </summary>
    public class SingleOperator
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SingleOperator"/> class.
        /// </summary>
        public SingleOperator()
        {
        }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        [XmlAttribute("IsActive")]
        public bool IsActive { get; set; }
    }
}