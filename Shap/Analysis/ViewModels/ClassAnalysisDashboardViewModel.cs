namespace Shap.Analysis.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using NynaeveLib.ViewModel;
    using Shap.Common;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Io;
    using Shap.Types;

    /// <summary>
    /// View model describing a location analysis dashboard.
    /// </summary>
    public class ClassAnalysisDashboardViewModel : ViewModelBase
    {
        /// <summary>
        /// IO Controllers.
        /// </summary>
        private readonly IIoControllers controllers;

        /// <summary>
        /// Index of the currently selected year.
        /// </summary>
        private int yearsIndex;

        /// <summary>
        /// Index of current class selected.
        /// </summary>
        private int clsIndex;

        /// <summary>
        /// The list of known classes.
        /// </summary>
        List<GroupsType> classList;

        /// <summary>
        /// This list of known families.
        /// </summary>
        List<SingleFamily> familyList;

        /// <summary>
        /// Value indicating whether the full list of names is displayed in the analysis or just
        /// relevant ones.
        /// </summary>
        private bool fullList;

        /// <summary>
        /// Value indicating whether to use the families, or the default classes.
        /// </summary>
        private bool useFamilies;

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
            this.AllLocationsForSpecificClassReportCommand =
                new CommonCommand(
                    this.GenerateAllLocationsVisitedPerClassReport);
            this.AllLocationsForSpecificClassAndYearReportCommand =
                new CommonCommand(
                    this.GenerateAllLocationsVisitedPerClassReportPerYear);

            if (this.YearsCollection.Count > 0)
            {
                this.yearsIndex = this.YearsCollection.Count - 1;
            }

            // Set up ClsCollection
            this.ClsCollection = new ObservableCollection<string>();
            this.classList = this.controllers.Gac.LoadFile();

            foreach (GroupsType group in this.classList)
            {
                this.ClsCollection.Add(group.Name);
            }

            if (this.ClsCollection.Count > 0)
            {
                this.clsIndex = 0;
            }

            this.fullList = false;
            this.useFamilies = false;
            this.familyList = null;
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

                this.OnPropertyChanged("YearsIndex");
            }
        }

        /// <summary>
        /// Gets a collection of all registered classes.
        /// </summary>
        public ObservableCollection<string> ClsCollection { get; set; }

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

                this.OnPropertyChanged("ClsIndex");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the full list in the analysis.
        /// </summary>
        public bool FullList
        {
            get => this.fullList;
            set => this.SetProperty(ref this.fullList, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the full list in the analysis.
        /// </summary>
        public bool UseFamilies
        {
            get => this.useFamilies;

            set
            {
                if (this.useFamilies == value)
                {
                    return;
                }

                this.useFamilies = value;
                this.OnPropertyChanged(nameof(this.UseFamilies));
                this.ResetLists();
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

            List<string> classes = this.GetClasses();

            ReportCounterManager<LocationCounter> results =
              ClassReportFactory.RunReportForClasses(
                this.controllers,
                classes,
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

            List<string> classes = this.GetClasses();

            ReportCounterManager<LocationCounter> results =
              ClassReportFactory.RunReportForClasses(
                this.controllers,
                classes,
                this.FullList,
                this.YearsCollection[this.YearsIndex]);
            this.singleClassSingleYearLocationReportResults.Invoke(
              results,
              this.YearsCollection[this.YearsIndex],
              this.ClsCollection[this.ClsIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Update the <see cref="ClsCollection"/> to use the collection which has been indicated
        /// by <see cref="UseFamilies"/>.
        /// </summary>
        private void ResetLists()
        {
            if (this.familyList == null)
            {
                FamilyDetails details = controllers.Family.Read();

                if (details != null && details.Families != null)
                {
                    this.familyList = details.Families;
                }
                else
                {
                    return;
                }
            }

            this.ClsCollection.Clear();

            if (this.useFamilies)
            {
                foreach (SingleFamily family in this.familyList)
                {
                    this.ClsCollection.Add(family.Name);
                }
            }
            else
            {
                foreach (GroupsType group in this.classList)
                {
                    this.ClsCollection.Add(group.Name);
                }
            }

            this.ClsIndex = this.ClsCollection.Count >= 0 ? 0 : -1;
        }

        /// <summary>
        /// Determine which classes have been selected.
        /// </summary>
        /// <returns>
        /// The selected classes.
        /// </returns>
        private List<string> GetClasses()
        {
            List<string> classes = new List<string>();

            if (this.useFamilies)
            {
                foreach (SingleClass cls in this.familyList[this.ClsIndex].Classes)
                {
                    classes.Add(cls.Name);
                }
            }
            else
            {
                classes.Add(this.ClsCollection[this.ClsIndex]);
            }

            return classes;
        }
    }
}