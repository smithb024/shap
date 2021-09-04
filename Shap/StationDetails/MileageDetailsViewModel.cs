namespace Shap.StationDetails
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Types;

    /// <summary>
    /// View model which supports the mileage details window.
    /// </summary>
    public class MileageDetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// the journey controller.
        /// </summary>
        private JourneyIOController journeyController;

        /// <summary>
        /// Collection of routes.
        /// </summary>
        private ObservableCollection<RouteDetailsType> routes;

        /// <summary>
        /// Collection of locations
        /// </summary>
        private ObservableCollection<string> stnList;

        /// <summary>
        /// The current location
        /// </summary>
        private string stn;

        /// <summary>
        /// Initialise a new instance of the <see cref="MileageDetailsViewModel"/> class.
        /// </summary>
        public MileageDetailsViewModel()
        {
            this.journeyController = JourneyIOController.Instance;
            routes = new ObservableCollection<RouteDetailsType>();
            this.RefreshCmd = new CommonCommand(CalculateRoutes);

            InitialiseComboBoxPrimary();
        }

        /// <summary>
        /// Gets or sets the stn list
        /// </summary>
        public ObservableCollection<string> StnList
        {
            get
            {
                return this.stnList;
            }
            set
            {
                this.stnList = value;
                this.RaisePropertyChangedEvent("StnList");
            }
        }

        /// <summary>
        /// Gets or sets the current stn.
        /// </summary>
        public string Stn
        {
            get
            {
                return this.stn;
            }
            set
            {
                this.stn = value;
                this.RaisePropertyChangedEvent("Stn");
                this.CalculateRoutes();
            }
        }

        /// <summary>
        /// Gets or sets the routes
        /// </summary>
        public ObservableCollection<RouteDetailsType> Routes
        {
            get
            {
                return this.routes;
            }
            set
            {
                this.routes = value;
                this.RaisePropertyChangedEvent("Routes");
            }
        }

        /// <summary>
        /// Refresh all.
        /// </summary>
        public ICommand RefreshCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// Refresh the stn list.
        /// </summary>
        private void RefreshStnList()
        {
            this.InitialiseComboBoxPrimary();
        }

        /// <summary>
        /// Close the window.
        /// </summary>
        private void CloseWindow()
        {
            this.OnClosingRequest();
        }

        /// <summary>
        ///   Loops through all the from Stations and adds them to the 
        ///     comboBoxPrimary combo box. It only adds one of each station, 
        ///     since they are sorted alphabetically it does this by checking 
        ///     against the previous value.
        /// </summary>
        private void InitialiseComboBoxPrimary()
        {
            this.stnList = new ObservableCollection<string>();
            this.stnList.Add(string.Empty);

            string previousvalue = string.Empty;
            string location = string.Empty;

            for (int i = 0; i < journeyController.GetMileageDetailsLength(); i++)
            {
                location = journeyController.GetFromStation(i);
                if (location != previousvalue)
                {
                    this.stnList.Add(location);
                }

                previousvalue = location;
            }
        }

        /// <summary>
        /// Get all the routes equalling the from stn.
        /// </summary>
        private void CalculateRoutes()
        {
            this.Routes = new ObservableCollection<RouteDetailsType>();

            for (int index = 0; index < journeyController.GetMileageDetailsLength(); ++index)
            {
                if (NynaeveLib.Utils.StringCompare.SimpleCompare(
                  journeyController.GetFromStation(index),
                  this.Stn))
                {
                    this.Routes.Add(journeyController.GetRoute(index));
                }
            }
        }
    }
}