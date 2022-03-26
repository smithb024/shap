namespace Shap.Analysis.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using NynaeveLib.ViewModel;
    using Shap.Common;
    using Shap.Common.Commands;
    using Shap.Config;
    using Shap.Interfaces.Io;
    using Shap.Types;
    using Windows;

    /// <summary>
    /// View model describing a location analysis dashboard.
    /// </summary>
    public class ClassAnalysisDashboardViewModel : ViewModelBase
    {
        /// <summary>
        /// Index of the currently selected year.
        /// </summary>
        private int yearsIndex;

        /// <summary>
        /// Index of current class selected.
        /// </summary>
        private int clsIndex;

        /// <summary>
        /// Value indicating whether the full list of names is displayed in the analysis or just
        /// relevant ones.
        /// </summary>
        private bool fullList;

        /// <summary>
        /// IO Controllers.
        /// </summary>
        private IIoControllers controllers;

        /// <summary>
        /// Action used to return the results of an all classes general report.
        /// </summary>
        private Action<ReportCounterManager<ClassCounter>> classGeneralReportResults;

        /// <summary>
        /// Action used to return the results of an all classes, single year report.
        /// </summary>
        private Action<ReportCounterManager<YearCounter>, string> classSingleYearReportResults;

        /// <summary>
        /// Action used to return the results of a single class, general location report.
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>, string> singleClassGeneralLocationReportResults;

        /// <summary>
        /// Action used to return the results of a single class, single year location report.
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>, string, string> singleClassSingleYearLocationReportResults;

        /// <summary>
        /// Initialises a new instance of the <see cref="AnalysisViewModel"/> class.
        /// </summary>
        public ClassAnalysisDashboardViewModel(
          IIoControllers controllers,
          Action<ReportCounterManager<ClassCounter>> classGeneralReportResults,
          Action<ReportCounterManager<YearCounter>, string> classSingleYearReportResults,
          Action<ReportCounterManager<LocationCounter>, string> singleClassGeneralLocationReportResults,
          Action<ReportCounterManager<LocationCounter>, string, string> singleClassSingleYearLocationReportResults)
        {
            this.controllers = controllers;

            this.classGeneralReportResults = classGeneralReportResults;
            this.classSingleYearReportResults = classSingleYearReportResults;
            this.singleClassGeneralLocationReportResults = singleClassGeneralLocationReportResults;
            this.singleClassSingleYearLocationReportResults = singleClassSingleYearLocationReportResults;

            this.AllRunsPerClassCommand = new CommonCommand(this.GenerateAllRunsPerClassReport);
            this.ClsSingleYearReportCmd = new CommonCommand(this.GenerateClassSingleYearReport);
            this.AllLocationsForSpecificClassReportCommand = new CommonCommand(this.GenerateAllLocationsVisitedPerClassReport);
            this.AllLocationsForSpecificClassAndYearReportCommand = new CommonCommand(this.GenerateAllLocationsVisitedPerClassReportPerYear);

            if (this.YearsCollection.Count > 0)
            {
                this.yearsIndex = this.YearsCollection.Count - 1;
            }

            if (this.ClsCollection.Count > 0)
            {
                this.clsIndex = 0;
            }

            this.fullList = false;
        }

        /// <summary>
        /// Gets or sets a way of returning the progress to interested parties.
        /// </summary>
        public Action<string> ProgressEvent { get; set; }

        /// <summary>
        /// Generate cls general report across all years.
        /// </summary>
        public ICommand AllRunsPerClassCommand { get; private set; }

        /// <summary>
        /// Generate cls report for single year.
        /// </summary>
        public ICommand ClsSingleYearReportCmd { get; private set; }

        /// <summary>
        /// Generate general cls report.
        /// </summary>
        public ICommand AllLocationsForSpecificClassReportCommand { get; private set; }

        /// <summary>
        /// Generate cls report for single cls.
        /// </summary>
        public ICommand AllLocationsForSpecificClassAndYearReportCommand { get; private set; }

        /// <summary>
        /// Gets a collection of all years within the data set.
        /// </summary>
        public ObservableCollection<string> YearsCollection
        {
            get
            {
                ObservableCollection<string> years = new ObservableCollection<string>();

                string[] dirNamesArray =
                  System.IO.Directory.GetDirectories(
                  BasePathReader.GetBasePath() + StaticResources.baPath);

                // TODO there should be a function in nyneave lib for this.
                foreach (string directories in dirNamesArray)
                {
                    years.Add(directories.Substring(directories.LastIndexOf('\\') + 1));
                }

                return years;
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected year.
        /// </summary>
        public int YearsIndex
        {
            get
            {
                return this.yearsIndex;
            }

            set
            {
                this.yearsIndex = value;

                RaisePropertyChangedEvent("YearsIndex");
            }
        }

        /// <summary>
        /// Gets a collection of all registered classes.
        /// </summary>
        public ObservableCollection<string> ClsCollection
        {
            get
            {
                ObservableCollection<string> clsCollection = new ObservableCollection<string>();

                List<GroupsType> groupsList = this.controllers.Gac.LoadFile();

                foreach (GroupsType group in groupsList)
                {
                    clsCollection.Add(group.Name);
                }

                return clsCollection;
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected class.
        /// </summary>
        public int ClsIndex
        {
            get
            {
                return this.clsIndex;
            }

            set
            {
                this.clsIndex = value;

                RaisePropertyChangedEvent("ClsIndex");
            }
        }

        public bool FullList
        {
            get
            {
                return this.fullList;
            }

            set
            {
                this.fullList = value;
                this.RaisePropertyChangedEvent("FullList");
            }
        }

        /// <summary>
        /// Generate a report containing a count of all runs across all classes and across all years.
        /// </summary>
        private void GenerateAllRunsPerClassReport()
        {
            string description = "All Class Runs Report";

            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<ClassCounter> results =
              ClassReportFactory.RunGeneralReportForAllCls(
                this.controllers,
                this.FullList);
            this.classGeneralReportResults.Invoke(results);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate cls report for single year.
        /// </summary>
        private void GenerateClassSingleYearReport()
        {
            string description = $"Class Report for {this.YearsCollection[this.YearsIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<YearCounter> results =
              ClassReportFactory.RunYearReportForAllCls(
                this.controllers,
                this.YearsCollection[this.YearsIndex],
                this.FullList);
            this.classSingleYearReportResults.Invoke(
              results,
              this.YearsCollection[this.YearsIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate a report which documents a count of all locations visited by a specific class.
        /// </summary>
        private void GenerateAllLocationsVisitedPerClassReport()
        {
            string description = $"Class Location Report for {this.ClsCollection[this.clsIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<LocationCounter> results =
              ClassReportFactory.RunReportForASingleClass(
                this.controllers,
                this.ClsCollection[this.ClsIndex],
                this.FullList);
            this.singleClassGeneralLocationReportResults.Invoke(
              results,
                this.ClsCollection[this.ClsIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate a report which documents a count of all locations visited by a specific class in a given year.
        /// </summary>
        private void GenerateAllLocationsVisitedPerClassReportPerYear()
        {
            string description = $"Class Location Report for {this.ClsCollection[this.clsIndex]} in {this.YearsCollection[this.YearsIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<LocationCounter> results =
              ClassReportFactory.RunReportForASingleClass(
                this.controllers,
                this.ClsCollection[this.ClsIndex],
                this.FullList,
                this.YearsCollection[this.YearsIndex]);
            this.singleClassSingleYearLocationReportResults.Invoke(
              results,
              this.YearsCollection[this.YearsIndex],
              this.ClsCollection[this.ClsIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }
    }
}