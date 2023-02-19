namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels;
    using System.Windows.Input;

    /// <summary>
    /// View model which supports the locations index window.
    /// </summary>
    public class LocationsIndexViewModel : ObservableRecipient, ILocationsIndexViewModel
    {
        /// <summary>
        /// Initialises a new instance of the 
        /// </summary>
        public LocationsIndexViewModel()
        {
        }

        /// <summary>
        /// Gets the command which is used to dispayed the alphabetical navigation view.
        /// </summary>
        public ICommand AlphabeticalNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the operators navigation view.
        /// </summary>
        public ICommand OperatorsNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the regions navigation view.
        /// </summary>
        public ICommand RegionsNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the lines navigation view.
        /// </summary>
        public ICommand LinesNavigationCommand { get; }

        /// <summary>
        /// Gets the naviation view model.
        /// </summary>
        public INavigationViewModel Navigation { get; }

        /// <summary>
        /// Gets the selector view model.
        /// </summary>
        public ISelectorViewModel Selector { get; }

        /// <summary>
        /// Gets the locations details view model.
        /// </summary>
        public IDetailsViewModel LocationDetails { get; }
    }
}