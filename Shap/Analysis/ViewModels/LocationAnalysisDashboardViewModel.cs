namespace Shap.Analysis.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;
    using Shap.Common;
    using Shap.Config;
    using Shap.StationDetails;

    /// <summary>
    /// View model describing a location analysis dashboard.
    /// </summary>
    public class LocationAnalysisDashboardViewModel : ViewModelBase
    {
        /// <summary>
        /// Index of the currently selected year.
        /// </summary>
        private int yearsIndex;

        /// <summary>
        /// Index of the short location collection.
        /// </summary>
        private int stnShortIndex;

        /// <summary>
        /// Index of the long location collection.
        /// </summary>
        private int stnAllIndex;

        /// <summary>
        /// Value indicating whether the location filter is on.
        /// </summary>
        private bool stnFilter;

        /// <summary>
        /// Value indicating whether the full list of names is displayed in the analysis or just
        /// relevant ones.
        /// </summary>
        private bool fullList;

        /// <summary>
        /// Action used to return the results for a location general report
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>> locationGeneralReportResults;

        /// <summary>
        /// Action used to return the results for a location general report (single year)
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>, string> locationYearReportResults;

        /// <summary>
        /// Action used to return the results for a single location report (all time)
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>, string> singleLocationGeneralReportResults;

        /// <summary>
        /// Action used to return the results for a single location report (single year)
        /// </summary>
        private Action<ReportCounterManager<LocationCounter>, string, string> singleLocationYearReportResults;

        /// <summary>
        /// 
        /// </summary>
        public LocationAnalysisDashboardViewModel(
          Action<ReportCounterManager<LocationCounter>> locationsGeneralReportResults,
          Action<ReportCounterManager<LocationCounter>, string> locationsYearReportResults,
          Action<ReportCounterManager<LocationCounter>, string> singleLocationGeneralReportResults,
          Action<ReportCounterManager<LocationCounter>, string, string> singleLocationYearReportResults)
        {
            this.locationGeneralReportResults = locationsGeneralReportResults;
            this.locationYearReportResults = locationsYearReportResults;
            this.singleLocationGeneralReportResults = singleLocationGeneralReportResults;
            this.singleLocationYearReportResults = singleLocationYearReportResults;

            this.StnGenYearReportCmd = new CommonCommand(this.LocationsGeneralReport);
            this.StnSingleYearReportCmd = new CommonCommand(this.LocationsSingleYearReport);
            this.StnGenStnReportCmd = new CommonCommand(this.SingleLocationGeneralReport);
            this.StnSingleStnReportCmd = new CommonCommand(this.SingleLocationYearReport);

            if (this.YearsCollection.Count > 0)
            {
                this.yearsIndex = this.YearsCollection.Count - 1;
            }

            if (this.GetStnAllCollection().Count > 0)
            {
                this.stnAllIndex = 0;
            }

            if (this.GetStnShortCollection().Count > 0)
            {
                this.stnShortIndex = 0;
            }

            this.fullList = false;
        }

        /// <summary>
        /// Gets or sets a way of returning the progress to interested parties.
        /// </summary>
        public event Action<string> ProgressEvent;

        /// <summary>
        /// Generate stn general report across all years.
        /// </summary>
        public ICommand StnGenYearReportCmd { get; private set; }

        /// <summary>
        /// Generate stn general report across all years.
        /// </summary>
        public ICommand StnSingleYearReportCmd { get; private set; }

        /// <summary>
        /// Generate stn general report across all stn.
        /// </summary>
        public ICommand StnGenStnReportCmd { get; private set; }

        /// <summary>
        /// Generate stn report for single stn.
        /// </summary>
        public ICommand StnSingleStnReportCmd { get; private set; }

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

                RaisePropertyChangedEvent(nameof(this.YearsIndex));
            }
        }

        /// <summary>
        /// Gets a collection of locations.
        /// </summary>
        public ObservableCollection<string> StnCollection =>
          this.StnFilter ?
          this.GetStnShortCollection() :
          this.GetStnAllCollection();

        /// <summary>
        /// Gets or sets the index of the currently selected location.
        /// </summary>
        public int StnIndex
        {
            get
            {
                return this.StnFilter ?
                  this.stnShortIndex :
                  this.stnAllIndex;
            }

            set
            {
                if (this.StnFilter)
                {
                    this.stnShortIndex = value;
                }
                else
                {
                    this.stnAllIndex = value;
                }

                RaisePropertyChangedEvent(nameof(this.StnIndex));
            }
        }

        /// <summary>
        /// Gets or sets the location filter flag.
        /// </summary>
        public bool StnFilter
        {
            get
            {
                return this.stnFilter;
            }

            set
            {
                this.stnFilter = value;
                this.RaisePropertyChangedEvent(nameof(this.StnFilter));
                this.RaisePropertyChangedEvent(nameof(this.StnCollection));
                this.RaisePropertyChangedEvent(nameof(this.StnIndex));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the full list of items should be included in the
        /// report.
        /// </summary>
        public bool FullList
        {
            get
            {
                return this.fullList;
            }

            set
            {
                this.fullList = value;
                this.RaisePropertyChangedEvent(nameof(this.FullList));
            }
        }

        /// <summary>
        /// Gets the collection of locations with the filter applied.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetStnShortCollection()
        {
            ObservableCollection<string> stnCollection = new ObservableCollection<string>();

            PopularStnIOController locationController = PopularStnIOController.GetInstance();
            List<string> locationList = locationController.LoadFile();

            foreach (string location in locationList)
            {
                stnCollection.Add(location);
            }

            return stnCollection;
        }

        /// <summary>
        /// Gets the collection of locations without the filter applied.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<string> GetStnAllCollection()
        {
            ObservableCollection<string> stnCollection = new ObservableCollection<string>();

            string previousvalue = string.Empty;
            string str = string.Empty;
            JourneyIOController jc = JourneyIOController.GetInstance();

            for (int i = 0; i < jc.GetMileageDetailsLength(); i++)
            {
                str = jc.GetFromStation(i);
                if (str != previousvalue)
                {
                    stnCollection.Add(str);
                }

                previousvalue = str;
            }

            return stnCollection;
        }

        /// <summary>
        /// Generate stn general report across all years.
        /// </summary>
        private void LocationsGeneralReport()
        {
            string description = $"Location General Report";

            this.ProgressEvent?.Invoke($"Started {description}");
            ReportCounterManager<LocationCounter> result = LocationReportFactory.RunStnGeneralReport();
            this.locationGeneralReportResults.Invoke(result);
            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate stn general report across all years.
        /// </summary>
        private void LocationsSingleYearReport()
        {
            string description = $"Location Report for {this.YearsCollection[this.YearsIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<LocationCounter> results =
              LocationReportFactory.RunStnAnnualReport(
                this.YearsCollection[this.YearsIndex],
                this.FullList);
            this.locationYearReportResults.Invoke(
              results,
              this.YearsCollection[this.YearsIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate stn general report across all stn.
        /// </summary>
        private void SingleLocationGeneralReport()
        {
            string description = $"Location Report for {this.StnCollection[this.StnIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<LocationCounter> results =
                  LocationReportFactory.RunSingleStnGeneralReport(
                 this.StnCollection[this.StnIndex],
                    this.FullList);
            this.singleLocationGeneralReportResults.Invoke(
              results,
              this.StnCollection[this.StnIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }

        /// <summary>
        /// Generate stn report for single stn.
        /// </summary>
        private void SingleLocationYearReport()
        {
            string description = $"Location Report for {this.StnCollection[this.StnIndex]} in {this.YearsCollection[this.YearsIndex]}";
            this.ProgressEvent?.Invoke($"Started {description}");

            ReportCounterManager<LocationCounter> results =
              LocationReportFactory.RunSingleStnAnnualReport(
                this.YearsCollection[this.YearsIndex],
                this.StnCollection[this.StnIndex],
                this.FullList);
            this.singleLocationYearReportResults.Invoke(
              results,
              this.YearsCollection[this.YearsIndex],
              this.StnCollection[this.StnIndex]);

            this.ProgressEvent?.Invoke($"Completed {description}");
        }
    }
}