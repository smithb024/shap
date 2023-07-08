namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Enums;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.Messages;
    using Shap.Locations.ViewModels.Icons;
    using Shap.Messages;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// View model which supports the locations selector view.
    /// </summary>
    /// <remarks>
    /// This view allows locations to be chosen for display on the locations view.
    /// This view contains a list of locations which are grouped according to a chosen filter 
    /// (alphabetical/geographical/operator). It is used to select the location for display on
    /// the main/configuration part of the window.
    /// </remarks>
    public class LocationsSelectorViewModel : ObservableRecipient, ILocationSelectorViewModel
    {
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// The location manager.
        /// </summary>
        private readonly ILocationManager locationManager;

        /// <summary>
        /// The location analyser.
        /// </summary>
        private readonly ILocationAnalyser locationAnalyser;

        /// <summary>
        /// Indicates how the locations are displayed.
        /// </summary>
        private SelectorType type;

        /// <summary>
        /// The criteria by which the locations are selected.
        /// </summary>
        private string searchCriteria;

        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsSelectorViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">
        /// IO controller manager object.
        /// </param>
        /// <param name="locationManager">
        /// The instance of the location manager.
        /// </param>
        /// <param name="locationAnalyser">
        /// The instance of the location analyser.
        /// </param>
        public LocationsSelectorViewModel(
            IIoControllers ioControllers,
            ILocationManager locationManager,
            ILocationAnalyser locationAnalyser)
        {
            this.Locations = new ObservableCollection<ISelectorRowViewModel>();
            this.ioControllers = ioControllers;
            this.locationManager = locationManager;
            this.locationAnalyser = locationAnalyser;

            this.Messenger.Register<NewLocationAddedMessage>(
                this,
                (r, message) => this.OnLocationAddedMessageReceived(message));
            this.Messenger.Register<AlphaSelectorMessage>(
                this,
                (r, message) => this.NewAlphaCharacterSelected(message));
            this.Messenger.Register<OperatorSelectorMessage>(
                this,
                (r, message) => this.NewOperatorSelected(message));
            this.Messenger.Register<RegionSelectorMessage>(
                this,
                (r, message) => this.NewRegionSelected(message));
            this.Messenger.Register<RequestLocationsRefreshMessage>(
                this,
                (r, message) => this.OnRequestLocationsRefreshMessage(message));

        }

        /// <summary>
        /// Gets the collection of locations.
        /// </summary>
        public ObservableCollection<ISelectorRowViewModel> Locations { get; }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        /// <param name="disposing">Is the class being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        /// A new location has been added to the database, reinitialise this view model.
        /// </summary>
        /// <param name="message">
        /// Message indicating that a new location has been added.
        /// </param>
        private void OnLocationAddedMessageReceived(NewLocationAddedMessage message)
        {
        }

        /// <summary>
        /// Display all locations starting with the indicated string.
        /// </summary>
        /// <param name="message">
        /// The <see cref="AlphaSelectorMessage"/> message.
        /// </param>
        public void NewAlphaCharacterSelected(AlphaSelectorMessage message)
        {
            this.type = SelectorType.Alphabetical;
            this.searchCriteria = message.Character;
            this.RebuildLocationsList();
        }

        /// <summary>
        /// Display all locations managed by the the indicated operator.
        /// </summary>
        /// <param name="message">
        /// The <see cref="OperatorSelectorMessage"/> message.
        /// </param>
        public void NewOperatorSelected(OperatorSelectorMessage message)
        {
            this.type = SelectorType.Operator;
            this.searchCriteria = message.Operator;
            this.RebuildLocationsList();
        }

        /// <summary>
        /// Display all locations in the indicated region.
        /// </summary>
        /// <param name="message">
        /// The <see cref="RegionSelectorMessage"/> message.
        /// </param>
        public void NewRegionSelected(RegionSelectorMessage message)
        {
            this.type = SelectorType.Region;
            this.searchCriteria = message.Region;
            this.RebuildLocationsList();
        }

        /// <summary>
        /// Request that all of the currently displayed locations are refreshed.
        /// </summary>
        /// <param name="message">
        /// The <see cref="RequestLocationsRefreshMessage"/> message.
        /// </param>
        private void OnRequestLocationsRefreshMessage(RequestLocationsRefreshMessage message)
        {
            List<string> locations = new List<string>();

            foreach (ISelectorRowViewModel selector in this.Locations)
            {
                locations.Add(selector.Name);
            }

            this.locationAnalyser.Analyse(locations);
        }

        /// <summary>
        /// Rebuild the locations list based on the current search criteria.
        /// </summary>
        private void RebuildLocationsList()
        {
            List<string> allLocations = this.locationManager.GetLocations();
            this.Locations.Clear();

            switch (this.type)
            {
                case SelectorType.Alphabetical:
                    {
                        foreach (string location in allLocations)
                        {
                            if (string.Equals(location.Substring(0, 1), this.searchCriteria))
                            {
                                ISelectorRowViewModel row =
                                    new SelectorRowViewModel(
                                        this.ioControllers,
                                        location);
                                this.Locations.Add(row);
                            }
                        }

                        break;
                    }

                case SelectorType.Operator:
                    {
                        foreach (string location in allLocations)
                        {
                            LocationDetails details =
                                this.ioControllers.Location.Read(
                                    location);

                            foreach (LocationOperator locationOperator in details.Operators)
                            {
                                if (string.Equals(locationOperator.Name, this.searchCriteria))
                                {
                                    ISelectorRowViewModel row =
                                        new SelectorRowViewModel(
                                            this.ioControllers,
                                            location);
                                    this.Locations.Add(row);
                                    break;
                                }
                            }
                        }

                        break;
                    }

                case SelectorType.Region:
                    {
                        foreach (string location in allLocations)
                        {
                            LocationDetails details =
                                this.ioControllers.Location.Read(
                                    location);

                            if (string.Equals(details.County, this.searchCriteria))
                            {
                                ISelectorRowViewModel row =
                                    new SelectorRowViewModel(
                                        this.ioControllers,
                                        location);
                                this.Locations.Add(row);
                            }
                        }

                        break;
                    }
            }

            this.OnPropertyChanged(nameof(this.Locations));
        }
    }
}