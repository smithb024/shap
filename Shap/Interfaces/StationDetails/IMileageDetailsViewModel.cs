namespace Shap.Interfaces.StationDetails
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Shap.Types;

    /// <summary>
    /// Interface which supports the mileage details view.
    /// </summary>
    public interface IMileageDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the stn list
        /// </summary>
        ObservableCollection<string> StnList { get; set; }

        /// <summary>
        /// Gets or sets the current stn.
        /// </summary>
        string Stn { get; set; }

        /// <summary>
        /// Gets or sets the routes
        /// </summary>
        ObservableCollection<RouteDetailsType> Routes { get; set; }

        /// <summary>
        /// Refresh all.
        /// </summary>
        ICommand RefreshCmd { get; }
    }
}