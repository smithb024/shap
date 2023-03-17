namespace Shap.Locations.ViewModels
{
    using NynaeveLib.Commands;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// View model which supports the refresh view.
    /// </summary>
    public class RefreshViewModel : IRefreshViewModel
    {
        /// <summary>
        /// The instance of the <see cref="ILocationAnalyser"/> class.
        /// </summary>
        private ILocationAnalyser locationAnalyser;

        /// <summary>
        /// The instance of the <see cref="ILocationManager"/> class.
        /// </summary>
        private ILocationManager locationManager;

        /// <summary>
        /// Initialise a new instance of the <see cref="RefreshViewModel"/> class.
        /// </summary>
        /// <param name="locationManager">
        /// The instance of the location manager.
        /// </param>
        /// <param name="locationAnalyser">
        /// The instance of the location analyser.
        /// </param>
        public RefreshViewModel(
            ILocationManager locationManager,
            ILocationAnalyser locationAnalyser) 
        {
            this.locationAnalyser = locationAnalyser;
            this.locationManager = locationManager;

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
            List<string> locations = this.locationManager.GetLocations();
            this.locationAnalyser.Analyse(locations);
        }

        /// <summary>
        /// Refresh visible locations.
        /// </summary>
        private void RefreshVisible() 
        {

        }
    }
}