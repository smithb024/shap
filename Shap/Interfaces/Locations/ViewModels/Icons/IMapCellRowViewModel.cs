namespace Shap.Interfaces.Locations.ViewModels.Icons
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interface for a view model which supports a set of cell icons within the lines 
    /// selector view.
    /// </summary>
    public interface IMapCellRowViewModel
    {
        /// <summary>
        /// Gets the collection of icons.
        /// </summary>
        ObservableCollection<IMapCellViewModel> Icons { get; }
    }
}