﻿namespace Shap.Analysis.ViewModels.ResultsPresentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Commands;
    using Data;
    using NynaeveLib.ViewModel;

    public class LocationCounterResultsViewModel : ViewModelBase
    {
        public LocationCounterResultsViewModel()
        {
            this.Locations = new List<LocationViewModel>();

            this.SortNameCommand = new CommonCommand(this.SortByName);
            this.SortTotalCommand = new CommonCommand(this.SortByTotal);
            this.SortFromCommand = new CommonCommand(this.SortByFrom);
            this.SortToCommand = new CommonCommand(this.SortByTo);
        }

        public List<LocationViewModel> Locations { get; private set; }

        /// <summary>
        /// Sort by name command.
        /// </summary>
        public ICommand SortNameCommand { get; private set; }

        /// <summary>
        /// Sort by total command.
        /// </summary>
        public ICommand SortTotalCommand { get; private set; }

        /// <summary>
        /// Sort by from command.
        /// </summary>
        public ICommand SortFromCommand { get; private set; }

        /// <summary>
        /// Sort by to command.
        /// </summary>
        public ICommand SortToCommand { get; private set; }

        public void ResetLocations(
            ReportCounterManager<LocationCounter> results,
            bool isYear)
        {
            this.Locations.Clear();

            foreach (LocationCounter counter in results.CounterCollection)
            {
                this.Locations.Add(
                    new LocationViewModel(
                        counter.Id,
                        counter.Total,
                        counter.From,
                        counter.To,
                        isYear));
            }

            this.SortByTotal();
        }

        /// <summary>
        /// Sort locations list by name.
        /// </summary>
        private void SortByName()
        {
            if (!this.AreLocationsValid())
            {
                return;
            }

            this.Locations =
                this.Locations.OrderBy(loc => loc.Name).ToList();
            this.UpdateIndexes();
            this.RaisePropertyChangedEvent(nameof(this.Locations));
        }

        /// <summary>
        /// Sort locations list by total.
        /// </summary>
        private void SortByTotal()
        {
            if (!this.AreLocationsValid())
            {
                return;
            }

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.Total).ToList();
            this.UpdateIndexes();
            this.RaisePropertyChangedEvent(nameof(this.Locations));
        }

        /// <summary>
        /// Sort locations list by from.
        /// </summary>
        private void SortByFrom()
        {
            if (!this.AreLocationsValid())
            {
                return;
            }

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.NumberFrom).ToList();
            this.UpdateIndexes();
            this.RaisePropertyChangedEvent(nameof(this.Locations));
        }

        /// <summary>
        /// Sort locations list by to.
        /// </summary>
        private void SortByTo()
        {
            if (!this.AreLocationsValid())
            {
                return;
            }

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.NumberTo).ToList();
            this.UpdateIndexes();
            this.RaisePropertyChangedEvent(nameof(this.Locations));
        }

        /// <summary>
        /// Set up the index for each location. It is used on the table to count the rows.
        /// </summary>
        private void UpdateIndexes()
        {
            if (!this.AreLocationsValid())
            {
                return;
            }

            int counter = 1;

            foreach (LocationViewModel location in this.Locations)
            {
                location.Index = counter;
                ++counter;
            }
        }

        /// <summary>
        /// Return a value indicating whether thw locations collection is valid
        /// </summary>
        /// <returns></returns>
        private bool AreLocationsValid()
        {
            return !(this.Locations == null || this.Locations.Count == 0);
        }
    }
}
