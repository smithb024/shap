namespace Shap.Analysis.ViewModels
{
    using System;
    using System.Windows.Input;

    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Interfaces.Io;
    using Shap.Messages;
    using Shap.Types.Enum;
    using Windows;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model for the <see cref="AnalysisWindow"/> window.
    /// </summary>
    public class AnalysisViewModel : ViewModelBase
    {
        /// <summary>
        /// status string, used for the status bar.
        /// </summary>
        private string reportStatus;

        /// <summary>
        /// The location analysis window.
        /// </summary>
        private LocationAnalysisWindow locationAnalysisWindow;

        /// <summary>
        /// The location analysis window.
        /// </summary>
        private ClassAnalysisWindow classAnalysisWindow;

        /// <summary>
        /// IO Controller objects.
        /// </summary>
        IIoControllers controllers;

        /// <summary>
        /// Initialises a new instance of the <see cref="AnalysisViewModel"/> class.
        /// </summary>
        public AnalysisViewModel(IIoControllers controllers)
        {
            this.controllers = controllers;
            this.reportStatus = string.Empty;

            this.ClassControls =
              new ClassAnalysisDashboardViewModel(
                this.controllers,
                this.ClassGeneralReportResults,
                this.ClassSingleYearReportResults,
                this.SingleClassGeneralLocationReportResults,
                this.SingleClassSingleYearLocationReportResults);
            this.LocationControls =
              new LocationAnalysisDashboardViewModel(
                this.LocationsGeneralReport,
                this.LocationsYearReport,
                this.SingleLocationGeneralReport,
                this.SingleLocationYearReport);

            this.ClassControls.ProgressEvent = this.UpdateReportStatus;
            this.LocationControls.ProgressEvent += this.UpdateReportStatus;

            this.OpenClassReportWindowCommand = new CommonCommand(this.ShowClassAnalysisWindow);
            this.OpenLocationReportWindowCommand = new CommonCommand(this.ShowLocationAnalysisWindow);
        }

        /// <summary>
        /// Open a class report window.
        /// </summary>
        public ICommand OpenClassReportWindowCommand { get; private set; }

        /// <summary>
        /// Open a location report window.
        /// </summary>
        public ICommand OpenLocationReportWindowCommand { get; private set; }

        /// <summary>
        /// Gets the Class controls information
        /// </summary>
        public ClassAnalysisDashboardViewModel ClassControls { get; }

        /// <summary>
        /// Gets the location controls information.
        /// </summary>
        public LocationAnalysisDashboardViewModel LocationControls { get; }

        public string ReportStatus
        {
            get => this.reportStatus;

            private set
            {
                this.reportStatus = value;

                this.OnPropertyChanged(nameof(this.ReportStatus));
            }
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        private string CurrentDate => DateTime.Now.ToString(ReportFactoryCommon.DatePattern);

        /// <summary>
        /// Show the class analysis window. Only one version can be shown at a given time.
        /// </summary>
        public void ShowClassAnalysisWindow()
        {
            FeedbackMessage feedbackMessage =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"Analysis - Display Class Analysis Window.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            if (this.classAnalysisWindow == null)
            {
                ClassAnalysisViewModel classAnalysisViewModel =
                    new ClassAnalysisViewModel(
                        this.controllers);
                this.classAnalysisWindow = new ClassAnalysisWindow();

                this.SetupWindow(
                  this.classAnalysisWindow,
                  classAnalysisViewModel,
                  this.CloseClassAnalysisWindow,
                  this.ClassAnalysisWindowClosed);
            }

            this.classAnalysisWindow.Focus();
        }

        /// <summary>
        /// Show the location analysis window. Only one version can be shown at a given time.
        /// </summary>
        public void ShowLocationAnalysisWindow()
        {
            FeedbackMessage feedbackMessage =
               new FeedbackMessage(
                   FeedbackType.Navigation,
                   $"Analysis - Display Location Analysis Window.");
            NynaeveMessenger.Default.Send(feedbackMessage);

            if (this.locationAnalysisWindow == null)
            {
                LocationAnalysisViewModel locationAnalysisViewModel =
                  new LocationAnalysisViewModel();
                this.locationAnalysisWindow = new LocationAnalysisWindow();

                this.SetupWindow(
                  this.locationAnalysisWindow,
                  locationAnalysisViewModel,
                  this.CloseLocationAnalysisWindow,
                  this.LocationAnalysisWindowClosed);
            }

            this.locationAnalysisWindow.Focus();
        }

        /// <summary>
        /// Setup and show a window.
        /// </summary>
        /// <remarks>
        /// This is a copy of code from the MainWindowViewModel. Should this be duplicated.
        /// </remarks>
        /// <param name="window">window to set up</param>
        /// <param name="viewModel">view model to assign to the view model</param>
        /// <param name="closedViewMethod">request from the view model to close the view</param>
        /// <param name="closedMethod">method to run when the window closes</param>
        private void SetupWindow(
          System.Windows.Window window,
          ViewModelBase viewModel,
          EventHandler closeViewMethod,
          EventHandler closedMethod)
        {
            // TODO - repeated code.
            window.DataContext = viewModel;

            viewModel.ClosingRequest += closeViewMethod;
            window.Closed += closedMethod;

            window.Show();
            window.Activate();
        }

        /// <summary>
        /// Close child class window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseClassAnalysisWindow(object sender, EventArgs e)
        {
            this.classAnalysisWindow.Close();
        }

        /// <summary>
        /// class analysis window has closed, reset to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClassAnalysisWindowClosed(object sender, EventArgs e)
        {
            this.classAnalysisWindow = null;
        }

        /// <summary>
        /// Close child location window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseLocationAnalysisWindow(object sender, EventArgs e)
        {
            this.locationAnalysisWindow.Close();
        }

        /// <summary>
        /// Location analysis window has closed, reset to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LocationAnalysisWindowClosed(object sender, EventArgs e)
        {
            this.locationAnalysisWindow = null;
        }

        /// <summary>
        /// Update the <see cref="ReportStatus"/> property.
        /// </summary>
        /// <param name="status">new status</param>
        private void UpdateReportStatus(string status)
        {
            this.ReportStatus = status;
        }

        /// <summary>
        /// Write the location general report to a file.
        /// </summary>
        /// <param name="results">results to write</param>
        private void LocationsGeneralReport(ReportCounterManager<LocationCounter> results)
        {
            results.WriteCSVFile(
              $"StnReport_Gen_{this.CurrentDate}.csv",
              "ReportBuilder: Failed to write General Stn Report.");
        }

        /// <summary>
        /// Write a locations single year report to a file.
        /// </summary>
        /// <param name="results">results to write</param>
        /// <param name="year">report year</param>
        private void LocationsYearReport(
          ReportCounterManager<LocationCounter> results,
          string year)
        {
            string writeName = $"StnReport_{year}_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write Annual Stn Report for {year}.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a single location general report to a file.
        /// </summary>
        /// <param name="results">results to write</param>
        /// <param name="location">report location </param>
        private void SingleLocationGeneralReport(
          ReportCounterManager<LocationCounter> results,
          string location)
        {
            string writeName = $"{location}_Report_Gen_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write General Stn Report for {location}.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a report for a single year and location.
        /// </summary>
        /// <param name="results">The results to report</param>
        /// <param name="year">The year of the report</param>
        /// <param name="location">The location of the report</param>
        private void SingleLocationYearReport(
          ReportCounterManager<LocationCounter> results,
          string year,
          string location)
        {
            string writeName = $"{location}_Report_{year}_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write Annual Stn Report for {location} in {year}.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a report for all classes.
        /// </summary>
        /// <param name="results">The results to report</param>
        private void ClassGeneralReportResults(
          ReportCounterManager<ClassCounter> results)
        {
            string writeName = $"ClsReport_Gen_{this.CurrentDate}.csv";
            string faultMessage = "ReportBuilder: Failed to write General Cls Report.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a report for a single year.
        /// </summary>
        /// <param name="results">The results to report</param>
        /// <param name="year">The year of the report</param>
        private void ClassSingleYearReportResults(
          ReportCounterManager<YearCounter> results,
          string year)
        {
            string writeName = $"ClsReport_{year}_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: Failed to write Annual Cls Report for {year}.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a report for a single class.
        /// </summary>
        /// <param name="results">The results to report</param>
        /// <param name="cls">The class of the report</param>
        private void SingleClassGeneralLocationReportResults(
          ReportCounterManager<LocationCounter> results,
          string cls)
        {
            string writeName = $"{cls}_Report_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: Failed to write General {cls} Report.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }

        /// <summary>
        /// Write a report for a single year and class.
        /// </summary>
        /// <param name="results">The results to report</param>
        /// <param name="year">The year of the report</param>
        /// <param name="cls">The class of the report</param>
        private void SingleClassSingleYearLocationReportResults(
          ReportCounterManager<LocationCounter> results,
          string year,
          string cls)
        {
            string writeName = $"{cls}_Report_{year}_{this.CurrentDate}.csv";
            string faultMessage = $"ReportBuilder: Failed to write Single Year {year} {cls} Report.";

            results.WriteCSVFile(
              writeName,
              faultMessage);
        }
    }
}