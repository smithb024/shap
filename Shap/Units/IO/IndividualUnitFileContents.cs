namespace Shap.Units.IO
{
  using System;
  using System.Collections.Generic;

  using NynaeveLib.Types;

  using Shap.Interfaces.Types;
  using Shap.Types;

  /// <summary>
  /// Contains the key contents of the individual units file.
  /// </summary>
  public class IndividualUnitFileContents
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="IndividualUnitFileContents"/> class.
    /// </summary>
    /// <param name="unitNumber">unit number</param>
    /// <param name="formerNumbers">collection of former numbers</param>
    /// <param name="inService">index of the service, see in service enumeration</param>
    /// <param name="lastCheckDate">last check date</param>
    /// <param name="journeys">journeys collection</param>
    /// <param name="notes">notes pertaining to unit</param>
    public IndividualUnitFileContents(
      string unitNumber,
      MilesChains distance,
      int entriesCount,
      IVehicleNumberType formerNumbers,
      VehicleServiceType inService,
      DateTime lastEntryDate,
      DateTime lastCheckDate,
      List<IJourneyDetailsType> journeys,
      string notes)
    {
      this.UnitNumber = unitNumber;
      this.Distance = distance;
      this.EntriesCount = entriesCount;
      this.FormerNumbers = formerNumbers;
      this.InService = inService;
      this.LastEntryDate = lastEntryDate;
      this.LastCheckDate = lastCheckDate;
      this.Journeys = journeys;
      this.Notes = notes;
    }

    /// <summary>
    /// Gets the unit number.
    /// </summary>
    public string UnitNumber { get; set; }

    /// <summary>
    /// Gets the total distance.
    /// </summary>
    public MilesChains Distance { get; }

    /// <summary>
    /// Gets the number of entries.
    /// </summary>
    public int EntriesCount { get; }

    /// <summary>
    /// Gets the collection of former numbers.
    /// </summary>
    public IVehicleNumberType FormerNumbers { get; }

    /// <summary>
    /// Gets the date of the last entry.
    /// </summary>
    public DateTime LastEntryDate { get; }

    /// <summary>
    /// Gets the last check date.
    /// </summary>
    public DateTime LastCheckDate { get; }

    /// <summary>
    /// Gets the collection of journeys
    /// </summary>
    public List<IJourneyDetailsType> Journeys { get; }

    /// <summary>
    /// Gets the service state of the unit.
    /// </summary>
    public VehicleServiceType InService { get; }

    /// <summary>
    /// Gets the unit notes.
    /// </summary>
    public string Notes { get; }

    /// <summary>
    /// Add a new number to the former numbers collection
    /// </summary>
    /// <param name="number">number to add</param>
    public void AddFormerNumber(int number)
    {
      this.FormerNumbers.AddFormerNumber(number);
    }
  }
}
