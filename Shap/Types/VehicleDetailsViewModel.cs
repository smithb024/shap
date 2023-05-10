namespace Shap.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Stats;
    using NynaeveLib.Types;
    using NynaeveLib.ViewModel;

    using Common.ViewModel;
    using Shap.Interfaces.Common.ViewModels;
    using Shap.Interfaces.Types;
    using Shap.Types.Factories;
    using Shap.Units.IO;

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// This class is designed to hold all the details of an individual vehicle
    /// as read from a individual file.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public class VehicleDetailsViewModel : ViewModelBase, IVehicleDetailsType
    {
        // TODO Think about how the numbers are stored. 
        // Former numbers are stored in a number type which allows for the current number
        // the current number is local. Mismatch, former numbers are ints, current is string.

        private string number = string.Empty;
        private MilesChains m_mileage = new MilesChains();
        private MilesChains origMilage;
        private DateTime m_lastEntryDate = new DateTime(1970, 1, 1);
        private DateTime m_lastCheckDate = new DateTime(1970, 1, 1);
        private string m_notes = string.Empty;

        /// <summary>
        /// Maintains the index of the currently selected in service state from the
        /// <see cref="ServiceTypeList"/> list of in service states.
        /// </summary>
        private int serviceIndex;

        /// <date>12/08/18</date>
        /// <summary>
        /// Initialises a new instance of the <see cref="VehicleDetailsViewModel"/> class.
        /// </summary>
        public VehicleDetailsViewModel(string unitNumber)
        {
            this.UnitNumber = unitNumber;
            this.FormerNumbers = new VehicleNumberTypeViewModel();
            this.JourneysList = new List<IJourneyViewModel>();
            this.origMilage = new MilesChains();
        }

        /// <date>12/08/2018</date>
        /// <summary>
        /// Initialises a new instance of the <see cref="VehicleDetailsViewModel"/> class.
        /// </summary>
        /// <param name="rawData"> raw data, read from the file.</param>
        public VehicleDetailsViewModel(
          IndividualUnitFileContents rawData,
          IFirstExampleManager firstExamples)
        {
            this.UnitNumber = rawData.UnitNumber;
            this.FormerNumbers = new VehicleNumberTypeViewModel();
            this.m_lastCheckDate = rawData.LastCheckDate;
            this.FormerNumbers = rawData.FormerNumbers;
            this.Notes = rawData.Notes;

            foreach (IJourneyDetailsType jny in rawData.Journeys)
            {
                IJourneyViewModel journey =
                  JourneyFactory.ToJourneyViewModel(
                    jny,
                    firstExamples,
                    this.UnitNumber);

                this.AddJourney(journey);
            }

            this.origMilage = UnitDistance;

            for (int index = 0; index < this.ServiceTypeList.Count; ++index)
            {
                if (this.ServiceTypeList[index] == rawData.InService)
                {
                    this.serviceIndex = index;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the number of the current unit.
        /// </summary>
        public string UnitNumber { get; }

        /// <summary>
        /// Gets the current distance of the unit.
        /// </summary>
        public MilesChains UnitDistance
        {
            get
            {
                MilesChains distance = new MilesChains();

                if (this.JourneysList != null)
                {
                    foreach (IJourneyViewModel journey in this.JourneysList)
                    {
                        distance = distance + journey.Distance;
                    }
                }

                return distance;
            }
        }

        /// <summary>
        /// Gets the currnet distance of the unit in the form of a string.
        /// </summary>
        public string UnitDistanceString => this.UnitDistance.ToString();

        /// <summary>
        /// Gets the unit distance prior to a change.
        /// </summary>
        public MilesChains UnitOrigDistance
        {
            get
            {
                return this.origMilage;
            }

            private set
            {
                this.origMilage = value;
            }
        }

        /// <summary>
        /// Gets the initial distance as a string.
        /// </summary>
        public string UnitOrigDistanceString => this.UnitOrigDistance.ToString();

        /// <summary>
        /// Gets the difference between the original distance and the current distance as a string.
        /// </summary>
        public string UnitDistanceDifferenceString =>
          (this.UnitDistance - this.UnitOrigDistance).ToString();

        /// <summary>
        /// Gets the date of the last time this unit was checked.
        /// </summary>
        public DateTime? UnitLastDate
        {
            get
            {
                if (this.JourneysList != null && this.JourneysList.Count > 0)
                {
                    return this.JourneysList[0].JnyId.Date;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the date of the last time this unit was checked in the form of a string.
        /// </summary>
        public string UnitLastDateString => this.UnitLastDate?.ToString("dd/MM/yyyy") ?? string.Empty;

        /// <summary>
        /// Gets the date of the first time for this unit.
        /// </summary>
        public DateTime? UnitFirstDate
        {
            get
            {
                if (this.JourneysList != null && this.JourneysList.Count > 0)
                {
                    return this.JourneysList[this.JourneysList.Count - 1].JnyId.Date;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the date of the first time for this unit in the form of a string.
        /// </summary>
        public string UnitFirstDateString => this.UnitFirstDate?.ToString("dd/MM/yyyy") ?? string.Empty;

        /// <summary>
        /// Gets the date of the last time this unit was checked.
        /// </summary>
        public DateTime UnitLastCheck
        {
            get
            {
                return this.m_lastCheckDate;
            }

            set
            {
                this.m_lastCheckDate = value;
            }
        }

        /// <summary>
        /// Gets the date of the last time this unit was checked in the form of a string.
        /// </summary>
        public string UnitLastCheckString => this.UnitLastCheck.ToString("dd/MM/yyyy");

        /// <summary>
        /// Gets an sets notes about the <see cref="VehicleDetailsViewModel"/>
        /// </summary>
        public string Notes
        {
            get
            {
                return this.m_notes;
            }

            set
            {
                this.m_notes = value;
                this.RaisePropertyChangedEvent(nameof(this.Notes));
            }
        }


        /// <summary>
        /// Gets the current status of the unit.
        /// </summary>
        public VehicleServiceType Status
        {
            get
            {
                return this.ServiceTypeList[this.ServiceIndex];
            }
        }

        /// <summary>
        /// Gets a collection containing all the enumerations in <see cref="VehicleServiceType"/>.
        /// </summary>
        public List<VehicleServiceType> ServiceTypeList =>
          System.Enum.GetValues(typeof(VehicleServiceType)).
          Cast<VehicleServiceType>().
          ToList();

        /// <summary>
        /// Gets or sets the index of the currently selected in service state from the
        /// <see cref="ServiceTypeList"/> list of in service states.
        /// </summary>
        public int ServiceIndex
        {
            get
            {
                return this.serviceIndex;
            }

            set
            {
                this.serviceIndex = value;
                this.RaisePropertyChangedEvent(nameof(ServiceIndex));
                this.RaisePropertyChangedEvent(nameof(Status));
            }
        }

        /// <summary>
        /// Gets the former numbers of this <see cref="VehicleDetailsViewModel"/>
        /// </summary>
        public IVehicleNumberType FormerNumbers { get; }

        /// <summary>
        /// Gets the collection of journeys.
        /// </summary>
        public List<IJourneyViewModel> JourneysList { get; set; }

        /// <summary>
        /// Gets the first three items in the <see cref="JourneysList"/>.
        /// </summary>
        public List<IJourneyViewModel> JourneysCutdownList
        {
            get
            {
                if (this.NumberOfJnys < 3)
                {
                    return this.JourneysList;
                }

                return this.JourneysList.GetRange(0, 3);
            }
        }

        /// <summary>
        /// Gets the number of journeys
        /// </summary>
        public int NumberOfJnys => this.JourneysList?.Count ?? 0;

        /// <summary>
        /// Gets a count of each location to.
        /// </summary>
        public LocationCounterManagerViewModel ToLocation => this.CountLocations(false);

        /// <summary>
        /// Gets a count of each location from.
        /// </summary>
        public LocationCounterManagerViewModel FromLocation => this.CountLocations(true);

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>addJourney</name>
        /// <date>22/10/12</date>
        /// <summary>
        /// add journey
        /// </summary>
        /// <param name="newJourney">new journey</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        protected void AddJourney(IJourneyViewModel newJourney)
        {
            if (this.JourneysList == null)
            {
                this.JourneysList = new List<IJourneyViewModel>();
            }

            //newJourney.ParentUnitNumber = this.UnitNumber;
            this.JourneysList.Insert(0, newJourney);
        }

        /// <summary>
        /// Update has completed, refresh the view.
        /// </summary>
        public void CompleteUpdate()
        {
            this.RaisePropertyChangedEvent(nameof(this.UnitDistance));
            this.RaisePropertyChangedEvent(nameof(this.UnitDistanceString));
            this.RaisePropertyChangedEvent(nameof(this.UnitDistanceDifferenceString));
            this.RaisePropertyChangedEvent(nameof(this.NumberOfJnys));

            this.RaisePropertyChangedEvent(nameof(this.JourneysList));
            this.RaisePropertyChangedEvent(nameof(this.JourneysCutdownList));

            this.RaisePropertyChangedEvent(nameof(this.UnitOrigDistance));
            this.RaisePropertyChangedEvent(nameof(this.UnitOrigDistanceString));
            this.RaisePropertyChangedEvent(nameof(this.UnitDistanceDifferenceString));

            this.RaisePropertyChangedEvent(nameof(this.UnitFirstDate));
            this.RaisePropertyChangedEvent(nameof(this.UnitFirstDateString));
            this.RaisePropertyChangedEvent(nameof(this.UnitLastDate));
            this.RaisePropertyChangedEvent(nameof(this.UnitLastDateString));
            this.RaisePropertyChangedEvent(nameof(this.UnitLastCheck));
            this.RaisePropertyChangedEvent(nameof(this.UnitLastCheckString));

            this.RaisePropertyChangedEvent(nameof(this.ToLocation));
            this.RaisePropertyChangedEvent(nameof(this.FromLocation));

            this.RaisePropertyChangedEvent(nameof(this.Status));
        }

        /// <summary>
        /// Loop through each <see cref="JourneysList"/> and count each of the 
        /// <see cref="IJourneyDetailsType.To"/> items. Order alphabetically and return.
        /// </summary>
        /// <param name="isFrom">indicates whether to count from or to</param>
        /// <returns>Count of each From or To location</returns>
        private LocationCounterManagerViewModel CountLocations(bool isFrom)
        {
            LocationCounterManagerViewModel counter = new LocationCounterManagerViewModel();

            if (this.JourneysList != null)
            {
                foreach (IJourneyViewModel journey in this.JourneysList)
                {
                    counter.AddLocation(isFrom ? journey.From : journey.To);
                }
            }

            counter.Sort();
            return counter;
        }
    }
}