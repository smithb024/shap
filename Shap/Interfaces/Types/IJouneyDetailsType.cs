namespace Shap.Interfaces.Types
{
  using System;
  using System.Collections.Generic;
  using NynaeveLib.Types;

  /// <summary>
  /// Interface describing single journey.
  /// </summary>
  /// <remarks>
  /// Formally used as a view model, that job has been taken on by IJourneyViewModel.
  /// </remarks>
  public interface IJourneyDetailsType
  {
    /// <summary>
    /// Gets the Jny Id.
    /// </summary>
    IJnyId JnyId { get; }

    /// <summary>
    /// Gets the journey from details.
    /// </summary>
    string From { get; }

    /// <summary>
    /// Gets the journey to details.
    /// </summary>
    string To { get; }

    /// <summary>
    /// Gets the journey via details.
    /// </summary>
    string Via { get; }

    /// <summary>
    /// Gets the journey distance details.
    /// </summary>
    MilesChains Distance { get; }

    /// <summary>
    /// Gets all unit identifications in the journey.
    /// </summary>
    List<string> Units { get; }

    /// <summary>
    /// Gets the journey date.
    /// </summary>
    DateTime Date { get; }

    /// <summary>
    /// Gets the journey date.
    /// </summary>
    string JourneyDateString { get; }

    /// <summary>
    ///   return string
    /// </summary>
    /// <returns>string object</returns>
    string ToString();

    /// <summary>
    ///   return string
    /// </summary>
    /// <returns>string object</returns>
    string ToFullString();

    /// <summary>
    /// Gets the number of the cuurent unit, this is a purely HCI property and is used to identify
    /// the unit number when the <see cref="IJourneyDetailsType"/> is being used for a specific 
    /// unit.
    /// </summary>
    string ParentUnitNumber { get; set; }
  }
}