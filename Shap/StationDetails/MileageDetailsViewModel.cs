namespace Shap.StationDetails
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Interfaces.StationDetails;
    using Messages;
    using Shap.Common.Commands;
    using Shap.Types;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

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
            this.RefreshCmd = new CommonCommand(this.Refresh);

            this.stnList = new ObservableCollection<string>();
            this.InitialiseComboBoxPrimary();

            this.Messenger.Register<NewLocationAddedMessage>(this, (r, message) => this.OnLocationAddedMessageReceived(message));
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

                FeedbackMessage message =
                    new FeedbackMessage(
                        FeedbackType.Info,
                        $"DistanceDetails - Show routes for {value}.");
                NynaeveMessenger.Default.Send(message);

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
        /// Refresh the location list.
        /// </summary>
        private void RefreshLocationList()
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
            this.stnList.Clear();
            this.stnList.Add(string.Empty);

            // Get all locations from the model.
            int routesCount = journeyController.GetMileageDetailsLength();
            List<string> locations = new List<string>();

            for (int i = 0; i < routesCount; i++)
            {
                locations.Add(journeyController.GetFromStation(i));
            }

            // Ensure the locations are in alphabetical order.
            locations.Sort();

            // Filter out duplicates.
            string previousvalue = string.Empty;
            foreach (string location in locations)
            {
                if (location != previousvalue)
                {
                    this.stnList.Add(location);
                }

                previousvalue = location;
            }
        }

        /// <summary>
        /// Refresh the routes list.
        /// </summary>
        private void Refresh()
        {
            FeedbackMessage message =
                new FeedbackMessage(
                    FeedbackType.Command,
                    $"DistanceDetails - Refresh.");
            NynaeveMessenger.Default.Send(message);

            this.CalculateRoutes();
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

        /// <summary>
        /// A new location has been added to the database, reinitialise this viewmodel
        /// </summary>
        /// <param name="message">
        /// Message indicating that a new location has been added.
        /// </param>
        private void OnLocationAddedMessageReceived(NewLocationAddedMessage message)
        {
            this.RefreshLocationList();
        }
    }
}