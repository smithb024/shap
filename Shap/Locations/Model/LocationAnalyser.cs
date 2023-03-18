namespace Shap.Locations.Model
{
    using Shap.Common;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Config.GroupsAndClasses;
    using Shap.Input.Factories;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Types;
    using Shap.Types;
    using Shap.Units.Factories;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class which manages the location analysis.
    /// </summary>
    public class LocationAnalyser : ILocationAnalyser
    {
        /// <summary>
        /// The IO controllers class.
        /// </summary>
        private readonly IIoControllers ioControllers;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationAnalyser"/> class.
        /// </summary>
        /// <param name="ioControllers">
        /// The IO Controllers class.
        /// </param>
        public LocationAnalyser(
            IIoControllers ioControllers)
        {
            this.ioControllers = ioControllers;
        }

        /// <summary>
        /// Analyse all the locations in the list.
        /// </summary>
        /// <param name="locations">
        /// The locations to analyse.
        /// </param>
        public void Analyse(
            List<string> locations)
        {
            // Loop through each location.
            List<GroupsType> types = ioControllers.Gac.LoadFile();

            foreach(string location in locations)
            {
                this.AnalyseLocation(
                    location,
                    ioControllers,
                    types);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="ioController"></param>
        /// <param name="types"></param>
        private void AnalyseLocation(
            string location,
            IIoControllers ioController,
            List<GroupsType> types)
        {
            // For each location:
            // Create new lists for trips, classes and years.
            // Create empty to and from counts.
            // Open the LocationsDetail class for the location.
            // Loop through each year.
            LocationDetails currentLocation =
                ioController.Location.Read(
                    location);

            currentLocation.Classes = new List<LocationClass>();
            currentLocation.Trips = new List<Trip>();
            currentLocation.Years = new List<LocationYear>();
            currentLocation.TotalFrom = 0;
            currentLocation.TotalTo = 0;

            string[] yearDirsArray =
              Directory.GetDirectories(
                BasePathReader.GetBasePath() + StaticResources.baPath);

            foreach (string yearDirectory in yearDirsArray)
            {
                string calculatedYear = yearDirectory.Substring(yearDirectory.LastIndexOf('\\') + 1);

                if (!int.TryParse(calculatedYear, out int year))
                {
                    continue;
                }

                this.AnalyseYear(
                    year,
                    yearDirectory,
                    currentLocation,
                    new List<LocationClass>(),
                    types);
            }

            // For each location:
            // Overwrite the trips classes and years collections.
            // Update the to and from counters {could be done along the way}
            // Save the location.
            // Send a location updated message?

            ioController.Location.Write(
                currentLocation,
                location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="yearDirectory"></param>
        /// <param name="currentLocation"></param>
        /// <param name="locationClasses"></param>
        /// <param name="types"></param>
        private void AnalyseYear(
            int year,
            string yearDirectory,
            LocationDetails currentLocation,
            List<LocationClass> locationClasses,
            List<GroupsType> types)
        {
            // For each year:
            // Create a LocationYear object.
            // Loop through each month.

            LocationYear currentLocationYear =
                new LocationYear()
                {
                    Year = year,
                    TotalFrom = 0,
                    TotalTo = 0
                };

            string[] fileNamesArray = Directory.GetFiles(yearDirectory);
            for (int monthIndex = 0; monthIndex < fileNamesArray.Count(); ++monthIndex)
            {
                int? currentMonth = Searcher.ConvertMonthFilename(fileNamesArray[monthIndex]);

                if (currentMonth == null)
                {
                    continue;
                }

                this.AnalyseMonth(
                    year,
                    (int)currentMonth,
                    currentLocation,
                    currentLocationYear,
                    locationClasses,
                    types);
            }

            // For each year:
            // Add the LocationYear object to the list.{If the count is zero and there is nothing in the list don't add, this will prevent leading zeros}
            if (currentLocationYear.TotalFrom > 0 ||
                currentLocationYear.TotalTo > 0)
            {
                currentLocation.Years.Add(currentLocationYear);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="currentLocation"></param>
        /// <param name="currentLocationYear"></param>
        /// <param name="locationClasses"></param>
        /// <param name="types"></param>
        private void AnalyseMonth(
            int year,
            int month,
            LocationDetails currentLocation,
            LocationYear currentLocationYear,
            List<LocationClass> locationClasses,
            List<GroupsType> types)
        {
            // For each month:
            // Open the month file.
            // Loop through each entry in the month file:
            List<IJourneyDetailsType> jnysList =
              DailyInputFactory.LoadMonth(
                year,
                month);

            foreach (IJourneyDetailsType journey in jnysList)
            {
                this.AnalyseJourney(
                    currentLocation,
                    currentLocationYear,
                    journey,
                    locationClasses,
                    types);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentLocation"></param>
        /// <param name="currentLocationYear"></param>
        /// <param name="journey"></param>
        /// <param name="locationClasses"></param>
        /// <param name="types"></param>
        private void AnalyseJourney(
            LocationDetails currentLocation,
            LocationYear currentLocationYear,
            IJourneyDetailsType journey,
            List<LocationClass> locationClasses,
            List<GroupsType> types)
        {
            // For each entry:
            // If the location is not present then return.
            // Add a count to the to and from count.
            // Add a count to the LocationYear.
            // Create and add a trip object. Add it to the list and remove the oldest one if there are now more than 10.
            // Loop through each unit.
            bool isFrom;

            if (string.Compare(currentLocation.Name, journey.From) == 0)
            {
                ++currentLocation.TotalFrom;
                ++currentLocationYear.TotalFrom;
                isFrom = true;
            }
            else if (string.Compare(currentLocation.Name, journey.To) == 0)
            {
                ++currentLocation.TotalTo;
                ++currentLocationYear.TotalTo;
                isFrom = false;
            }
            else
            {
                return;
            }

            Trip trip = 
                new Trip()
                {
                    Unit1 = journey.Units[0],
                    Unit2 = journey.Units.Count >= 2 ? journey.Units[1] : string.Empty,
                    Unit3 = journey.Units.Count >= 3 ? journey.Units[2] : string.Empty,
                    Unit4 = journey.Units.Count >= 4 ? journey.Units[3] : string.Empty,
                    From = journey.From,
                    To = journey.To,
                    Route = journey.Via,
                    Date = journey.Date.ToString("dd-MM-yyyy"),
                    Distance = journey.Distance.ToString()
                };

            currentLocation.Trips.Insert(0, trip);
            if (currentLocation.Trips.Count > 10)
            {
                currentLocation.Trips.RemoveAt(10);
            }

            List<string> foundUnitTypes = new List<string>();

            foreach(string unit in journey.Units)
            {
                string foundUnitType =
                    this.AnalyseUnit(
                        unit,
                        isFrom,
                        foundUnitTypes,
                        locationClasses,
                        types);

                if (!string.IsNullOrEmpty(foundUnitType))
                {
                    foundUnitTypes.Add(foundUnitType);

                    this.UpdateTypeCount(
                        currentLocation,
                        foundUnitType,
                        isFrom);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="isFrom"></param>
        /// <param name="foundUnitTypes"></param>
        /// <param name="locationClasses"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private string AnalyseUnit(
            string unitId,
            bool isFrom,
            List<string> foundUnitTypes,
            List<LocationClass> locationClasses,
            List<GroupsType> types)
        {
            // For each unit:
            // Determine the type. 
            // If this is the first of the type in the entry (return if not the first of the type):
            // If type is present in the type list:
            // Add a count to the specific type.
            // Else:
            // Create a type object and include the type.
            // Add the type to the type list.
            string unitType =
                GacDecoder.GetClass(
                    unitId,
                    types);

            if (foundUnitTypes.Contains(unitType))
            {
                return string.Empty;
            }

            LocationClass foundLocationClass =
                locationClasses.Find(m => string.Compare(m.Name, unitType) == 0);

            if (foundLocationClass == null)
            {
                LocationClass newLocationClass =
                    new LocationClass()
                    {
                        Name = unitType,
                        TotalFrom = isFrom ? 1 : 0,
                        TotalTo = isFrom ? 0 : 1,
                    };
                locationClasses.Add(newLocationClass);
            }
            else
            {
                if (isFrom)
                {
                    ++foundLocationClass.TotalFrom;
                }
                else
                {
                    ++foundLocationClass.TotalTo;
                }
            }

            return unitType;
        }

        private void UpdateTypeCount(
            LocationDetails currentLocation,
            string unitType,
            bool isFrom)
        {
            LocationClass foundClass =
                currentLocation.Classes.Find(
                    m => string.Equals(unitType, m.Name));

            if (foundClass == null)
            {
                LocationClass newClass =
                    new LocationClass()
                    {
                        Name = unitType,
                        TotalFrom = isFrom ? 1 : 0,
                        TotalTo = isFrom ? 0 : 1
                    };

                currentLocation.Classes.Add(newClass);
            }
            else
            {
                if (isFrom)
                {
                    ++foundClass.TotalFrom;
                }
                else
                {
                    ++foundClass.TotalTo;
                }
            }
        }
    }
}