using Shap.Interfaces.Common.ViewModels;
using System.Collections.Generic;

namespace Shap.Interfaces.Locations.ViewModels
{
    /// <summary>
    /// Interface which supports the view model which displays a location on a view.
    /// </summary>
    public interface ILocationViewModel : IDetailsViewModel
    {
        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the code of the location.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Gets the size of the location.
        /// </summary>
        string Size { get; }

        /// <summary>
        /// Gets the year that the location opened.
        /// </summary>
        string Opened { get; }

        /// <summary>
        /// Gets the year that the location closed.
        /// </summary>
        string Closed { get; }

        /// <summary>
        /// Gets the region where the location is located.
        /// </summary>
        string Region { get; }

        /// <summary>
        /// Gets the type of the location.
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Gets number from.
        /// </summary>
        string TotalFrom { get; }

        /// <summary>
        /// Gets the number to.
        /// </summary>
        string TotalTo { get; }

        /// <summary>
        /// Gets the collection of photos.
        /// </summary>
        string PhotoPath { get; }

        /// <summary>
        /// Gets the counters for all years.
        /// </summary>
        List<ITravelCounterViewModel> YearCounters { get; }

        /// <summary>
        /// Gets the counters for all classes.
        /// </summary>
        List<ITravelCounterViewModel> ClassCounters { get; }
    }
}