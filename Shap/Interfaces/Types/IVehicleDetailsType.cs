namespace Shap.Interfaces.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NynaeveLib.Types;
    using NynaeveLib.ViewModel;

    using Shap.Common.ViewModel;
    using Shap.Interfaces.Common.ViewModels;
    using Shap.Types.Enum;
    using Shap.Types;

    /// <summary>
    /// Interface describing the vehicle details.
    /// </summary>
    public interface IVehicleDetailsType
    {
        /// <summary>
        /// Gets the number
        /// </summary>
        string UnitNumber { get; }

        /// <summary>
        /// Gets the distance
        /// </summary>
        MilesChains UnitDistance { get; }

        /// <summary>
        /// Gets the distance as a string
        /// </summary>
        string UnitDistanceString { get; }

        /// <summary>
        /// Gets the initial distance.
        /// </summary>
        MilesChains UnitOrigDistance { get; }

        /// <summary>
        /// Gets the initial distance as a string.
        /// </summary>
        string UnitOrigDistanceString { get; }

        /// <summary>
        /// Gets the difference between the original distance and the current distance as a string.
        /// </summary>
        string UnitDistanceDifferenceString { get; }

        DateTime? UnitLastDate { get; }

        string UnitLastDateString { get; }

        DateTime? UnitFirstDate { get; }

        string UnitFirstDateString { get; }

        /// <summary>
        /// Gets the date of the last check.
        /// </summary>
        DateTime UnitLastCheck { get; set; }

        /// <summary>
        /// Gets the date of the last check.
        /// </summary>
        string UnitLastCheckString { get; }

        /// <summary>
        /// Gets or sets the notes associated with this <see cref="IVehicleDetailsType"/>
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// Gets or sets the former numbers.
        /// </summary>
        IVehicleNumberType FormerNumbers { get; }

        /// <summary>
        /// Gets or setsthe list of <see cref="JourneyDetailsType"/> associated with this
        /// <see cref="IVehicleDetailsType"/> 
        /// </summary>
        List<IJourneyViewModel> JourneysList { get; set; }

        List<IJourneyViewModel> JourneysCutdownList { get; }

        /// <summary>
        /// Gets the number of journeys
        /// </summary>
        int NumberOfJnys { get; }

        LocationCounterManagerViewModel ToLocation { get; }

        LocationCounterManagerViewModel FromLocation { get; }

        /// <summary>
        /// Gets the current service status.
        /// </summary>
        VehicleServiceType Status { get; }

        /// <summary>
        /// Provides a list of in service type values
        /// </summary>
        List<VehicleServiceType> ServiceTypeList { get; }

        /// <summary>
        /// Used to store the index of the currently selected in service type.
        /// </summary>
        int ServiceIndex { get; set; }

        /// <summary>
        /// Update has completed, refresh the view.
        /// </summary>
        void CompleteUpdate();
    }
}