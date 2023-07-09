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
        /// <param name="lineLayerCode">
        /// Indicates which icon to use for the line layer.
        /// </param>
        /// <param name="isLocationLayer"></param>
        public MapCellViewModel(
            string lineLayerCode,
            bool isLocationLayer) 
        {
            switch (lineLayerCode)
            {
                case "a":
                    this.LineLayer = "/Icons/Map/straight.png";
                    break;
                case "b":
                    this.LineLayer = "/Icons/Map/blocks.png";
                    break;
                case "f":
                    this.LineLayer = "/Icons/Map/finish.png";
                    break;
                case "h":
                    this.LineLayer = "/Icons/Map/horizontal.png";
                    break;
                case "i":
                    this.LineLayer = "/Icons/Map/turnOutRight.png";
                    break;
                case "j":
                    this.LineLayer = "/Icons/Map/turnOutLeft.png";
                    break;
                case "k":
                    this.LineLayer = "/Icons/Map/turnInLeft.png";
                    break;
                case "l":
                    this.LineLayer = "/Icons/Map/turnInRight.png";
                    break;
                case "m":
                    this.LineLayer = "/Icons/Map/turnOutBoth.png";
                    break;
                case "n":
                    this.LineLayer = "/Icons/Map/turnInBoth.png";
                    break;
                case "o":
                    this.LineLayer = "/Icons/Map/terminateRight.png";
                    break;
                case "p":
                    this.LineLayer = "/Icons/Map/terminateLeft.png";
                    break;
                case "s":
                    this.LineLayer = "/Icons/Map/start.png";
                    break;
                case "t":
                    this.LineLayer = "/Icons/Map/terminate.png";
                    break;
                case "u":
                    this.LineLayer = "/Icons/Map/bridgeOver.png";
                    break;
                case "v":
                    this.LineLayer = "/Icons/Map/bridgeUnder.png";
                    break;
                case "w":
                    this.LineLayer = "/Icons/Map/goRight.png";
                    break;
                case "x":
                    this.LineLayer = "/Icons/Map/goLeft.png";
                    break;
                case "y":
                    this.LineLayer = "/Icons/Map/fromLeft.png";
                    break;
                case "z":
                    this.LineLayer = "/Icons/Map/fromRight.png";
                    break;
                default:
                    this.LineLayer = string.Empty;
                    break;
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