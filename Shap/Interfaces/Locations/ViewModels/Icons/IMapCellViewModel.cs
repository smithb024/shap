namespace Shap.Interfaces.Locations.ViewModels.Icons
{
    /// <summary>
    /// Interface for a view model which supports a cell icon within the lines selector view.
    /// </summary>
    public interface IMapCellViewModel
    {
        /// <summary>
        /// Gets the path to the line layer icon.
        /// </summary>
        string LineLayer { get; }

        /// <summary>
        /// Gets the path to the location layer icon.
        /// </summary>
        string LocationLayer { get; }
    }
}