﻿namespace Shap.Interfaces.Io
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
        /// Gets the Operator IO Controller
        /// </summary>
        IXmlOperatorIoController Operator { get; }

        /// <summary>
        /// Gets the Units IO Controller.
        /// </summary>
        IUnitsIoController Units { get; }

        /// <summary>
        /// Gets the Units IO XML Controller.
        /// </summary>
        IUnitsXmlIoController UnitsXml { get; }

        /// <summary>
        /// Gets the location IO Controller.
        /// </summary>
        ILocationIoController Location { get; }
    }
}