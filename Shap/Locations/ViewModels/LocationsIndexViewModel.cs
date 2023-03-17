namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using NynaeveLib.Commands;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Locations.Messages;
    using System.Windows.Input;

    /// <summary>
    /// View model which supports the locations index window.
    /// </summary>
    public class LocationsIndexViewModel : ObservableRecipient, ILocationsIndexViewModel
    {
        /// <summary>
        /// The location manager.
        /// </summary>
        private readonly ILocationManager locationManager;

        /// <summary>
        /// The location analyser.
        /// </summary>
        private readonly ILocationAnalyser locationAnalyser;

        /// <summary>
        /// The navigation window which is currently selected.
        /// </summary>
        private NavigationType selectedNatigation;

        /// <summary>
        /// Indicates whether the locations are being configured.
        /// </summary>
        private bool isConfigurationMode;

        /// <summary>
        /// The location configuration view model.
        /// </summary>
        private ILocationConfigurationViewModel locationConfigurationViewModel;

        /// <summary>
        /// The location view model.
        /// </summary>
        private ILocationViewModel locationViewModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationsIndexViewModel"/> class.
        /// </summary>
        /// <param name="locationManager">The location manager</param>
        /// <param name="locationAnalyser">The location analyser</param>
        /// <param name="ioControllers">The IO Controller manager object</param>
        public LocationsIndexViewModel(
            ILocationManager locationManager,
            ILocationAnalyser locationAnalyser,
            IIoControllers ioControllers)
        {
            this.locationManager = locationManager;
            this.locationAnalyser = locationAnalyser;
            this.isConfigurationMode = false;

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

            IAlphabeticalNavigationViewModel alphabeticalViewModel =
                new AlphabeticalNavigationViewModel();
            ISelectorViewModel locationSelectorViewModel =
              new LocationsSelectorViewModel(
                  this.locationManager);

            this.Navigation = alphabeticalViewModel;
            this.Selector = locationSelectorViewModel;

            this.locationViewModel =
                new LocationViewModel();
            this.locationConfigurationViewModel = 
                new LocationConfigurationViewModel(
                    ioControllers);

            this.Refresh = 
                new RefreshViewModel(
                    locationAnalyser);

            this.Messenger.Register<DisplayLocationMessage>(
                this,
                (r, message) => this.OnDisplayLocationMessageReceived(message));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the locations are being configured.
        /// </summary>
        public bool IsConfigurationMode {
            get => this.isConfigurationMode;
            set
            {
                if (this.isConfigurationMode == value)
                {
                    return;
                }

                this.isConfigurationMode = value;
                this.OnPropertyChanged(nameof(this.IsConfigurationMode));

                ConfigurationModeMessage message =
                    new ConfigurationModeMessage(
                        this.isConfigurationMode);
                this.Messenger.Send(message);

                if (this.LocationDetails != null)
                {
                    if (this.isConfigurationMode)
                    {
                        this.LocationDetails = this.locationConfigurationViewModel;
                    }
                    else
                    {
                        this.LocationDetails = this.locationViewModel;
                    }

                    this.OnPropertyChanged(nameof(this.LocationDetails));
                }
            }
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
        public IDetailsViewModel LocationDetails { get; private set; }

        /// <summary>
        /// Gets the refresh details view model.
        /// </summary>
        public IRefreshViewModel Refresh { get; }

        /// <summary>
        /// Select the alphabetical navigation view.
        /// </summary>
        private void SelectAlphabetical()
        {
            this.selectedNatigation = NavigationType.Alphabetical;

            IAlphabeticalNavigationViewModel alphabeticalViewModel =
                new AlphabeticalNavigationViewModel();
            ISelectorViewModel locationSelectorViewModel =
                new LocationsSelectorViewModel(
                    this.locationManager);

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
                new LocationsSelectorViewModel(
                    this.locationManager);

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
                new LocationsSelectorViewModel(
                    this.locationManager);

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
        /// Load a new location into the view model.
        /// </summary>
        /// <param name="message">
        /// Message requesting that a new location is added.
        /// </param>
        private void OnDisplayLocationMessageReceived(DisplayLocationMessage message)
        {
            if (string.IsNullOrEmpty(message.Location))
            {
                this.LocationDetails = null;
            }
            else
            {
                if (this.isConfigurationMode)
                {
                    this.LocationDetails = this.locationConfigurationViewModel;
                }
                else
                {
                    this.LocationDetails = this.locationViewModel;
                }
            }

            this.OnPropertyChanged(nameof(this.LocationDetails));
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