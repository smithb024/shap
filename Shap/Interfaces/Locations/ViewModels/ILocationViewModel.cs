﻿using Shap.Icons.ListViewItems;
using Shap.Interfaces.Common.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        int TotalFrom { get; }

        /// <summary>
        /// Gets the number to.
        /// </summary>
        int TotalTo { get; }

        /// <summary>
        /// Gets the collection of photos.
        /// </summary>
        string PhotoPath { get; }

        /// <summary>
        /// Gets the counters for all years.
        /// </summary>
        ObservableCollection<ITravelCounterViewModel> YearCounters { get; }

        /// <summary>
        /// Gets the counters for all classes.
        /// </summary>
        ObservableCollection<ITravelCounterViewModel> ClassCounters { get; }

        /// <summary>
        /// Gets the collection of all known operators assigned to the current location.
        /// </summary>
        ObservableCollection<IOperatorListItemViewModel> LocationOperators { get; }

        /// <summary>
        /// Gets the collection of the latest journeys.
        /// </summary>
        ObservableCollection<IJourneyViewModel> Journeys { get; }
    }
}