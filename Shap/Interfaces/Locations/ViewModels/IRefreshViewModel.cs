namespace Shap.Interfaces.Locations.ViewModels
{
    using System.Windows.Input;

    /// <summary>
    /// Interface which supports the refresh locations view.
    /// </summary>
    public interface IRefreshViewModel
    {
        /// <summary>
        /// Gets the command which is used to refresh all locations.
        /// </summary>
        ICommand RefreshAllCommand { get; }

        /// <summary>
        /// Gets the command which is used to refresh the locations which are currently on display.
        /// </summary>
        ICommand RefreshVisibleCommand { get; }
    }
}