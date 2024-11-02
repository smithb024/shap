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
    /// View model which supports the full year counter table.
    /// </summary>
    public class FullYearCounterResultsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FullYearCounterResultsViewModel"/> class.
        /// </summary>
        public FullYearCounterResultsViewModel()
        {
            this.Totals = new List<FullYearViewModel>();

            this.SortNameCommand = new CommonCommand(this.SortByName);
            this.SortTotalCommand = new CommonCommand(this.SortByTotal);
        }

        /// <summary>
        /// Gets the totals table.
        /// </summary>
        public List<FullYearViewModel> Totals { get; private set; }

        /// <summary>
        /// Sort by name command.
        /// </summary>
        public ICommand SortNameCommand { get; private set; }

        /// <summary>
        /// Sort by total command.
        /// </summary>
        public ICommand SortTotalCommand { get; private set; }

        /// <summary>
        /// Reset the totals.
        /// </summary>
        /// <param name="results">The new results.</param>
        public void ResetTotals(
            ReportCounterManager<YearCounter> results)
        {
            this.Totals =
                new List<FullYearViewModel>();

            foreach (YearCounter counter in results.CounterCollection)
            {
                this.Totals.Add(
                    new FullYearViewModel(
                        counter.Id,
                        counter.Total,
                        counter.Jan,
                        counter.Feb,
                        counter.Mar,
                        counter.Apr,
                        counter.May,
                        counter.Jun,
                        counter.Jul,
                        counter.Aug,
                        counter.Sept,
                        counter.Oct,
                        counter.Nov,
                        counter.Dec));
            }
        }

        /// <summary>
        /// Update the table.
        /// </summary>
        public void UpdateView()
        {
            this.OnPropertyChanged(nameof(this.Totals));
        }

        /// <summary>
        /// Sort locations list by name.
        /// </summary>
        private void SortByName()
        {
            if (this.Totals == null || this.Totals.Count == 0)
            {
                return;
            }

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by name.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Totals =
                this.Totals.OrderBy(loc => loc.Name).ToList();
            this.OnPropertyChanged(nameof(this.Totals));
        }

        /// <summary>
        /// Sort locations list by total.
        /// </summary>
        private void SortByTotal()
        {
            if (this.Totals == null || this.Totals.Count == 0)
            {
                return;
            }

            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Sort by total.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Totals =
                this.Totals.OrderByDescending(loc => loc.Total).ToList();
            this.OnPropertyChanged(nameof(this.Totals));
        }
    }
}