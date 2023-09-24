namespace Shap.Common.SerialiseModel.Family
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a single family.
    /// </summary>
    public class SingleFamily
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SingleFamily"/> class.
        /// </summary>
        public SingleFamily()
        {
        }
 
        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Collection of groups/classes.
        /// </summary>
        [XmlElement("Classes")]
        public List<SingleClass> Classes { get; set; }
    }
}