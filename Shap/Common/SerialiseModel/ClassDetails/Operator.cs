namespace Shap.Common.SerialiseModel.ClassDetails
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a class's operator.
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Operator"/> class.
        /// </summary> 
        public Operator()
        {
        }

        /// <summary>
        /// Gets or sets the subclass id.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}