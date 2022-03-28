namespace Shap.Common.SerialiseModel.Operator
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the operator details.
    /// </summary>
    [XmlRoot("OperatorDetails")]
    public class OperatorDetails
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorDetails"/> class.
        /// </summary>
        public OperatorDetails()
        {
        }

        /// <summary>
        /// Gets or sets the Collection of operators.
        /// </summary>
        [XmlElement("Operators")]
        public List<SingleOperator> Operators { get; set; }
    }
}