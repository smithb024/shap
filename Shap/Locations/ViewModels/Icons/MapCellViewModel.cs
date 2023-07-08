namespace Shap.Locations.ViewModels.Icons
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels.Icons;

    /// <summary>
    /// A view model which supports a cell icon within the lines selector view.
    /// </summary>
    public class MapCellViewModel : ObservableRecipient, IMapCellViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MapCellViewModel"/> class.
        /// </summary>
        /// <param name="isLineLayer"></param>
        /// <param name="isLocationLayer"></param>
        public MapCellViewModel(
            bool isLineLayer,
            bool isLocationLayer) 
        {
            if (isLineLayer)
            {
                this.LineLayer = "/Icons/Map/straight.png";
            }
            else
            {
                this.LineLayer = string.Empty;
            }

            if (isLocationLayer)
            {
                this.LocationLayer = "/Icons/Map/circle.png";
            }
            else
            {
                this.LocationLayer = string.Empty;
            }
        }

        /// <summary>
        /// Gets the path to the line layer icon.
        /// </summary>
        public string LineLayer { get; }

        /// <summary>
        /// Gets the path to the location layer icon.
        /// </summary>
        public string LocationLayer { get; }
    }
}