namespace Shap.Analysis.ViewModels
{
    using NynaeveLib.ViewModel;
    using ResultsPresentation;

    /// <summary>
    /// View model for location analysis.
    /// </summary>
    public class LocationAnalysisViewModel : ViewModelBase
    {
        /// <summary>
        /// Results table for the locations counter.
        /// </summary>
        private LocationCounterResultsViewModel resultsTable;

        public LocationAnalysisViewModel()
        {
            this.LocationControls =
              new LocationAnalysisDashboardViewModel(
                this.LocationsGeneralReport,
                this.LocationsYearReport,
                this.SingleLocationGeneralReport,
                this.SingleLocationYearReport);

            this.resultsTable = new LocationCounterResultsViewModel();
        }

        /// <summary>
        /// Gets the location controls information.
        /// </summary>
        public LocationAnalysisDashboardViewModel LocationControls { get; }

        /// <summary>
        /// Gets the results table.
        /// </summary>
        public object ResultsTable => this.resultsTable;

        /// <summary>
        /// </summary>
        /// <param name="results">results to write</param>
        private void LocationsGeneralReport(ReportCounterManager<LocationCounter> results)
        {
            this.resultsTable.ResetLocations(
                results, 
                false);
        }

        /// <summary>
        /// </summary>
        /// <param name="results">results to write</param>+
        /// <param name="year">report year</param>
        private void LocationsYearReport(
          ReportCounterManager<LocationCounter> results,
          string year)
        {
            this.resultsTable.ResetLocations(results, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="results">results to write</param>
        /// <param name="location">report location </param>
        private void SingleLocationGeneralReport(
          ReportCounterManager<LocationCounter> results,
          string location)
        {
            this.resultsTable.ResetLocations(results, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <param name="year"></param>
        /// <param name="location"></param>. .: - -
        private void SingleLocationYearReport(
          ReportCounterManager<LocationCounter> results,
          string year,
          string location)
        {
            this.resultsTable.ResetLocations(results, true);
        }
    }
}