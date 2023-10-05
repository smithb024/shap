namespace Shap.Analysis.ViewModels
{
    using NynaeveLib.ViewModel;
    using ResultsPresentation;
    using Shap.Interfaces.Io;

    /// <summary>
    /// View model for class analysis.
    /// </summary>
    public class ClassAnalysisViewModel : ViewModelBase
    {
        /// <summary>
        /// Results table for the locations counter.
        /// </summary>
        private LocationCounterResultsViewModel locationCounterResultsTable;

        /// <summary>
        /// Results table for the totals counter.
        /// </summary>
        private TotalsCounterResultsViewModel totalsCounterResultsTable;

        /// <summary>
        /// Results table for the full year totals counter.
        /// </summary>
        private FullYearCounterResultsViewModel fullYearCounterResultsTable;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClassAnalysisViewModel"/> class.
        /// </summary>
        /// <param name="groupsAndClassesIoController">
        /// IO Controller for groups and controllers.
        /// </param>
        public ClassAnalysisViewModel(
            IIoControllers controllers)
        {
            this.ClassControls =
                   new ClassAnalysisDashboardViewModel(
                     controllers,
                     this.ClassGeneralReportResults,
                     this.ClassSingleYearReportResults,
                     this.SingleClassGeneralLocationReportResults,
                     this.SingleClassSingleYearLocationReportResults);

            this.locationCounterResultsTable = new LocationCounterResultsViewModel();
            this.totalsCounterResultsTable = new TotalsCounterResultsViewModel();
            this.fullYearCounterResultsTable = new FullYearCounterResultsViewModel();
            this.ResultsTable = this.locationCounterResultsTable;
        }

        /// <summary>
        /// Gets the Class controls information
        /// </summary>
        public ClassAnalysisDashboardViewModel ClassControls { get; }

        /// <summary>
        /// Gets the results table.
        /// </summary>
        public object ResultsTable { get; private set; }

        private void ClassGeneralReportResults(
          ReportCounterManager<ClassCounter> results)
        {
            this.totalsCounterResultsTable.ResetTotals(results);
            this.ResultsTable = this.totalsCounterResultsTable;
            this.OnPropertyChanged(nameof(this.ResultsTable));
        }

        private void ClassSingleYearReportResults(
          ReportCounterManager<YearCounter> results,
          string year)
        {
            this.fullYearCounterResultsTable.ResetTotals(results);
            this.ResultsTable = this.fullYearCounterResultsTable;
            this.OnPropertyChanged(nameof(this.ResultsTable));
            this.fullYearCounterResultsTable.UpdateView();
        }

        private void SingleClassGeneralLocationReportResults(
          ReportCounterManager<LocationCounter> results,
          string cls)
        {
            this.locationCounterResultsTable.ResetLocations(results, false);
            this.ResultsTable = this.locationCounterResultsTable;
            this.OnPropertyChanged(nameof(this.ResultsTable));
        }

        private void SingleClassSingleYearLocationReportResults(
          ReportCounterManager<LocationCounter> results,
          string year,
          string cls)
        {
            this.locationCounterResultsTable.ResetLocations(results, true);
            this.ResultsTable = this.locationCounterResultsTable;
            this.OnPropertyChanged(nameof(this.ResultsTable));
        }
    }
}