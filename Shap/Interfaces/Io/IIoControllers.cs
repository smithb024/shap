namespace Shap.Interfaces.Io
{
    /// <summary>
    /// Interface for a factory which contains all the IO Controllers.
    /// </summary>
    public interface IIoControllers
    {
        /// <summary>
        /// Gets the Groups and Classes IO Controller.
        /// </summary>
        IGroupsAndClassesIOController Gac { get; }

        /// <summary>
        /// Gets the Family IO Controller
        /// </summary>
        IXmlFamilyIoController Family { get; }

        /// <summary>
        /// Gets the Units IO Controller.
        /// </summary>
        IUnitsIoController Units { get; }

        /// <summary>
        /// Gets the Units IO XML Controller.
        /// </summary>
        IUnitsXmlIoController UnitsXml { get; }
    }
}