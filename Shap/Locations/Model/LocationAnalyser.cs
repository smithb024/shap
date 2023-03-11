namespace Shap.Locations.Model
{
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using System.Collections.Generic;

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
        public void Analyse(List<string> locations)
        {

        }

        // Loop through each location.

        // For each location:
        // Create new lists for trips, classes and years.
        // Create empty to and from counts.
        // Open the LocationsDetail class for the location.
        // Loop through each year.

        // For each year:
        // Create a LocationYear object.
        // Loop through each month.

        // For each month:
        // Open the month file.
        // Loop through each entry in the month file:

        // For each entry:
        // If the location is not present then return.
        // Add a count to the to and from count.
        // Add a count to the LocationYear.
        // Create and add a trip object. Add it to the list and remove the oldest one if there are now more than 10.
        // Loop through each unit.

        // For each unit:
        // Determine the type. 
        // If this is the first of the type in the entry:
        // If type is present in the type list:
        // Add a count to the specific type.
        // Else:
        // Create a type object and include the type.
        // Add the type to the type list.

        // For each year:
        // Add the LocationYear object to the list.{If the count is zero and there is nothing in the list don't add, this will prevent leading zeros}

        // For each location:
        // Overwrite the trips classes and years collections.
        // Update the to and from counters {could be done along the way}
        // Save the location.
        // Send a location updated message?
    }
}