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
    /// View model which supports the totals counter table.
    /// </summary>
    public class TotalsCounterResultsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TotalsCounterResultsViewModel"/> class.
        /// </summary>
        public TotalsCounterResultsViewModel()
        {
            this.Totals = new List<TotalsViewModel>();

            this.SortNameCommand = new CommonCommand(this.SortByName);
            this.SortTotalCommand = new CommonCommand(this.SortByTotal);
        }

        /// <summary>
        /// Gets the totals table.
        /// </summary>
        public List<TotalsViewModel> Totals { get; private set; }

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
        /// <param name="results">The new set of results</param>
        public void ResetTotals(
            ReportCounterManager<ClassCounter> results)
        {
            this.Totals =
                new List<TotalsViewModel>();

            foreach (ClassCounter counter in results.CounterCollection)
            {
                this.Totals.Add(
                    new TotalsViewModel(
                        counter.Id,
                        counter.Total));
            }
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
                    $"Analysis - Sort by totals.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            this.Totals =
                this.Totals.OrderByDescending(loc => loc.Total).ToList();
            this.OnPropertyChanged(nameof(this.Totals));
        }
    }
}