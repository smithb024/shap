namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using NynaeveLib.Types;
    using Shap.Common;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Common.ViewModel;
    using Shap.Icons.ListViewItems;
    using Shap.Interfaces.Common.ViewModels;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Stats;
    using Shap.Locations.Messages;
    using Shap.Stats;
    using Shap.Types.Enum;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The view model which is used to display a location on a view.
    /// </summary>
    public class LocationViewModel : ObservableRecipient, ILocationViewModel
    {
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// The first example manager.
        /// </summary>
        private IFirstExampleManager firstExampleManager;

        /// <summary>
        /// The list of all known operators.
        /// </summary>
        private List<SingleOperator> operators;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">
        /// IO controller manager object.
        /// </param>
        /// <param name="firstExampleManager">
        /// The first example manager.
        /// </param>
        public LocationViewModel(
            IIoControllers ioControllers,
            IFirstExampleManager firstExampleManager)
        {
            this.ioControllers = ioControllers;
            this.firstExampleManager = firstExampleManager;
            this.ClassCounters = new ObservableCollection<ITravelCounterViewModel>();
            this.YearCounters= new ObservableCollection<ITravelCounterViewModel>();
            this.LocationOperators = new ObservableCollection<IOperatorListItemViewModel>();
            this.Journeys = new ObservableCollection<IJourneyViewModel>();

            OperatorDetails operatorDetails = ioControllers.Operator.Read();
            this.operators = operatorDetails.Operators;

            this.Messenger.Register<DisplayLocationMessage>(
                this,
                (r, message) => this.OnDisplayLocationMessageReceived(message));
        }

        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the code of the location.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the size of the location.
        /// </summary>
        public string Size { get; private set; }

        /// <summary>
        /// Gets the year that the location opened.
        /// </summary>
        public string Opened { get; private set; }

        /// <summary>
        /// Gets the year that the location closed.
        /// </summary>
        public string Closed { get; private set; }

        /// <summary>
        /// Gets the region where the location is located.
        /// </summary>
        public string Region { get; private set; }

        /// <summary>
        /// Gets the type of the location.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets number from.
        /// </summary>
        public int TotalFrom { get; private set; }

        /// <summary>
        /// Gets the number to.
        /// </summary>
        public int TotalTo { get; private set; }

        /// <summary>
        /// Gets path to the location photo.
        /// </summary>
        public string PhotoPath { get; private set; }

        /// <summary>
        /// Gets the counters for all years.
        /// </summary>
        public ObservableCollection<ITravelCounterViewModel> YearCounters { get; }

        /// <summary>
        /// Gets the counters for all classes.
        /// </summary>
        public ObservableCollection<ITravelCounterViewModel> ClassCounters { get; }

        /// <summary>
        /// Collection of all known operators assigned to the current location.
        /// </summary>
        public ObservableCollection<IOperatorListItemViewModel> LocationOperators { get; }

        /// <summary>
        /// Gets the collection of the latest journeys.
        /// </summary>
        public ObservableCollection<IJourneyViewModel> Journeys { get; }

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
                return;
            }

            LocationDetails currentLocation =
                this.ioControllers.Location.Read(
                    message.Location);

            this.Name = currentLocation.Name;
            this.Code = currentLocation.Code;
            this.Size = currentLocation.Size;
            this.Opened = currentLocation.Opened;
            this.Closed = currentLocation.Closed;
            this.Region = currentLocation.County;
            this.TotalFrom = currentLocation.TotalFrom;
            this.TotalTo = currentLocation.TotalTo;

            this.Category =
                $"{currentLocation.Category} - {LocationCategoriesConverter.Convert(currentLocation.Category)}";

            if (currentLocation.Photos == null ||
                currentLocation.Photos.Count < 1 ||
                string.IsNullOrEmpty(currentLocation.Photos[0].Path))
            {
                this.PhotoPath = string.Empty;
            }
            else
            {
                this.PhotoPath =
                        BasePathReader.GetBasePathUri() +
                        StaticResources.locImgPath +
                        currentLocation.Photos[0].Path +
                        ".jpg";

            }

            this.YearCounters.Clear();

            foreach(LocationYear year in currentLocation.Years)
            {
                ITravelCounterViewModel counter =
                    new TravelCounterViewModel(
                        year.Year.ToString(),
                        year.TotalFrom,
                        year.TotalTo);
                this.YearCounters.Add(counter);
            }

            this.ClassCounters.Clear();

            foreach (LocationClass thisClass in currentLocation.Classes)
            {
                ITravelCounterViewModel counter =
                    new TravelCounterViewModel(
                        thisClass.Name,
                        thisClass.TotalFrom,
                        thisClass.TotalTo);
                this.ClassCounters.Add(counter);
            }

            this.LocationOperators.Clear();

            if (currentLocation.Operators.Count > 0)
            {
                foreach (LocationOperator modelOperator in currentLocation.Operators)
                {
                    bool isOperatorActive =
                        this.FindActiveState(
                            modelOperator.Name);
                    IOperatorListItemViewModel viewModel =
                        new OperatorListItemReadOnlyViewModel(
                            modelOperator.Name,
                            isOperatorActive,
                            modelOperator.IsContemporary);

                    this.LocationOperators.Add(viewModel);
                }
            }

            this.Journeys.Clear();

            if (currentLocation.Trips.Count > 0)
            {
                foreach (Trip modelTrip in currentLocation.Trips)
                {
                    DateTime date =
                        new DateTime(
                            modelTrip.Year,
                            modelTrip.Month,
                            modelTrip.Day);
                    MilesChains distance =
                        new MilesChains(
                            modelTrip.Distance);
                    IJourneyViewModel journey =
                        new JourneyViewModel(
                            this.firstExampleManager,
                            string.Empty,
                            modelTrip.Id,
                            modelTrip.From,
                            modelTrip.To,
                            modelTrip.Route,
                            date,
                            distance,
                            modelTrip.Unit1,
                            modelTrip.Unit2,
                            modelTrip.Unit3,
                            modelTrip.Unit4);
                    journey.CalculateStates();

                    this.Journeys.Add(journey);
                }
            }

            this.OnPropertyChanged(nameof(this.Name));
            this.OnPropertyChanged(nameof(this.Code));
            this.OnPropertyChanged(nameof(this.Size));
            this.OnPropertyChanged(nameof(this.Opened));
            this.OnPropertyChanged(nameof(this.Closed));
            this.OnPropertyChanged(nameof(this.Region));
            this.OnPropertyChanged(nameof(this.Category));
            this.OnPropertyChanged(nameof(this.TotalFrom));
            this.OnPropertyChanged(nameof(this.TotalTo));
            this.OnPropertyChanged(nameof(this.PhotoPath));
            this.OnPropertyChanged(nameof(this.YearCounters));
            this.OnPropertyChanged(nameof(this.ClassCounters));
            this.OnPropertyChanged(nameof(this.LocationOperators));
            this.OnPropertyChanged(nameof(this.Journeys));
        }

        /// <summary>
        /// Find the operator with a given name and return its active state.
        /// </summary>
        /// <param name="name">name to find</param>
        /// <returns>is active state</returns>
        private bool FindActiveState(string name)
        {
            foreach (SingleOperator op in this.operators)
            {
                if (string.Compare(op.Name, name) == 0)
                {
                    return op.IsActive;
                }
            }

            return false;
        }
    }
}