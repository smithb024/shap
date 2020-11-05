namespace Shap.Interfaces.Units
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Windows.Input;

  using NynaeveLib.ViewModel;

  using Shap.Types;
  using Shap.Units;

  /// <summary>
  /// Interface for the sub class view model.
  /// </summary>
  public interface ISubClassViewModel
  {
    /// <summary>
    /// Collection of units belonging to this <see cref="ISubClassViewModel"/>
    /// </summary>
    ObservableCollection<IUnitViewModel> Units { get; }

    /// <summary>
    /// Gets the alpha identifier for this <see cref="SubClassViewModel"/>.
    /// </summary>
    string AlphaIdentifier { get; }

    ///// <summary>
    ///// Attach the events.
    ///// </summary>
    ///// <param name="newVehicle"></param>
    //void SetUpEvents(IUnitViewModel newVehicle);
  }
}
