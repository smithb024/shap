namespace Shap.Common.SerialiseModel.Family
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Class used to serialise and deserialise the family details.
    /// </summary>
    [XmlRoot("FamilyDetails")]
    public class FamilyDetails
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FamilyDetails"/> class.
        /// </summary>
        public FamilyDetails()
        {
        }

        /// <summary>
        /// Gets or sets the Collection of families.
        /// </summary>
        [XmlElement("Families")]
        public List<SingleFamily> Families { get; set; }
    }
}
