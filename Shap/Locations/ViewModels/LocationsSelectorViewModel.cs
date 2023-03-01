namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
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
        /// A list of all locations which are available.
        /// </summary>
        private List<string> locations;

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
        public LocationsSelectorViewModel()
        {
            this.Locations = new ObservableCollection<ISelectorRowViewModel>();

            ISelectorRowViewModel tempRow1 = new SelectorRowViewModel("Row 1");
            ISelectorRowViewModel tempRow2 = new SelectorRowViewModel("Row 2");
            ISelectorRowViewModel tempRow3 = new SelectorRowViewModel("Row 3");
            this.Locations.Add(tempRow1);
            this.Locations.Add(tempRow2);
            this.Locations.Add(tempRow3);

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
        }

        ///// <summary>
        /////   Loops through all the from Stations and adds them to the 
        /////     comboBoxPrimary combo box. It only adds one of each station, 
        /////     since they are sorted alphabetically it does this by checking 
        /////     against the previous value.
        ///// </summary>
        //private void InitialiseComboBoxPrimary()
        //{
        //    this.stnList.Clear();
        //    this.stnList.Add(string.Empty);

        //    // Get all locations from the model.
        //    int routesCount = journeyController.GetMileageDetailsLength();
        //    List<string> locations = new List<string>();

        //    for (int i = 0; i < routesCount; i++)
        //    {
        //        locations.Add(journeyController.GetFromStation(i));
        //    }

        //    // Ensure the locations are in alphabetical order.
        //    locations.Sort();

        //    // Filter out duplicates.
        //    string previousvalue = string.Empty;
        //    foreach (string location in locations)
        //    {
        //        if (location != previousvalue)
        //        {
        //            this.stnList.Add(location);
        //        }

        //        previousvalue = location;
        //    }
        //}

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