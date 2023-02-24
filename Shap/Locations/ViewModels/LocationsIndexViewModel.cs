namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using NynaeveLib.Commands;
    using Shap.Interfaces.Locations.ViewModels;
    using System.Windows.Input;

    /// <summary>
    /// View model which supports the locations index window.
    /// </summary>
    public class LocationsIndexViewModel : ObservableRecipient, ILocationsIndexViewModel
    {
        /// <summary>
        /// The navigation window which is currently selected.
        /// </summary>
        private NavigationType selectedNatigation;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationsIndexViewModel"/> class.
        /// </summary>
        public LocationsIndexViewModel()
        {
            this.selectedNatigation = NavigationType.Alphabetical;
            this.AlphabeticalNavigationCommand =
                new CommonCommand(
                    this.SelectAlphabetical,
                    this.CanSelectAlphabetical);
            this.OperatorsNavigationCommand =
                new CommonCommand(
                    this.SelectOperators,
                    this.CanSelectOperators);
            this.RegionsNavigationCommand =
                new CommonCommand(
                    this.SelectRegions,
                    this.CanSelectRegions);
            this.LinesNavigationCommand =
                new CommonCommand(
                    this.SelectLines,
                    this.CanSelectLines);

            ISelectorViewModel locationSelectorViewModel =
              new LocationsSelectorViewModel();
            IAlphabeticalNavigationViewModel alphabeticalViewModel =
                new AlphabeticalNavigationViewModel();

            this.Selector = locationSelectorViewModel;
            this.Navigation = alphabeticalViewModel;
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
        public INavigationViewModel Navigation { get; private set; }

        /// <summary>
        /// Gets the selector view model.
        /// </summary>
        public ISelectorViewModel Selector { get; private set; }

        /// <summary>
        /// Gets the locations details view model.
        /// </summary>
        public IDetailsViewModel LocationDetails { get; }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectAlphabetical()
        {
            this.selectedNatigation = NavigationType.Alphabetical;

            IAlphabeticalNavigationViewModel alphabeticalViewModel =
                new AlphabeticalNavigationViewModel();
            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();

            this.Navigation?.Dispose();
            this.Selector?.Dispose();

            this.Navigation = alphabeticalViewModel;
            this.Selector = locationSelectorViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectOperators()
        {
            this.selectedNatigation = NavigationType.Operators;

            IDirectNavigationViewModel operatorsViewModel =
                new DirectNavigationViewModel();
            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();

            this.Navigation?.Dispose();
            this.Selector?.Dispose();

            this.Navigation = operatorsViewModel;
            this.Selector = locationSelectorViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectRegions()
        {
            this.selectedNatigation = NavigationType.Regions;

            IDirectNavigationViewModel regionsViewModel =
                new DirectNavigationViewModel();
            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();

            this.Navigation = regionsViewModel;
            this.Selector = locationSelectorViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectLines()
        {
            this.selectedNatigation = NavigationType.Lines;

            IDirectNavigationViewModel linesViewModel =
                new DirectNavigationViewModel();
            ILinesSelectorViewModel linesSelectorViewModel =
                new LinesSelectorViewModel();

            this.Navigation?.Dispose();
            this.Selector?.Dispose();

            this.Navigation = linesViewModel;
            this.Selector = linesSelectorViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Indicates whether the alphabetical navigation view can be selected.
        /// </summary>
        /// <returns></returns>
        private bool CanSelectAlphabetical()
        {
            return this.selectedNatigation != NavigationType.Alphabetical;
        }

        /// <summary>
        /// Indicates whether the operators navigation view can be selected.
        /// </summary>
        /// <returns></returns>
        private bool CanSelectOperators()
        {
            return this.selectedNatigation != NavigationType.Operators;
        }

        /// <summary>
        /// Indicates whether the regions navigation view can be selected.
        /// </summary>
        /// <returns></returns>
        private bool CanSelectRegions()
        {
            return this.selectedNatigation != NavigationType.Regions;
        }

        /// <summary>
        /// Indicates whether the lines navigation view can be selected.
        /// </summary>
        /// <returns></returns>
        private bool CanSelectLines()
        {
            return this.selectedNatigation != NavigationType.Lines;
        }

        /// <summary>
        /// Enumeration type used to record which naviation type has been selected.
        /// </summary>
        private enum NavigationType
        {
            Alphabetical,
            Operators,
            Regions,
            Lines
        }
    }
}