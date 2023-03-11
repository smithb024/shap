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
    }
}