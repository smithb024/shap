namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.Messages;
    using Shap.Locations.ViewModels.Icons;
    using Shap.Messages;
    using Shap.StationDetails;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// View model which supports the locations selector view.
    /// </summary>
    /// <remarks>
    /// This view allows locations to be chosen for display on the locations view.
    /// </remarks>
    public class LocationsSelectorViewModel : ObservableRecipient, ILocationSelectorViewModel
    {
        /// <summary>
        /// The location manager.
        /// </summary>
        private readonly ILocationManager locationManager;

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
        /// <param name="locationManager">The location manager</param>
        public LocationsSelectorViewModel(
            ILocationManager locationManager)
        {
            this.Locations = new ObservableCollection<ISelectorRowViewModel>();
            this.locationManager = locationManager;


            this.Messenger.Register<NewLocationAddedMessage>(
                this, 
                (r, message) => this.OnLocationAddedMessageReceived(message));
            this.Messenger.Register<AlphaSelectorMessage>(
                this,
                (r, message) => this.NewAlphaCharacterSelected(message));
            this.Messenger.Register<OperatorSelectorMessage>(
                this,
                (r, message) => this.NewOperatorCharacterSelected(message));
            this.Messenger.Register<RegionSelectorMessage>(
                this,
                (r, message) => this.NewRegionCharacterSelected(message));
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
        public void NewOperatorCharacterSelected(OperatorSelectorMessage message)
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
        public void NewRegionCharacterSelected(RegionSelectorMessage message)
        {
            this.type = SelectorType.Region;
            this.searchCriteria = message.Region;
            this.RebuildLocationsList();
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
                            if (string.Equals(location.Substring(0,1), this.searchCriteria))
                            {
                                ISelectorRowViewModel row =
                                    new SelectorRowViewModel(
                                        location);
                                this.Locations.Add(row);
                            }
                        }
                        break;
                    }
                case SelectorType.Operator:
                    {
                        break;
                    }

                case SelectorType.Region:
                    {
                        break;
                    }
            }

            this.OnPropertyChanged(nameof(this.Locations));
        }

        /// <summary>
        /// Enumeration which defines how the locations are displayed.
        /// </summary>
        private enum SelectorType
        {
            Alphabetical,
            Operator,
            Region
        }
    }
}