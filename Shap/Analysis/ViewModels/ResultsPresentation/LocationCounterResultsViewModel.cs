namespace Shap.Analysis.ViewModels.ResultsPresentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Commands;
    using Data;
    using NynaeveLib.ViewModel;
    using Shap.Messages;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the table for locations.
    /// </summary>
    public class LocationCounterResultsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LocationCounterResultsViewModel"/> class.
        /// </summary>
        public LocationCounterResultsViewModel()
        {
            this.Locations = new List<LocationViewModel>();
            this.Totals = null;
                //new LocationViewModel(
                //    "Totals",
                //    0,
                //    0,
                //    0,
                //    false,
                //    false);

            this.SortNameCommand = new CommonCommand(this.SortByName);
            this.SortTotalCommand = new CommonCommand(this.SortByTotal);
            this.SortFromCommand = new CommonCommand(this.SortByFrom);
            this.SortToCommand = new CommonCommand(this.SortByTo);
        }

        /// <summary>
        /// Gets the analysis totals.
        /// </summary>
        public LocationViewModel Totals { get; private set; }

        /// <summary>
        /// Gets the collection of all locations present in the analysis.
        /// </summary>
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

        /// <summary>
        /// Update the location results.
        /// </summary>
        /// <param name="results">The results to display</param>
        /// <param name="isYear">Indicates whether this relates to a year.</param>
        public void ResetLocations(
            ReportCounterManager<LocationCounter> results,
            bool isYear)
        {
            this.Locations.Clear();

            int totalFrom = 0;
            int totalTo = 0;

            foreach (LocationCounter counter in results.CounterCollection)
            {
                this.Locations.Add(
                    new LocationViewModel(
                        counter.Id,
                        counter.Total,
                        counter.From,
                        counter.To,
                        isYear));

                totalFrom += counter.From;
                totalTo += counter.To;
            }

            this.Totals =
                new LocationViewModel(
                    "Totals",
                    totalFrom + totalTo,
                    totalFrom,
                    totalTo,
                    isYear,
                    false);
            this.OnPropertyChanged(nameof(this.Totals));

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

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by name.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Locations =
                this.Locations.OrderBy(loc => loc.Name).ToList();
            this.UpdateIndexes();
            this.OnPropertyChanged(nameof(this.Locations));
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

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by total.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.Total).ToList();
            this.UpdateIndexes();
            this.OnPropertyChanged(nameof(this.Locations));
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

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by from.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.NumberFrom).ToList();
            this.UpdateIndexes();
            this.OnPropertyChanged(nameof(this.Locations));
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

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by to.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Locations =
                this.Locations.OrderByDescending(loc => loc.NumberTo).ToList();
            this.UpdateIndexes();
            this.OnPropertyChanged(nameof(this.Locations));
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
