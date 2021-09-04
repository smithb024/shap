namespace Shap.StationDetails
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Shap.Common.Commands;
    using Shap.Types;
    using NynaeveLib.Logger;
    using NynaeveLib.Types;
    using NynaeveLib.ViewModel;

    /// <summary>
    /// Used for editing mileage
    /// </summary>
    public partial class EditMileageViewModel : ViewModelBase
    {
        /// <summary>
        /// Contains all the journeys.
        /// TODO, should not be a singleton.
        /// </summary>
        private JourneyIOController journeyController = JourneyIOController.GetInstance();

        /// <summary>
        /// Defines the new from location.
        /// </summary>
        private string newFromLocation;

        /// <summary>
        /// Defines the new to location.
        /// </summary>
        private string newToLocation;

        /// <summary>
        /// Defines the new route.
        /// </summary>
        private string newViaRoute;

        /// <summary>
        /// Defines the new return route.
        /// </summary>
        private string newViaReturnRoute;

        /// <summary>
        /// Defines the new out miles.
        /// </summary>
        private int newOutMiles = 0;

        /// <summary>
        /// Defines the new back miles.
        /// </summary>
        private int newBackMiles = 0;

        /// <summary>
        /// Defines the new out chains.
        /// </summary>
        private int newOutChains = 0;

        /// <summary>
        /// Defines the new back chains.
        /// </summary>
        private int newBackChains = 0;

        /// <summary>
        /// Defines the index of the selected item in the to list.
        /// </summary>
        private int jnyToIndex;

        /// <summary>
        /// All to locations.
        /// </summary>
        private ObservableCollection<string> jnyToList;

        /// <summary>
        /// Defines the index of the selected item in the from list.
        /// </summary>
        private int jnyFromIndex;

        /// <summary>
        /// All from locations.
        /// </summary>
        private ObservableCollection<string> jnyFromList;

        /// <summary>
        /// Defines the index of the selected item in the to list.
        /// </summary>
        private int jnyViaIndex;

        /// <summary>
        /// All routes.
        /// </summary>
        private ObservableCollection<string> jnyViaList;

        /// <summary>
        /// When selecting a route, this defines the index of the out journey.
        /// </summary>
        private int outIndex = 0;

        /// <summary>
        /// When selecting a route, this defines the index of the in journey which corresponds to the 
        /// out journey.
        /// </summary>
        private int inIndex = 0;

        /// <summary>
        /// Gets a value indicating which tab is selected.
        /// </summary>
        private int tabIndex = 0;

        /// <summary>
        /// Gets a value indicating the current mode.
        /// </summary>
        private bool editMode = false;

        /// <summary>
        ///   Creates a new instance of this class
        /// </summary>
        /// <param name="mainForm">main form</param>
        public EditMileageViewModel()
        {
            this.journeyController = JourneyIOController.GetInstance();

            this.InitialiseAddFields();
            this.InitialiseEditFields();

            this.outIndex = 0;
            this.inIndex = 0;

            this.tabIndex = 0;

            this.editMode = false;

            this.CloseWindowCmd = new CommonCommand(this.CloseWindow);
            this.AddNewCmd = new CommonCommand(this.AddNewRoute);
            this.CompleteEditCmd = new CommonCommand(this.CompleteUpdate);
            this.RefreshCmd = new CommonCommand(this.InitialiseEditTab);
            this.SelectForEditCmd = new CommonCommand(this.SelectJourneyToEdit, this.CanJnyBeSelected);
        }

        /// <summary>
        /// Gets or sets the new from location
        /// </summary>
        public string NewFromStn
        {
            get
            {
                return this.newFromLocation;
            }

            set
            {
                this.newFromLocation = value;
                this.RaisePropertyChangedEvent(nameof(this.NewFromStn));
            }
        }

        /// <summary>
        /// Gets or sets the new to location
        /// </summary>
        public string NewToStn
        {
            get
            {
                return this.newToLocation;
            }

            set
            {
                this.newToLocation = value;
                this.RaisePropertyChangedEvent(nameof(this.NewToStn));
            }
        }

        /// <summary>
        /// Gets or sets the new route.
        /// </summary>
        public string NewViaRoute
        {
            get
            {
                return this.newViaRoute;
            }

            set
            {
                this.newViaRoute = value;
                this.newViaReturnRoute = value;
                this.RaisePropertyChangedEvent(nameof(this.NewViaRoute));
                RaisePropertyChangedEvent(nameof(this.NewViaReturnRoute));
            }
        }

        /// <summary>
        /// Gets or sets the new return route
        /// </summary>
        public string NewViaReturnRoute
        {
            get
            {
                return this.newViaReturnRoute;
            }

            set
            {
                this.newViaReturnRoute = value;
            }
        }

        /// <summary>
        /// Gets or sets the new outbound miles
        /// </summary>
        public int NewOutMiles
        {
            get
            {
                return this.newOutMiles;
            }

            set
            {
                this.newOutMiles = value;
                this.newBackMiles = value;
                RaisePropertyChangedEvent(nameof(this.NewBackMiles));
            }
        }

        /// <summary>
        /// Gets or sets the new inbound miles
        /// </summary>
        public int NewBackMiles
        {
            get
            {
                return this.newBackMiles;
            }

            set
            {
                this.newBackMiles = value;
            }
        }

        /// <summary>
        /// Gets or sets the new outbound chains
        /// </summary>
        public int NewOutChains
        {
            get
            {
                return this.newOutChains;
            }

            set
            {
                this.newOutChains = value;
                this.newBackChains = value;
                RaisePropertyChangedEvent(nameof(this.NewBackChains));
            }
        }

        /// <summary>
        /// Gets or sets the new inbound chains
        /// </summary>
        public int NewBackChains
        {
            get
            {
                return this.newBackChains;
            }

            set
            {
                this.newBackChains = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected outbound journey, used during editing.
        /// </summary>
        public int JnyToIndex
        {
            get
            {
                return this.jnyToIndex;
            }

            set
            {
                this.jnyToIndex = value;
                this.ToIndexChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of outbound journies, used during editing.
        /// </summary>
        public ObservableCollection<string> JnyToList
        {
            get
            {
                return this.jnyToList;
            }

            set
            {
                this.jnyToList = value;
                RaisePropertyChangedEvent(nameof(this.JnyToList));
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected inbound journey, used during editing.
        /// </summary>
        public int JnyFromIndex
        {
            get
            {
                return this.jnyFromIndex;
            }

            set
            {
                this.jnyFromIndex = value;

                this.FromIndexChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of inbound journies, used during editing.
        /// </summary>
        public ObservableCollection<string> JnyFromList
        {
            get
            {
                return this.jnyFromList;
            }

            set
            {
                this.jnyFromList = value;
                RaisePropertyChangedEvent(nameof(this.JnyFromList));
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected route, used during editing.
        /// </summary>
        public int JnyViaIndex
        {
            get
            {
                return this.jnyViaIndex;
            }

            set
            {
                this.jnyViaIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of routes, used during editing.
        /// </summary>
        public ObservableCollection<string> JnyViaList
        {
            get
            {
                return this.jnyViaList;
            }

            set
            {
                this.jnyViaList = value;
                RaisePropertyChangedEvent(nameof(this.JnyViaList));
            }
        }

        /// <summary>
        /// Gets or sets the view tab index.
        /// </summary>
        public int TabIndex
        {
            get
            {
                return this.tabIndex;
            }

            set
            {
                this.tabIndex = value;
                RaisePropertyChangedEvent(nameof(this.TabIndex));

                if (value == 0)
                {
                    this.InitialiseAddFields();
                }
                else if (value == 1)
                {
                    this.InitialiseEditTab();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current edit mode. It determine which components to show on the 
        /// edit tab.
        /// </summary>
        public bool EditMode
        {
            get
            {
                return this.editMode;
            }

            set
            {
                this.editMode = value;
                RaisePropertyChangedEvent(nameof(this.EditMode));
            }
        }

        /// <summary>
        /// Add new command.
        /// </summary>
        public ICommand AddNewCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand CloseWindowCmd { get; private set; }

        /// <summary>
        /// Refresh command.
        /// </summary>
        public ICommand RefreshCmd { get; private set; }

        /// <summary>
        /// Complete edit command.
        /// </summary>
        public ICommand CompleteEditCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand SelectForEditCmd { get; private set; }

        /// <summary>
        /// Close the window.
        /// </summary>
        private void CloseWindow()
        {
            this.OnClosingRequest();
        }

        /// <summary>
        /// Initialise all add fields.
        /// </summary>
        private void InitialiseAddFields()
        {
            // Empty all fields
            this.newFromLocation = string.Empty;
            this.newToLocation = string.Empty;
            this.newViaRoute = string.Empty;
            this.newViaReturnRoute = string.Empty;
            this.newOutMiles = 0;
            this.newOutChains = 0;
            this.newBackMiles = 0;
            this.newBackChains = 0;
        }

        /// <summary>
        /// Initialise all edit fields.
        /// </summary>
        private void InitialiseEditFields()
        {
            // Empty all fields
            this.jnyFromList = new ObservableCollection<string>();
            this.jnyToList = new ObservableCollection<string>();
            this.jnyViaList = new ObservableCollection<string>();
            this.jnyFromIndex = 0;
            this.jnyToIndex = -1;
            this.jnyViaIndex = -1;
        }

        /// <summary>
        /// Add a new route to the journey controller and reset all fields.
        /// </summary>
        private void AddNewRoute()
        {
            int nextPairId = (journeyController.GetMileageDetailsLength() / 2) + 1;

            RouteDetailsType newOutRoute =
              new RouteDetailsType(
                this.NewFromStn,
                this.NewToStn,
                this.NewViaRoute,
                nextPairId.ToString(),
                new MilesChains(
                  this.NewOutMiles,
                  this.NewOutChains));

            RouteDetailsType newBackRoute =
              new RouteDetailsType(
                this.NewToStn,
                this.NewFromStn,
                this.NewViaReturnRoute,
                nextPairId.ToString(),
                new MilesChains(
                  this.NewBackMiles,
                  this.NewBackChains));

            this.journeyController.PutRoute(newOutRoute);
            this.journeyController.PutRoute(newBackRoute);

            this.journeyController.SaveMileageDetails();

            this.InitialiseAddFields();
            this.RaiseAddPropertyChangedEvents();
        }

        /// <summary>
        /// InitialiseEditTab, performs all the initialisation tasks for the 
        ///    edit tab.
        ///  1. Get the routes data and populate the outward combobox.
        /// </summary>
        /// <returns>success flag</returns>
        private void InitialiseEditTab()
        {
            string previousValue = string.Empty;
            string fromStn = string.Empty;

            this.EditMode = false;

            this.InitialiseEditFields();
            this.RaiseEditPropertyChangedEvents();

            for (int i = 0; i < journeyController.GetMileageDetailsLength(); i++)
            {
                fromStn = journeyController.GetFromStation(i);
                if (fromStn != previousValue)
                {
                    this.JnyFromList.Add(fromStn);
                }

                previousValue = fromStn;
            }

            this.FromIndexChanged();
        }

        /// <summary>
        ///   loops through all the journeys to find the line matching 
        ///     the out combo boxes. 
        ///   if it finds a match, then it loops through all the journeys
        ///     to find the matching return journey (using the key field).
        ///   if there is no match make a note.
        /// </summary>
        private void SelectJourneyToEdit()
        {
            string keyIndex = string.Empty;
            bool success = false;

            for (int index = 0; index < journeyController.GetMileageDetailsLength(); ++index)
            {
                if (this.ValidRouteSelection(index))
                {
                    outIndex = index;
                    keyIndex = journeyController.GetMileageKey(index);
                    success = true;
                    break;
                }
            }

            if (success)
            {
                for (int index = 0; index < journeyController.GetMileageDetailsLength(); ++index)
                {
                    if (string.Equals(keyIndex, journeyController.GetMileageKey(index))
                      && index != outIndex)
                    {
                        inIndex = index;
                        break;
                    }
                }

                this.newFromLocation = journeyController.GetFromStation(outIndex);
                this.newToLocation = journeyController.GetToStation(outIndex);
                this.newViaRoute = journeyController.GetViaRoute(outIndex);
                this.newOutMiles = journeyController.GetMiles(outIndex);
                this.newOutChains = journeyController.GetChains(outIndex);
                this.newViaReturnRoute = journeyController.GetViaRoute(inIndex);
                this.newBackMiles = journeyController.GetMiles(inIndex);
                this.newBackChains = journeyController.GetChains(inIndex);

                this.EditMode = true;
                this.RaiseAddPropertyChangedEvents();
            }
        }

        /// <summary>
        /// Ensure that a jny can be selected.
        /// </summary>
        /// <returns>true if can be selected</returns>
        private bool CanJnyBeSelected()
        {
            if (this.JnyFromList == null ||
              this.JnyToList == null ||
              this.JnyViaList == null)
            {
                return false;
            }

            // TODO, this doesn't check for the via. So will return true even if a via is expected.
            // This is so those with empty and none empty vias can be selected. Is this right?
            // Should there be a more specific check. To refactor at some later date.

            return
              this.JnyFromList.Count > 0 &&
              this.JnyToList.Count > 0 &&
              this.JnyFromIndex >= 0 && this.JnyFromIndex < this.JnyFromList.Count &&
              this.JnyToIndex >= 0 && this.JnyToIndex < this.JnyToList.Count;
        }

        /// <summary>
        /// The complete update command has been selected. Update the selected outbound and inbound
        /// values with the current values. Save and reinitialise all fields.
        /// </summary>
        private void CompleteUpdate()
        {
            this.journeyController.PutFromStation(this.outIndex, this.NewFromStn);
            this.journeyController.PutToStation(this.outIndex, this.NewToStn);
            this.journeyController.PutViaRoute(this.outIndex, this.NewViaRoute);
            this.journeyController.PutMiles(this.outIndex, this.NewOutMiles);
            this.journeyController.PutChains(this.outIndex, this.NewOutChains);

            this.journeyController.PutFromStation(this.inIndex, this.NewToStn);
            this.journeyController.PutToStation(this.inIndex, this.NewFromStn);
            this.journeyController.PutViaRoute(this.inIndex, this.NewViaReturnRoute);
            this.journeyController.PutMiles(this.inIndex, this.NewBackMiles);
            this.journeyController.PutChains(this.inIndex, this.NewBackChains);

            this.journeyController.SaveMileageDetails();

            this.InitialiseEditTab();
        }

        /// <summary>
        /// Returns a value which indicates whether the from index is valid.
        /// </summary>
        /// <returns></returns>
        private bool IsFromIndexValid()
        {
            return this.JnyFromIndex >= 0 && this.JnyFromIndex < this.JnyFromList.Count;
        }

        /// <summary>
        /// Returns a value which indicates whether the to index is valid.
        /// </summary>
        /// <returns></returns>
        private bool IsToIndexValid()
        {
            return this.JnyToIndex >= 0 && this.JnyToIndex < this.JnyToList.Count;
        }

        /// <summary>
        /// Edit selector from index has changed. Determine the new set of collections to present.
        /// </summary>
        private void FromIndexChanged()
        {
            Logger logger = Logger.Instance;
            if (!this.IsFromIndexValid())
            {
                return;
            }

            logger.WriteLog("TRACE: Edit mileage journeys from Station is " + this.JnyFromList[this.JnyFromIndex]);

            this.jnyToList = new ObservableCollection<string>();
            this.jnyViaList = new ObservableCollection<string>();

            // Create list to contain the new to values.
            List<string> workingToList = new List<string>();

            for (int index = 0; index < journeyController.GetMileageDetailsLength(); index++)
            {
                if (string.Equals(this.JnyFromList[this.JnyFromIndex], journeyController.GetFromStation(index)))
                {
                    logger.WriteLog("TRACE: Get - " + journeyController.GetToStation(index));
                    workingToList.Add(journeyController.GetToStation(index));
                }
            }

            this.AddToList(
              this.jnyToList,
              workingToList);

            this.RaiseEditPropertyChangedEvents();
        }

        /// <summary>
        /// Edit selector to index has changed. Determine the new set of collections to present.
        /// </summary>
        private void ToIndexChanged()
        {
            if (!this.IsToIndexValid())
            {
                return;
            }

            Logger.Instance.WriteLog("TRACE: Edit mileage journeys to Station is " + this.JnyToList[this.JnyToIndex]);

            this.jnyViaList = new ObservableCollection<string>();

            // Create list to contain the new route values.
            List<string> workingViaList = new List<string>();

            for (int index = 0; index < journeyController.GetMileageDetailsLength(); index++)
            {
                if (string.Equals(this.JnyFromList[this.JnyFromIndex], journeyController.GetFromStation(index)) &&
                  string.Equals(this.JnyToList[this.JnyToIndex], journeyController.GetToStation(index)))
                {
                    Logger.Instance.WriteLog("TRACE: Get - " + journeyController.GetViaRoute(index));
                    workingViaList.Add(journeyController.GetViaRoute(index));
                }
            }

            this.AddToList(
              this.jnyViaList,
              workingViaList);

            this.RaiseEditPropertyChangedEvents();
        }

        /// <summary>
        /// Add the <paramref name="workingList"/> to the <paramref name="destinationCollection"/> in
        /// alphabetical order and taking care not to add duplicates.
        /// </summary>
        /// <param name="destinationCollection">destination collection</param>
        /// <param name="workingList">list to add to the destination</param>
        private void AddToList(
          ObservableCollection<string> destinationCollection,
          List<string> workingList)
        {
            string previousValue = string.Empty;

            workingList.Sort();

            foreach (string toString in workingList)
            {
                if (toString != previousValue)
                {
                    Logger.Instance.WriteLog("TRACE: Add - " + toString);
                    destinationCollection.Add(toString);
                }

                previousValue = toString;
            }
        }

        /// <summary>
        /// Check to see if the current set of values matches a jny in the controller object.
        /// </summary>
        /// <param name="jnyControllerIndex">index of jny to check</param>
        /// <returns>flag indicating success</returns>
        private bool ValidRouteSelection(int jnyControllerIndex)
        {
            bool viaCheck =
              this.JnyViaIndex < 0 ?
              string.Equals(string.Empty, journeyController.GetViaRoute(jnyControllerIndex)) :
              string.Equals(this.JnyViaList[this.JnyViaIndex], journeyController.GetViaRoute(jnyControllerIndex));

            return
              string.Equals(this.JnyFromList[this.JnyFromIndex], journeyController.GetFromStation(jnyControllerIndex)) &&
              string.Equals(this.JnyToList[this.JnyToIndex], journeyController.GetToStation(jnyControllerIndex)) &&
              viaCheck;
        }

        /// <summary>
        /// Raise property changed events on all properties.
        /// </summary>
        /// <remarks>
        /// These are used on the add and edit tabs.
        /// </remarks>
        private void RaiseAddPropertyChangedEvents()
        {
            RaisePropertyChangedEvent(nameof(this.NewFromStn));
            RaisePropertyChangedEvent(nameof(this.NewToStn));
            RaisePropertyChangedEvent(nameof(this.NewViaRoute));
            RaisePropertyChangedEvent(nameof(this.NewViaReturnRoute));
            RaisePropertyChangedEvent(nameof(this.NewOutMiles));
            RaisePropertyChangedEvent(nameof(this.NewBackMiles));
            RaisePropertyChangedEvent(nameof(this.NewOutChains));
            RaisePropertyChangedEvent(nameof(this.NewBackChains));
        }

        /// <summary>
        /// Raise property changed events on all properties.
        /// </summary>
        private void RaiseEditPropertyChangedEvents()
        {
            RaisePropertyChangedEvent(nameof(this.JnyFromList));
            RaisePropertyChangedEvent(nameof(this.JnyToList));
            RaisePropertyChangedEvent(nameof(this.JnyViaList));
            RaisePropertyChangedEvent(nameof(this.JnyToIndex));
            RaisePropertyChangedEvent(nameof(this.JnyFromIndex));
            RaisePropertyChangedEvent(nameof(this.JnyViaIndex));
        }
    }
}