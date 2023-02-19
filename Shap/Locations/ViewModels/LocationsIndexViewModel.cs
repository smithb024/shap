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

            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();
            IAlphabeticalNavigationViewModel alphabeticalViewModel =
                new AlphabeticalNavigationViewModel();

            this.Selector = locationSelectorViewModel;
            this.Navigation = alphabeticalViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectOperators()
        {
            this.selectedNatigation = NavigationType.Operators;

            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();
            IDirectNavigationViewModel operatorsViewModel =
                new DirectNavigationViewModel();

            this.Selector = locationSelectorViewModel;
            this.Navigation = operatorsViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectRegions()
        {
            this.selectedNatigation = NavigationType.Regions;

            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel();
            IDirectNavigationViewModel regionsViewModel =
                new DirectNavigationViewModel();

            this.Selector = locationSelectorViewModel;
            this.Navigation = regionsViewModel;

            this.OnPropertyChanged(nameof(Navigation));
            this.OnPropertyChanged(nameof(Selector));
        }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectLines()
        {
            this.selectedNatigation = NavigationType.Lines;

            ILinesSelectorViewModel linesSelectorViewModel =
                new LinesSelectorViewModel();
            IDirectNavigationViewModel linesViewModel =
                new DirectNavigationViewModel();

            this.Selector = linesSelectorViewModel;
            this.Navigation = linesViewModel;

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