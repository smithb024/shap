namespace Shap.Locations.Model
{
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Io;
    using Shap.StationDetails;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LocationManager : ILocationManager
    {
        /// <summary>
        /// The IO Controllers class.
        /// </summary>
        private readonly IIoControllers controllers;

        /// <summary>
        /// Initialise a new instance of the <see cref="LocationManager"/> class.
        /// </summary>
        /// <param name="ioControllers">IO Controllers</param>
        public LocationManager(
            IIoControllers ioControllers)
        {
            this.controllers = ioControllers;
            this.Initialise();
        }

        /// <summary>
        /// Gets a collection of all available locations.
        /// </summary>
        /// <returns>A list of locations</returns>
        public List<string> GetLocations()
        {
            JourneyIOController journeyController = JourneyIOController.GetInstance();

            //this.stnList.Clear();
            //this.stnList.Add(string.Empty);

            // Get all locations from the model.
            int routesCount = journeyController.GetMileageDetailsLength();
            List<string> workingList = new List<string>();
            List<string> returnList = new List<string>();

            for (int i = 0; i < routesCount; i++)
            {
                workingList.Add(journeyController.GetFromStation(i));
            }

            // Ensure the locations are in alphabetical order.
            workingList.Sort();

            // Filter out duplicates.
            string previousvalue = string.Empty;
            foreach (string location in workingList)
            {
                if (location != previousvalue)
                {
                    returnList.Add(location);
                }

                previousvalue = location;
            }

            return returnList;
        }

        /// <summary>
        /// Build/Rebuild the xaml files to ensure a complete set is available.
        /// </summary>
        public void Initialise()
        {
            List<string> locations = this.GetLocations();

            foreach (string location in locations)
            {
                if (!this.controllers.Location.DoesFileExist(location))
                {
                    LocationDetails locationDetails =
                        new LocationDetails()
                        {
                            Name = location,
                            Category = Types.Enum.LocationCategories.ND
                        };

                    this.controllers.Location.Write(
                        locationDetails,
                        location);
                }
            }
        }
    }
}