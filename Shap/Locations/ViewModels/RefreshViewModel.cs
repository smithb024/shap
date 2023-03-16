namespace Shap.Locations.ViewModels
{
    using NynaeveLib.Commands;
    using Shap.Interfaces.Locations.ViewModels;
    using System.Windows.Input;

    /// <summary>
    /// View model which supports the refresh view.
    /// </summary>
    public class RefreshViewModel : IRefreshViewModel
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="RefreshViewModel"/> class.
        /// </summary>
        public RefreshViewModel() 
        {
            this.RefreshAllCommand =
                    new CommonCommand(
                        this.RefreshAll);
            this.RefreshVisibleCommand =
                    new CommonCommand(
                        this.RefreshVisible);
        }

        /// <summary>
        /// Gets the command which is used to refresh all locations.
        /// </summary>
        public ICommand RefreshAllCommand { get; }

        /// <summary>
        /// Gets the command which is used to refresh the locations which are currently on display.
        /// </summary>
        public ICommand RefreshVisibleCommand { get; }

        /// <summary>
        /// Refresh all locations.
        /// </summary>
        private void RefreshAll()
        {

        }

        /// <summary>
        /// Refresh visible locations.
        /// </summary>
        private void RefreshVisible() 
        {

        }
    }
}