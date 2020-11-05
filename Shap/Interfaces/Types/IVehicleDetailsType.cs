namespace Shap.Interfaces.Types
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using NynaeveLib.Types;
  using NynaeveLib.ViewModel;

  using Shap.Common.ViewModel;
  using Shap.Interfaces.ViewModels;
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

    ///// <summary>
    /////   set mileage
    ///// </summary>
    ///// <param name="miles">distance miles</param>
    ///// <param name="chains">distance chains</param>
    //void SetMileage(string miles, string chains);

    ///// <summary>
    ///// set number of entries
    ///// </summary>
    ///// <param name="numberEntries">number of entries</param>
    //void SetNumberEntries(int numberEntries);

    ///// <summary>
    ///// add former number
    ///// </summary>
    ///// <param name="formerNumber">former number</param>
    //void AddFormerNumber(int formerNumber);

    ///// <summary>
    ///// set last check date
    ///// </summary>
    ///// <param name="year">new year</param>
    ///// <param name="month">new month</param>
    ///// <param name="day">new day</param>
    //void SetLastCheckDate(int year, int month, int day);

    ///// <summary>
    ///// add journey
    ///// </summary>
    ///// <param name="date">journey date</param>
    ///// <param name="journeyNumber">journey number</param>
    ///// <param name="from">journey origin</param>
    ///// <param name="to">journey destination</param>
    ///// <param name="via">journey route</param>
    ///// <param name="distance">journey distance</param>
    ///// <param name="totalVehicles">total vehicles</param>
    ///// <param name="firstNumber">first vehicle</param>
    ///// <param name="secondNumber">second vehicle</param>
    ///// <param name="thirdNumber">third vehicle</param>
    ///// <param name="fourthNumber">fourth vehicle</param>
    //void AddJourney(
    //  DateTime date,
    //  string journeyNumber,
    //  string from,
    //  string to,
    //  string via,
    //  MilesChains distance,
    //  int totalVehicles,
    //  string firstNumber,
    //  string secondNumber,
    //  string thirdNumber,
    //  string fourthNumber);

    ///// <summary>
    ///// add journey
    ///// </summary>
    ///// <param name="newJourney">new journey</param>
    //void AddJourney(IJourneyDetailsType newJourney);

    ///// <summary>
    ///// Clear the journeys list
    ///// </summary>
    //void ClearJourneysList();

    ///// <summary>
    ///// Indicates that tall the data has been loaded into this <see cref="VehicleDetailsType"/>.
    ///// </summary>
    //void LoadComplete();

    ///// <summary>
    ///// get number of entries
    ///// </summary>
    ///// <returns>number of entries</returns>
    //int GetNumberEntries();

    ///// <summary>
    ///// get the size of the journeys list.
    ///// </summary>
    ///// <returns>journeys count</returns>
    //int GetJourneysCount();

    /// <summary>
    /// Update has completed, refresh the view.
    /// </summary>
    void CompleteUpdate();
  }
}
