namespace Shap.Common.ViewModel
{
  using System.Collections.ObjectModel;
  using System.Linq;

  using NynaeveLib.ViewModel;

  /// <summary>
  /// Used to count and display locations and a frequency counter.
  /// </summary>
  public class LocationCounterManagerViewModel : ViewModelBase
  {
    /// <summary>
    /// List of locations and counters
    /// </summary>
    private ObservableCollection<LocationCounterViewModel> locations;

    /// <summary>
    /// Initialises a new instance of the <see cref="LocationCounterManagerViewModel"/> class.
    /// </summary>
    public LocationCounterManagerViewModel()
    {
      this.locations = new ObservableCollection<LocationCounterViewModel>();
    }

    /// <summary>
    /// Gets a collection of locations and counters.
    /// </summary>
    public ObservableCollection<LocationCounterViewModel> Locations => this.locations;

    /// <summary>
    /// Add a new count for the location. If not present a new one is created and adde to 
    /// the list. 
    /// </summary>
    /// <param name="newLocation">location to count</param>
    public void AddLocation(string newLocation)
    {
      foreach (LocationCounterViewModel locationCounter in this.Locations)
      {
        if (string.Compare(locationCounter.Location, newLocation) == 0)
        {
          locationCounter.AddOne();
          this.RaisePropertyChangedEvent(nameof(this.Locations));
          return;
        }
      }

      LocationCounterViewModel newLocationCounter =
        new LocationCounterViewModel(
          newLocation);

      newLocationCounter.AddOne();
      this.locations.Add(newLocationCounter);
      this.RaisePropertyChangedEvent(nameof(this.Locations));
    }

    /// <summary>
    /// Sort the locations by count.
    /// </summary>
    public void Sort()
    {
      this.locations =
        new ObservableCollection<LocationCounterViewModel>(
          from i in this.locations orderby i.Count descending select i);
      this.RaisePropertyChangedEvent(nameof(this.Locations));
    }
  }
}
