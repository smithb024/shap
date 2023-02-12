namespace Shap.Input
{ //1601, 1079, 943, 1221, 860
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Factories;

    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Interfaces.Types;
    using Shap.Interfaces.ViewModels;
    using Shap.StationDetails;
    using Shap.Stats;
    using Shap.Types;
    using Shap.Types.Factories;
    using Shap.Types.ViewModels;
    using NynaeveLib.Logger;
    using NynaeveLib.Types;

    /// <summary>
    /// Main form for inputting data.
    /// </summary>
    public class InputFormViewModel : ViewModelBase
    {
        /// <summary>
        /// Factory to handle file IO for this view model.
        /// </summary>
        //private IDailyInputFactory mainFactory;
        private JourneyIOController journeyController = JourneyIOController.GetInstance();

        /// <summary>
        /// Manages the collection of all first examples.
        /// </summary>
        private FirstExampleManager firstExamples;

        private string currentYear = "1970";
        private string lastErrorString = string.Empty;

        private DateTime date;
        private int jnyNumber;
        private int numberOfVehicles;
        private string firstVehicle;
        private string secondVehicle;
        private string thirdVehicle;
        private string fourthVehicle;
        private int jnyToIndex;
        private ObservableCollection<string> jnyToList;
        private int jnyFromIndex;
        private ObservableCollection<string> jnyFromList;
        private int jnyViaIndex;
        private ObservableCollection<string> jnyViaList;
        private string status;
        private ObservableCollection<DayViewModel> days;

        /// <summary>
        /// Creates a new instance of this form.
        /// </summary>
        /// <param name="mainForm">pare11nt form</param>
        public InputFormViewModel(FirstExampleManager firstExamples)
        {
            //this.mainFactory = mainFactory;

            currentYear = DateTime.Now.Year.ToString();
            this.date = DateTime.Now;

            this.firstExamples = firstExamples;
            firstExamples.LoadAnnualList(currentYear);

            this.PreviousDayCmd = new CommonCommand(this.PreviousDay);
            this.NextDayCmd = new CommonCommand(this.NextDay);
            this.AddJnyCmd = new CommonCommand(this.AddNewJny, this.VclePropertiesValid);
            this.SaveCmd = new CommonCommand(this.SaveDay, this.CanSave);
            this.CloseCmd = new CommonCommand(this.CloseWindow);

            this.JnyList = new ObservableCollection<IJourneyViewModel>();

            this.InitialiseInputForm();

            firstVehicle = string.Empty;
            secondVehicle = string.Empty;
            thirdVehicle = string.Empty;
            fourthVehicle = string.Empty;

            this.SetupDays();
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <remarks>
        /// The set calls a method to update the form on a new date.
        /// </remarks>
        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.SetNewDate(value);
            }
        }

        /// <summary>
        /// Gets the current day.
        /// </summary>
        public int Day => this.date.Day;

        /// <summary>
        /// Gets the current month.
        /// </summary>
        public int Month => this.date.Month;

        /// <summary>
        /// Gets the current year.
        /// </summary>
        public int Year => this.date.Year;

        /// <summary>
        /// Get the total distance for the day.
        /// </summary>
        public MilesChains DayDistance
        {
            get
            {
                MilesChains totalDistance = new MilesChains();

                foreach (IJourneyViewModel jny in this.JnyList)
                {
                    totalDistance += jny.Distance;
                }

                return totalDistance;
            }
        }

        /// <summary>
        /// Get the total distance for the day.
        /// </summary>
        public string DayDistanceString => this.DayDistance.ToString();

        /// <summary>
        /// Gets the Jny Distance.
        /// </summary>
        public MilesChains JnyDistance
        {
            get
            {
                string via =
                  this.JnyViaIsValid() ?
                  this.JnyViaList[this.JnyViaIndex] :
                    string.Empty;

                if (this.JnyToIsValid() &&
                  this.JnyFromIsValid())
                {
                    MilesChains test = journeyController.GetDistance(
                      this.JnyFromList[this.JnyFromIndex],
                      this.JnyToList[this.JnyToIndex],
                      via);
                    return journeyController.GetDistance(
                      this.JnyFromList[this.JnyFromIndex],
                      this.JnyToList[this.JnyToIndex],
                      via);
                }

                return new MilesChains();
            }
        }

        /// <summary>
        /// Gets or sets the jny number.
        /// </summary>
        public int JnyNumber
        {
            get
            {
                return this.jnyNumber;
            }

            set
            {
                this.jnyNumber = value;
                RaisePropertyChangedEvent(nameof(this.JnyNumber));
            }
        }

        /// <summary>
        /// Gets the number of vehicles.
        /// </summary>
        public int NumberOfVehicles =>
          (string.IsNullOrEmpty(this.FirstVehicle) ? 0 : 1) +
            (string.IsNullOrEmpty(this.SecondVehicle) ? 0 : 1) +
            (string.IsNullOrEmpty(this.ThirdVehicle) ? 0 : 1) +
            (string.IsNullOrEmpty(this.FourthVehicle) ? 0 : 1);

        /// <summary>
        /// Gets or sets the first vehicle.
        /// </summary>
        public string FirstVehicle
        {
            get
            {
                return this.firstVehicle;
            }

            set
            {
                this.firstVehicle = value;
                this.RaisePropertyChangeNumbers();
            }
        }

        /// <summary>
        /// Gets or sets the second vehicle.
        /// </summary>
        public string SecondVehicle
        {
            get
            {
                return this.secondVehicle;
            }

            set
            {
                this.secondVehicle = value;
                this.RaisePropertyChangeNumbers();
            }
        }

        /// <summary>
        /// Gets or sets the third vehicle.
        /// </summary>
        public string ThirdVehicle
        {
            get
            {
                return this.thirdVehicle;
            }

            set
            {
                this.thirdVehicle = value;
                this.RaisePropertyChangeNumbers();
            }
        }

        /// <summary>
        /// Gets or sets the fourth vehicle.
        /// </summary>
        public string FourthVehicle
        {
            get
            {
                return this.fourthVehicle;
            }

            set
            {
                this.fourthVehicle = value;
                this.RaisePropertyChangeNumbers();
            }
        }

        /// <summary>
        /// Gets or sets the jny to index.
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
                this.PopulateViaCollection();
                this.RaisePropertyChangedEventsOnJny();
            }
        }

        /// <summary>
        /// Gets or sets the jny to list.
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
            }
        }

        /// <summary>
        /// Gets or sets the jny from index.
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
                this.PopulateToCollection();
                this.PopulateViaCollection();
                this.RaisePropertyChangedEventsOnJny();
            }
        }

        /// <summary>
        /// Gets or sets the jny from list.
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
            }
        }

        /// <summary>
        /// Gets or sets the jny via index.
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
                this.RaisePropertyChangedEventsOnJny();
            }
        }

        /// <summary>
        /// Gets or sets the jny via list.
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
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                RaisePropertyChangedEvent(nameof(this.Status));
            }
        }

        /// <summary>
        /// Gets the days.
        /// </summary>
        public ObservableCollection<DayViewModel> Days => this.days;

        /// <summary>
        /// Gets the jny distance as a string.
        /// </summary>
        public string JnyDistanceString => this.JnyDistance?.ToString();

        /// <summary>
        /// Show the previous day.
        /// </summary>
        public ICommand PreviousDayCmd { get; private set; }

        /// <summary>
        /// Show the next day.
        /// </summary>
        public ICommand NextDayCmd { get; private set; }

        /// <summary>
        /// Show add jny and reset.
        /// </summary>
        public ICommand AddJnyCmd { get; private set; }

        /// <summary>
        /// Save command.
        /// </summary>
        public ICommand SaveCmd { get; private set; }

        /// <summary>
        /// close window command.
        /// </summary>
        public ICommand CloseCmd { get; private set; }

        /// <summary>
        /// Gets the jny list.
        /// </summary>
        public ObservableCollection<IJourneyViewModel> JnyList { get; set; }

        /// <summary>
        /// Move the <see cref="Date"/> forward 1 day.
        /// </summary>
        private void NextDay()
        {
            this.SetNewDate(this.Date.AddDays(1));
        }

        /// <summary>
        /// Move the <see cref="Date"/> back 1 day.
        /// </summary>
        private void PreviousDay()
        {
            this.SetNewDate(this.Date.AddDays(-1));
        }

        /// <summary>
        /// Set the day to the <paramref name="newDate"/>.
        /// </summary>
        /// <param name="newDate"></param>
        private void SetNewDate(DateTime newDate)
        {
            this.Status = "Selecting new day";

            bool isNewMonth =
                this.date.Year != newDate.Year ||
                this.date.Month != newDate.Month;

            this.date = newDate;
            this.UpdateFirstExampleLists();

            bool success = this.LoadJourneysForSelectedDay();

            if (isNewMonth)
            {
                this.SetupDays();
            }

            this.FindAndSetDaySelectedFlag();

            this.Status =
              success ?
              "New day selected" :
              lastErrorString;

            this.RaisePropertyChangedEventJnysPanelProperties();
            this.RaisePropertyChangedEventsOnDate();
        }

        /// <summary>
        /// Add the journey to the Journey Panel, if successful it clears
        /// the journey panel and updates the days mileage.
        /// </summary>
        private void AddNewJny()
        {
            this.Status = "Adding";
            this.WriteToLine();

            this.ResetInputProperties();
            ++this.jnyNumber;

            this.Days[this.Date.Day - 1].Dist = this.DayDistance.Miles.ToString();

            this.Status = "Journey Added";

            // TODO, can you move focus in WPF
            this.RaisePropertyChangedEventJnysPanelProperties();
            this.RaisePropertyChangedEventsOnDate();
            this.RaisePropertyChangedEventsOnJny();
        }

        /// <summary>
        /// Save the form.
        /// </summary>
        private void SaveDay()
        {
            ColourResourcesClass colourResources = ColourResourcesClass.GetInstance();

            //bool success = true;

            Status = "Saving";

            List<IJourneyDetailsType> currentDayJourneys =
              new List<IJourneyDetailsType>();

            foreach (IJourneyViewModel journey in this.JnyList)
            {
                IJourneyDetailsType converted =
                  JourneyFactory.ToJourneyModel(
                    journey);

                currentDayJourneys.Add(converted);
            }

            DailyInputFactory.SaveDay(
              this.Year,
              this.Month,
              this.Day,
              currentDayJourneys);

            this.firstExamples.CheckNewJnyList(
              currentDayJourneys);

            this.RaisePropertyChangedOnJnyList();

            this.Status = $"Save Completed for {this.Day}/{this.Month}/{this.Year}";
        }

        /// <summary>
        /// Close the current window.
        /// </summary>
        private void CloseWindow()
        {
            this.OnClosingRequest();
        }

        /// <summary>
        /// InitialiseInputForm, performs all the initialisation tasks.
        ///    1. Get the routes data and populate the outward combobox.
        ///    2. Load the current days journeys.
        /// </summary>
        private void InitialiseInputForm()
        {
            string previousvalue = string.Empty;
            string fromString = string.Empty;
            this.JnyFromList = new ObservableCollection<string>();
            this.JnyFromList.Add(string.Empty);
            this.JnyFromIndex = 0;
            this.Status = "Initialising";

            for (int i = 0; i < journeyController.GetMileageDetailsLength(); i++)
            {
                fromString = journeyController.GetFromStation(i);
                if (fromString != previousvalue)
                {
                    JnyFromList.Add(fromString);
                }

                previousvalue = fromString;
            }

            this.ResetInputProperties();

            // load the days journeys
            this.Status =
              LoadJourneysForSelectedDay() ?
              "Initialised" :
              lastErrorString;
        }

        /// <summary>
        /// WriteToLine, wrapper for OutputNewRow puts details into a
        /// list and passes it to OutputNewRow.
        ///    0  day
        ///    1  not used as yet
        ///    2  from
        ///    3  to
        ///    4  via
        ///    5  distance
        ///    6  numberOfVehicles
        ///    7  Number1
        ///    8  Number2{optional}
        ///    9  Number3{optional}
        ///    10 Number4{optional}
        ///  Returns true if it outputs the line successfully.
        /// </summary>
        private void WriteToLine()
        {
            IJourneyViewModel journeyDetails =
              new JourneyViewModel(
                this.firstExamples,
                string.Empty,
                this.JnyList.Count.ToString(),
                JnyFromList[JnyFromIndex],
                JnyToList[JnyToIndex],
                JnyViaList[JnyViaIndex],
                this.Date,
                new MilesChains(this.JnyDistance.ToString()),
                this.FirstVehicle,
                this.SecondVehicle,
                this.ThirdVehicle,
                this.FourthVehicle);

            this.JnyList.Add(journeyDetails);
        }

        /// <summary>
        /// Resets all the input properties and sets the From list value to the last known To list 
        /// value.
        /// </summary>
        private void ResetInputProperties()
        {
            if (this.JnyFromList != null)
            {
                this.JnyFromIndex = this.JnyFromList.IndexOf(this.JnyToList[this.JnyToIndex]);
            }

            this.PopulateToCollection();
            this.PopulateViaCollection();

            this.FirstVehicle = string.Empty;
            this.SecondVehicle = string.Empty;
            this.ThirdVehicle = string.Empty;
            this.FourthVehicle = string.Empty;
        }

        /// <summary>
        /// Jnys for the selected day are loaded into the form.
        /// </summary>
        /// <returns>success flag</returns>
        private bool LoadJourneysForSelectedDay()
        {
            List<JourneyDetailsType> dayDetails = new List<JourneyDetailsType>();
            string filePath = string.Empty;
            Logger logger = Logger.Instance;

            this.JnyList = new ObservableCollection<IJourneyViewModel>();
            foreach (IJourneyDetailsType jny in DailyInputFactory.LoadDay(this.Date))
            {

                IJourneyViewModel journey =
                  JourneyFactory.ToJourneyViewModel(
                    jny,
                    this.firstExamples);

                this.JnyList.Add(journey);
            }

            this.RaisePropertyChangedEvent(nameof(this.JnyList));
            return true;
        }

        /// <summary>
        /// Populate the To collection, based on the from value
        /// </summary>
        private void PopulateToCollection()
        {
            this.jnyToIndex = 0;

            // Do nothing if Jny To is invalid.
            if (this.JnyFromIsValid())
            {
                this.jnyToList =
                  journeyController.GetToCollection(
                    this.JnyFromList[this.JnyFromIndex]);
            }
            else
            {
                this.jnyToList = new ObservableCollection<string>();
                this.jnyToList.Add(string.Empty);
            }
        }

        /// <summary>
        /// Populate the Via collection, based on the from and to value
        /// </summary>
        private void PopulateViaCollection()
        {
            this.jnyViaIndex = 0;

            // Do nothing if Jny To is invalid.
            if (this.JnyToIsValid() &&
              this.JnyFromIsValid())
            {
                this.jnyViaList =
                  journeyController.GetViaCollection(
                    this.JnyFromList[this.JnyFromIndex],
                    this.JnyToList[this.JnyToIndex]);
            }
            else
            {
                this.jnyViaList = new ObservableCollection<string>();
                this.jnyViaList.Add(string.Empty);
            }
        }

        /// <summary>
        /// Determine whether the vehicle properties are valid
        /// </summary>
        /// <returns>valid flag</returns>
        private bool VclePropertiesValid()
        {
            if (this.FourthVehicle != string.Empty &&
              this.ThirdVehicle == string.Empty)
            {
                return false;
            }

            if (this.ThirdVehicle != string.Empty &&
              this.SecondVehicle == string.Empty)
            {
                return false;
            }

            if (this.SecondVehicle != string.Empty &&
              this.FirstVehicle == string.Empty)
            {
                return false;
            }

            if (this.FirstVehicle == string.Empty)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is possible to save
        /// </summary>
        /// <returns></returns>
        private bool CanSave()
        {
            return this.JnyList?.Count > 0;
        }

        /// <summary>
        /// Set up the days ribbon.
        /// </summary>
        private void SetupDays()
        {
            this.days =
              DailyInputFactory.GetAllDaysInMonth(
                this.Year,
                this.Month);

            foreach (DayViewModel day in this.days)
            {
                if (day.CurrentDate.Date == this.Date.Date)
                {
                    day.IsSelected = true;
                }

                day.NewDateCallback = new DateTimeDelegate(this.SetNewDate);
            }
        }

        /// <summary>
        /// Raise property changed on all jny list items.
        /// </summary>
        private void RaisePropertyChangedOnJnyList()
        {
            foreach (IJourneyViewModel jny in this.JnyList)
            {
                jny.CalculateStates();
            }
        }

        /// <summary>
        /// Search through the days to set the selected flag.
        /// </summary>
        /// <remarks>
        /// Method used on the calendar bar.
        /// </remarks>
        private void FindAndSetDaySelectedFlag()
        {
            foreach (DayViewModel day in this.Days)
            {
                day.IsSelected =
                  day.CurrentDate.Date == this.Date.Date;
            }
        }

        /// <summary>
        /// Returns a flag which indicates whether the Jny To selection is valid
        /// </summary>
        /// <returns>validity flag</returns>
        private bool JnyToIsValid()
        {
            return this.JnyToIndex < this.JnyToList.Count && this.JnyToIndex > 0;
        }

        /// <summary>
        /// Returns a flag which indicates whether the Jny From selction is valid
        /// </summary>
        /// <returns>validity flag</returns>
        private bool JnyFromIsValid()
        {
            return this.JnyFromIndex < this.JnyFromList.Count && this.JnyFromIndex > 0;
        }

        /// <summary>
        /// Returns a flag which indicates whether the Jny From selction is valid
        /// </summary>
        /// <returns>validity flag</returns>
        private bool JnyViaIsValid()
        {
            return this.JnyViaIndex < this.JnyViaList.Count && this.JnyViaIndex > 0;
        }

        /// <summary>
        /// Refresh the lists used to descripe cops.
        /// </summary>
        private void UpdateFirstExampleLists()
        {
            this.firstExamples.LoadCompleteList();

            if (Year.ToString() != currentYear)
            {
                currentYear = Year.ToString();
                this.firstExamples.LoadAnnualList(currentYear);
            }
        }

        /// <summary>
        /// Raise property changed event on the properties covering jny fields.
        /// </summary>
        private void RaisePropertyChangedEventsOnJny()
        {
            this.RaisePropertyChangedEvent(nameof(this.JnyFromIndex));
            this.RaisePropertyChangedEvent(nameof(this.JnyToIndex));
            this.RaisePropertyChangedEvent(nameof(this.JnyToList));
            this.RaisePropertyChangedEvent(nameof(this.JnyViaIndex));
            this.RaisePropertyChangedEvent(nameof(this.JnyViaList));
            this.RaisePropertyChangedEvent(nameof(this.JnyDistance));
            this.RaisePropertyChangedEvent(nameof(this.JnyDistanceString));
        }

        /// <summary>
        /// Raise property changed event on the properties covering date fields.
        /// </summary>
        private void RaisePropertyChangedEventsOnDate()
        {
            this.RaisePropertyChangedEvent(nameof(this.Date));
            this.RaisePropertyChangedEvent(nameof(this.Day));
            this.RaisePropertyChangedEvent(nameof(this.Month));
            this.RaisePropertyChangedEvent(nameof(this.Year));
            this.RaisePropertyChangedEvent(nameof(this.Days));
            this.RaisePropertyChangedEvent(nameof(this.DayDistance));
            this.RaisePropertyChangedEvent(nameof(this.DayDistanceString));
        }

        /// <summary>
        /// Raise property changed event on the properties covering the jny panel.
        /// </summary>
        private void RaisePropertyChangedEventJnysPanelProperties()
        {
            this.RaisePropertyChangedEvent(nameof(this.JnyList));
        }

        /// <summary>
        /// Raise property changed event on the properties covering the numbers.
        /// </summary>
        private void RaisePropertyChangeNumbers()
        {
            RaisePropertyChangedEvent(nameof(this.FirstVehicle));
            RaisePropertyChangedEvent(nameof(this.SecondVehicle));
            RaisePropertyChangedEvent(nameof(this.ThirdVehicle));
            RaisePropertyChangedEvent(nameof(this.FourthVehicle));
            RaisePropertyChangedEvent(nameof(this.NumberOfVehicles));
        }

        /// <summary>
        /// Add <paramref name="newValue"/> to <paramref name="collection"/> if not null or whitespace.
        /// </summary>
        /// <param name="collection">collection of strings</param>
        /// <param name="newValue">value to add to the collection</param>
        private void Add(
          List<string> collection,
          string newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                collection.Add(newValue);
            }
        }
    }
}