namespace Shap.Units
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Interfaces.Io;
    using Shap.Types;
    using Common;
    using Stats;

    /// <summary>
    /// View model which supports a tile on the Class Index Window. A tile provides access to a 
    /// single named class. 
    /// </summary>
    public class IndexItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private FirstExampleManager firstExamples;

        /// <summary>
        /// The name of the class which is accessed by this tile.
        /// </summary>
        private string className;

        /// <summary>
        /// Indicates whether the view is in configuration mode. This will determine wither the 
        /// user can access the configuration window for the class, or the front page.
        /// </summary>
        private bool inConfigurationMode;

        /// <summary>
        /// Inidicates whether the icon should be displayed or not.
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// IO controllers.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// The <see cref="ClassConfigWindow"/> XAML object. Used for configuration.
        /// </summary>
        ClassConfigWindow classConfigWindow;

        /// <summary>
        /// The <see cref="ClassFrontPage"/> XAML object. Used for history.
        /// </summary>
        ClassFrontPage classFrontPageWindow;

        /// <summary>
        /// The family which this item belongs to.
        /// </summary>
        private string itemFamily;

        /// <summary>
        /// The family currently being filter on.
        /// </summary>
        private string familyFilter;

        /// <summary>
        /// Initialises a new instance of the <see cref="IndexItemViewModel"/> class.
        /// </summary>
        /// <param name="ioController">IO Controllers</param>
        /// <param name="name">class name</param>
        public IndexItemViewModel(
          IIoControllers ioControllers,
          FirstExampleManager firstExamples,
          string name)
        {
            this.ioControllers = ioControllers;
            this.firstExamples = firstExamples;
            this.className = name;
            this.inConfigurationMode = false;
            this.OpenWindowCmd = new CommonCommand(this.ShowClassWindow);
            this.isVisible = true;
            this.familyFilter = string.Empty;

            ClassDetails classFileConfiguration =
                    this.ioControllers.UnitsXml.Read(
                        name);
            this.itemFamily =
                classFileConfiguration?.Family
                ?? string.Empty;

            this.InService = classFileConfiguration?.ServiceType ?? VehicleServiceType.Withdrawn;
        }

        /// <summary>
        /// Gets or sets the index name.
        /// </summary>
        public string IndexName
        {
            get
            {
                return this.className;
            }

            set
            {
                this.className = value;
                this.RaisePropertyChangedEvent(nameof(this.IndexName));
                this.RaisePropertyChangedEvent(nameof(this.IndexImagePath));
            }
        }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        public string IndexImagePath
        {
            get
            {
                string returnString = BasePathReader.GetBasePathUri() +
                   StaticResources.classIconPath +
                   this.IndexName +
                   ".jpg";

                return returnString;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the class is in configuration mode or not.
        /// </summary>
        public bool InConfigurationMode
        {
            get
            {
                return this.inConfigurationMode;
            }

            set
            {
                this.inConfigurationMode = value;
                this.RaisePropertyChangedEvent("InConfigurationMode");
            }
        }

        /// <summary>
        /// Gets a value indicating what the service state of the class is.
        /// </summary>
        public VehicleServiceType InService { get; }

        /// <summary>
        /// Indicates whether the tile should be visible or not.
        /// </summary>
        public bool IsVisible => this.isVisible;

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand OpenWindowCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// Show a new window, which one depends on the mode.
        /// </summary>
        public void ShowClassWindow()
        {
            if (this.InConfigurationMode)
            {
                this.ShowClassConfigWindow();
            }
            else
            {
                this.ShowClassFrontPage();
            }
        }

        /// <summary>
        /// Show the new class config window. Manage it so multiple examples are not shown. If the 
        /// front page window exists, show that instead.
        /// </summary>
        public void ShowClassConfigWindow()
        {
            if (this.classFrontPageWindow != null)
            {
                this.classFrontPageWindow.Focus();
                return;
            }

            if (this.classConfigWindow == null)
            {
                ClassConfigViewModel classConfig =
                  new ClassConfigViewModel(
                    this.ioControllers,
                    this.className);

                this.classConfigWindow = new ClassConfigWindow();
                SetupWindow(
                    this.classConfigWindow = new ClassConfigWindow(),
                    classConfig,
                    this.CloseClassConfigWindow,
                    this.EditClassConfigWindowClosed);
                this.classConfigWindow.DataContext = classConfig;
            }

            this.classConfigWindow.Focus();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseClassConfigWindow(object sender, EventArgs e)
        {
            this.classConfigWindow.Close();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditClassConfigWindowClosed(object sender, EventArgs e)
        {
            this.classConfigWindow = null;
        }

        /// <summary>
        /// Show the new class front page window. Manage it so multiple examples are not shown. If
        /// the config window exists, show that instead.
        /// </summary>
        public void ShowClassFrontPage()
        {
            if (this.classConfigWindow != null)
            {
                this.classConfigWindow.Focus();
                return;
            }

            if (this.classFrontPageWindow == null)
            {
                this.classFrontPageWindow = new ClassFrontPage();
                ClassFunctionalViewModel classFunctionalViewModel =
                  new ClassFunctionalViewModel(
                    this.ioControllers,
                    this.firstExamples,
                    this.className);

                SetupWindow(
                  this.classFrontPageWindow,
                  classFunctionalViewModel,
                  this.CloseClassFrontPageWindow,
                  this.EditClassFrontPageClosed);
            }

            this.classFrontPageWindow.Focus();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseClassFrontPageWindow(object sender, EventArgs e)
        {
            this.classFrontPageWindow.Close();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditClassFrontPageClosed(object sender, EventArgs e)
        {
            this.classFrontPageWindow = null;
        }

        /// <summary>
        /// Setup and show a window.
        /// </summary>
        /// <param name="window">window to set up</param>
        /// <param name="viewModel">view model to assign to the view model</param>
        /// <param name="closedViewMethod">request from the view model to close the view</param>
        /// <param name="closedMethod">method to run when the window closes</param>
        public void SetupWindow(
          Window window,
          ViewModelBase viewModel,
          EventHandler closeViewMethod,
          EventHandler closedMethod)
        {
            window.DataContext = viewModel;

            viewModel.ClosingRequest += closeViewMethod;
            window.Closed += closedMethod;

            window.Show();
            window.Activate();
        }

        /// <summary>
        /// The family which is currently being filtered on.
        /// </summary>
        /// <param name="familyFilter">family being filter on</param>
        public void SetFamilyFilter(string familyFilter)
        {
            this.familyFilter = familyFilter;
            this.AnalyseFilters();
        }

        /// <summary>
        /// Determine whether this icon passes any filters which have been set and consequently
        /// whether this icon should be displayed.
        /// </summary>
        private void AnalyseFilters()
        {
            if (string.IsNullOrEmpty(this.familyFilter))
            {
                this.isVisible = true;
            }
            else
            {
                this.isVisible =
                    string.Compare(this.itemFamily, this.familyFilter) == 0;
            }
            
            this.RaisePropertyChangedEvent(nameof(this.isVisible));
        }
    }
}