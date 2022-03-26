namespace Shap.Io
{
    using Shap.Config;
    using Shap.Interfaces.Io;
    using Shap.Units.IO;

    /// <summary>
    /// Interface for a factory which contains all the IO Controllers.
    /// </summary>
    public class IoControllers : IIoControllers
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="IoControllers"/> class.
        /// </summary>
        public IoControllers()
        {
            this.Gac = new GroupsAndClassesIOController();
            this.Family = new XmlFamilyIoController();
            this.Units = new UnitsIOController();
            this.UnitsXml = new UnitsXmlIOController();
        }

        /// <summary>
        /// Gets the Groups and Classes IO Controller.
        /// </summary>
        public IGroupsAndClassesIOController Gac { get; }

        /// <summary>
        /// Gets the Family IO Controller
        /// </summary>
        public IXmlFamilyIoController Family { get; }

        /// <summary>
        /// Gets the Units IO Controller.
        /// </summary>
        public IUnitsIoController Units { get; }

        /// <summary>
        /// Gets the Units IO XML Controller.
        /// </summary>
        public IUnitsXmlIoController UnitsXml { get; }
    }
}