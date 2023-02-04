namespace Shap.StationDetails
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Interfaces.StationDetails;
    using Shap.Common.Commands;
    using Shap.Types;

    /// <summary>
    /// View model which supports the mileage details window.
    /// </summary>
    public class MileageDetailsViewModel : ObservableRecipient, IMileageDetailsViewModel
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
            this.routes = new ObservableCollection<RouteDetailsType>();
            this.RefreshCmd = new CommonCommand(CalculateRoutes);

            InitialiseComboBoxPrimary();
        }

        /// <summary>
        /// Gets or sets the stn list
        /// </summary>
        public ObservableCollection<string> StnList
        {
            get => this.stnList;

            set
            {
                if (this.stnList == value)
                {
                    return;
                }

                this.stnList = value;
                this.OnPropertyChanged(nameof(this.StnList));
            }
        }

        /// <summary>
        /// Gets or sets the current stn.
        /// </summary>
        public string Stn
        {
            get => this.stn;

            set
            {
                if (this.stn == value)
                {
                    return;
                }

                this.stn = value;
                this.OnPropertyChanged(nameof(this.Stn));
                this.CalculateRoutes();
            }
        }

        /// <summary>
        /// Gets or sets the routes
        /// </summary>
        public ObservableCollection<RouteDetailsType> Routes
        {
            get => this.routes;

            set
            {
                if (this.routes == value)
                {
                    return;
                }

                this.routes = value;
                this.OnPropertyChanged(nameof(this.Routes));
            }
        }

        /// <summary>
        /// Refresh all.
        /// </summary>
        public ICommand RefreshCmd { get; private set; }

        /// <summary>
        /// Refresh the stn list.
        /// </summary>
        private void RefreshStnList()
        {
            this.InitialiseComboBoxPrimary();
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