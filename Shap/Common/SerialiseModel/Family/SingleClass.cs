namespace Shap.Common.SerialiseModel.Family
{
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise a single class.
    /// </summary>
    public class SingleClass
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SingleClass"/> class.
        /// </summary>
        public SingleClass()
        {
        }
 
        /// <summary>
        /// Gets or sets the grpup/class name.
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}