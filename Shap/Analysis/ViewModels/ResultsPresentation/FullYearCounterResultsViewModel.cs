namespace Shap.Analysis.ViewModels.ResultsPresentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Commands;
    using Data;
    using NynaeveLib.ViewModel;

    public class FullYearCounterResultsViewModel : ViewModelBase
    {
        public FullYearCounterResultsViewModel()
        {
            this.Totals = new List<FullYearViewModel>();

            this.SortNameCommand = new CommonCommand(this.SortByName);
            this.SortTotalCommand = new CommonCommand(this.SortByTotal);
        }

        public List<FullYearViewModel> Totals { get; private set; }

        /// <summary>
        /// Sort by name command.
        /// </summary>
        public ICommand SortNameCommand { get; private set; }

        /// <summary>
        /// Sort by total command.
        /// </summary>
        public ICommand SortTotalCommand { get; private set; }

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

        public void UpdateView()
        {
            this.RaisePropertyChangedEvent(nameof(this.Totals));
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

            this.Totals =
                this.Totals.OrderBy(loc => loc.Name).ToList();
            this.RaisePropertyChangedEvent(nameof(this.Totals));
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

            this.Totals =
                this.Totals.OrderByDescending(loc => loc.Total).ToList();
            this.RaisePropertyChangedEvent(nameof(this.Totals));
        }
    }
}