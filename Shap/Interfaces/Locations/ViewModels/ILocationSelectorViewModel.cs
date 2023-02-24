namespace Shap.Interfaces.Locations.ViewModels
{
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interface which supports the location selecto view
    /// </summary>
    public interface ILocationSelectorViewModel : ISelectorViewModel
    {
        /// <summary>
        /// Gets the collection of locations.
        /// </summary>
        ObservableCollection<ISelectorRowViewModel> Locations { get; }
    }
}