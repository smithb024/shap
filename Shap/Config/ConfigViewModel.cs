namespace Shap.Config
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows.Input;

    using System.ComponentModel;
    using System.Windows;

    using Interfaces.Stats;
    using NynaeveLib.DialogService;
    using NynaeveLib.Logger;
    using NynaeveLib.ViewModel;
    using Shap.Common;
    using Shap.Common.Commands;
    using Shap.Interfaces.Io;
    using Shap.Stats;

    /// <summary>
    /// View model class for the configuration dialog.
    /// </summary>
    public class ConfigViewModel : ViewModelBase
    {
        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private IFirstExampleManager firstExamples;

        /// <summary>
        /// Collection of all years with data.
        /// </summary>
        ObservableCollection<string> years;

        /// <summary>
        /// Index of the currently selected year.
        /// </summary>
        int yearsIndex;

        /// <summary>
        /// Number of old numbers to manage.
        /// </summary>
        int oldNumbersCount;

        /// <summary>
        /// Proposed number of old numbers to manage.
        /// </summary>
        int oldNumbersCountUpdate;

        /// <summary>
        /// Defines the current operation status.
        /// </summary>
        string status;

        /// <summary>
        /// IO Controllers.
        /// </summary>
        IIoControllers ioControllers;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO Controllers</param>
        /// <param name="firstExamples">first example manager</param>
        public ConfigViewModel(
          IIoControllers ioControllers,
          IFirstExampleManager firstExamples)
        {
            this.ioControllers = ioControllers;
            this.firstExamples = firstExamples;
            this.PopulateYearCollection();
            this.PopulateOldNumbersAvailable();

            if (this.YearsCollection.Count > 0)
            {
                this.yearsIndex = this.YearsCollection.Count - 1;
            }

            this.CloseCmd = new CommonCommand(this.CloseWindow);
            this.RefreshYearCmd = new CommonCommand(this.RefreshYear);
            this.RefreshAllCmd = new CommonCommand(this.RefreshAll);
            this.ConfigClsCmd = new CommonCommand(this.ConfigCls);
            this.ConfigStnCmd = new CommonCommand(this.ConfigStn);
            this.UpdatePreviousIdCountCmd = new CommonCommand(this.UpdatePreviousIdCount);
        }

        /// <summary>
        /// Generate refresh first examples for a single year
        /// </summary>
        public ICommand RefreshYearCmd { get; private set; }

        /// <summary>
        /// Generate refresh first examples for a all years
        /// </summary>
        public ICommand RefreshAllCmd { get; private set; }

        /// <summary>
        /// Generate refresh first examples for a all years
        /// </summary>
        public ICommand UpdatePreviousIdCountCmd { get; private set; }

        /// <summary>
        /// Generate refresh first examples for a all years
        /// </summary>
        public ICommand ConfigStnCmd { get; private set; }

        /// <summary>
        /// Generate refresh first examples for a all years
        /// </summary>
        public ICommand ConfigClsCmd { get; private set; }

        /// <summary>
        /// Close the current window.
        /// </summary>
        public ICommand CloseCmd { get; private set; }

        /// <Date>17/11/18</Date>
        /// <summary>
        /// Gets or sets the current operation status.
        /// </summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                RaisePropertyChangedEvent(nameof(this.Status));
            }
        }

        /// <summary>
        /// Gets the collection of all years with data.
        /// </summary>
        public ObservableCollection<string> YearsCollection
        {
            get
            {
                return this.years;
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
        /// Gets or sets the number of old numbers to manage.
        /// </summary>
        public int OldNumbersCount
        {
            get
            {
                return this.oldNumbersCount;
            }

            set
            {
                this.oldNumbersCount = value;
                RaisePropertyChangedEvent("OldNumbersCount");
            }
        }

        /// <summary>
        /// Gets or sets the proposed number of old numbers to manage.
        /// </summary>
        public int OldNumbersCountUpdate
        {
            get
            {
                return this.oldNumbersCountUpdate;
            }

            set
            {
                this.oldNumbersCountUpdate = value;
                RaisePropertyChangedEvent("OldNumbersCountUpdate");
            }
        }

        /// <summary>
        /// The dialog is closing.
        /// </summary>
        private void CloseWindow()
        {
            this.OnClosingRequest();
        }

        /// <summary>
        /// Determine the all the years with data.
        /// </summary>
        private void PopulateYearCollection()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(BasePathReader.GetBasePath() + StaticResources.baPath);
            DirectoryInfo[] subdirs = dirInfo.GetDirectories();
            this.years = new ObservableCollection<string>();

            foreach (DirectoryInfo di in subdirs)
            {
                this.years.Add(di.ToString());
            }
        }

        /// <summary>
        /// TODO This method always returns 0 because this file doesn't exist.
        /// </summary>
        private void PopulateOldNumbersAvailable()
        {
            // populate the oldNumberAvailable label
            if (File.Exists(BasePathReader.GetBasePath() +
                            StaticResources.miscellaneousPath +
                            StaticResources.FileNameOldNumbersAvailable))
            {
                using (StreamReader sr = new StreamReader(BasePathReader.GetBasePath() +
                                                          StaticResources.miscellaneousPath +
                                                          StaticResources.FileNameOldNumbersAvailable))
                {
                    int prevNumbersAvail = 0;
                    if (int.TryParse(sr.ReadLine(), out prevNumbersAvail))
                    {
                        this.oldNumbersCount = prevNumbersAvail;
                        return;
                    }
                }
            }

            oldNumbersCount = 0;
        }

        /// <summary>
        /// Refresh the data for the named year.
        /// </summary>
        private void RefreshYear()
        {
            this.Status = "Refresh Year Working";

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerCompleted += WorkerCompleted;

            worker.RunWorkerAsync(ConfigJobType.RefreshYear);
        }

        /// <summary>
        /// Refresh all data.
        /// </summary>
        private void RefreshAll()
        {
            this.Status = "Search All Working";

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerDoWork;
            worker.RunWorkerCompleted += WorkerCompleted;

            worker.RunWorkerAsync(ConfigJobType.RefreshAll);
        }

        /// <Date>17/11/18</Date>
        /// <summary>
        /// Set up and show the dialog for managing the groups and classes.
        /// </summary>
        private void ConfigCls()
        {
            GroupsAndClassesViewModel groupsViewModel =
              new GroupsAndClassesViewModel(
                this.ioControllers);

            DialogService service = new DialogService();

            service.ShowDialog(
              new GroupsAndClassesDialog()
              {
                  DataContext = groupsViewModel
              });
        }

        /// <summary>
        /// Setup and show the dialog for managing the locations.
        /// </summary>
        private void ConfigStn()
        {
            PopularStnConfigViewModel popularViewModel = new PopularStnConfigViewModel();

            DialogService service = new DialogService();

            service.ShowDialog(
              new PopularStnConfigDialog()
              {
                  DataContext = popularViewModel
              });
        }

        /// <summary>
        /// TODO, this file doesn't exist.
        /// </summary>
        private void UpdatePreviousIdCount()
        {
            if (this.OldNumbersCount > this.OldNumbersCountUpdate)
            {
                DialogService dialogService = new DialogService();
                if (dialogService.ShowDialog("New value is less then the old one, this may cause loss of data. Confirm?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    if (WriteFile(BasePathReader.GetBasePath() +
                                  StaticResources.miscellaneousPath +
                                  StaticResources.FileNameOldNumbersAvailable,
                                  this.OldNumbersCountUpdate.ToString(),
                                  false))
                    {
                        this.OldNumbersCount = this.OldNumbersCountUpdate;
                    }
                }
            }
            else
            {
                if (WriteFile(BasePathReader.GetBasePath() +
                              StaticResources.miscellaneousPath +
                              StaticResources.FileNameOldNumbersAvailable,
                              this.OldNumbersCountUpdate.ToString(),
                              false))
                {
                    this.OldNumbersCount = this.OldNumbersCountUpdate;
                }
            }
            //    }
            //  }
            //}
        }

        /// <summary>
        ///  TODO, not currently used.
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="input">value to write</param>
        /// <param name="append">append flag</param>
        /// <returns>success flag</returns>
        private bool WriteFile(
          string path,
          string input,
          bool append)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }

                using (StreamWriter sw = new StreamWriter(path, append))
                {
                    sw.WriteLine(input.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                  $"ERROR: ConfigForm: Filed to write {path}: {ex.ToString()}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Do a job on the background worker thread.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ConfigJobType jobType = (ConfigJobType)e.Argument;
            string returnString = "return";

            if (jobType == ConfigJobType.RefreshAll)
            {
                firstExamples.RunSearchAll();
                returnString = "Refresh All";
            }
            else if (jobType == ConfigJobType.RefreshYear)
            {
                firstExamples.RunSearchYear(this.YearsCollection[this.YearsIndex]);
                returnString = "Refresh Year";
            }

            e.Result = returnString;
        }

        /// <summary>
        /// The background worker thread has completed.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string result = (string)e.Result;
            this.Status = $"{result} Complete";
        }
    }
}