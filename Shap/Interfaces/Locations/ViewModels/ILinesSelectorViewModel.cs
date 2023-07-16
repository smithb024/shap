namespace Shap.Interfaces.Locations.ViewModels
{
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interface for a view model which supports the lines selector view.
    /// </summary>
    public interface ILinesSelectorViewModel : ISelectorViewModel
    {
        /// <summary>
        /// Gets the collection of locations.
        /// </summary>
        ObservableCollection<ISelectorRowViewModel> Locations { get; }

        /// <summary>
        /// Gets the collection of rows of icons.
        /// </summary>
        ObservableCollection<IMapCellRowViewModel> Icons { get; }
    }
}