namespace Shap
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using CommunityToolkit.Mvvm.DependencyInjection;
    using Interfaces;
    using NynaeveLib.Logger;
    using Shap.Analysis.ViewModels;
    using Shap.Analysis.Windows;
    using Shap.Common.Commands;
    using Shap.Config;
    using Shap.Input;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Stats;
    using Shap.Locations.Views;
    using Shap.StationDetails;
    using Shap.Units;

    /// <summary>
    /// View model for the main window.
    /// </summary>
    public class MainWindowViewModel : IMainWindowViewModel
    {
        /// <summary>
        /// The location manager
        /// </summary>
        private readonly ILocationManager locationManager;

        /// <summary>
        /// The instance of the <see cref="InputForm"/>.
        /// </summary>
        private InputForm inputWindow;

        /// <summary>
        /// The instance of the <see cref="MileageDetailsWindow"/>.
        /// </summary>
        private MileageDetailsWindow jnyDetailsWindow;

        /// <summary>
        /// The instance of the <see cref="EditMileageWindow"/>.
        /// </summary>
        private EditMileageWindow editMileageWindow;

        /// <summary>
        /// The instance of the <see cref="LocationsIndexWindow"/>.
        /// </summary>
        private LocationsIndexWindow locationsIndexWindow;

        /// <summary>
        /// The instance of the <see cref="ClassIndexWindow"/>.
        /// </summary>
        private ClassIndexWindow classIndexWindow;

        /// <summary>
        /// The instance of the <see cref="AnalysisWindow"/>.
        /// </summary>
        private AnalysisWindow analysisWindow;

        /// <summary>
        /// The instanceo of the <see cref="ConfigWindow"/>.
        /// </summary>
        private ConfigWindow configWindow;

        /// <summary>
        /// Collection of IO controllers
        /// </summary>
        private IIoControllers controllers;

        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private IFirstExampleManager firstExamples;

        /// <summary>
        /// Initialises a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="controllers">
        /// Factory containing IO controllers.
        /// </param>
        /// <param name="locationManager">
        /// The location manager
        /// </param>
        public MainWindowViewModel(
          IIoControllers controllers,
          ILocationManager locationManager)
        {
            this.controllers = controllers;
            this.locationManager = locationManager;
            this.firstExamples = Ioc.Default.GetService<IFirstExampleManager>();

            this.AddEditJnyDetailsCommand = new CommonCommand(this.ShowAddEditJnyDetailsWindow);
            this.AnalysisCommand = new CommonCommand(this.ShowAnalysisWindow);
            this.ConfigurationCommand = new CommonCommand(this.ShowConfigurationWindow);
            this.ExitCommand = new CommonCommand(this.ExitProgram);
            this.OpenLogCommand = new CommonCommand(this.ShowLog);
            this.OpenLogFolderCommand = new CommonCommand(this.ShowLogFolder);
            this.ShowClassIndexCommand = new CommonCommand(this.ShowClassIndexWindow);
            this.ShowLocationIndexCommand = new CommonCommand(this.ShowLocationIndexWindow);
            this.ShowJnyDetailsCommand = new CommonCommand(this.ShowJnyDetailsWindow);
            this.ShowInputDataCommand = new CommonCommand(this.ShowInputWindow);

            this.inputWindow = null;
        }

        /// <summary>
        /// Gets the command which opens the edit jny window.
        /// </summary>
        public ICommand AddEditJnyDetailsCommand { get; private set; }

        /// <summary>
        /// Gets the command which opens the analysis window.
        /// </summary>
        public ICommand AnalysisCommand { get; private set; }

        /// <summary>
        /// Gets the command which opens the configuration window.
        /// </summary>
        public ICommand ConfigurationCommand { get; private set; }

        /// <summary>
        /// Gets the command which exits the app.
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Gets the command which opens the log.
        /// </summary>
        public ICommand OpenLogCommand { get; private set; }

        /// <summary>
        /// Gets the command which opens the log folder in file explorer.
        /// </summary>
        public ICommand OpenLogFolderCommand { get; private set; }

        /// <summary>
        /// Gets the command which is used to display the class index window.
        /// </summary>
        public ICommand ShowClassIndexCommand { get; private set; }

        /// <summary>
        /// Gets the command which is used to dispayed the location index window.
        /// </summary>
        public ICommand ShowLocationIndexCommand { get; private set; }

        /// <summary>
        /// Gets the command which opens the jny details window.
        /// </summary>
        public ICommand ShowJnyDetailsCommand { get; private set; }

        /// <summary>
        /// Show the input form.
        /// </summary>
        public ICommand ShowInputDataCommand { get; private set; }

        /// <summary>
        /// Display the add edit window.
        /// </summary>
        public void ShowAddEditJnyDetailsWindow()
        {
            if (this.editMileageWindow == null)
            {
                this.editMileageWindow = new EditMileageWindow();
                this.SetupWindow(
                  this.editMileageWindow,
                  this.EditJnyDetailsWindowClosed);
            }

            this.editMileageWindow.Focus();
        }

        /// <summary>
        /// Associated view closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EditJnyDetailsWindowClosed(object sender, EventArgs e)
        {
            this.editMileageWindow.Closed -= this.EditJnyDetailsWindowClosed;
            this.editMileageWindow = null;
        }

        /// <summary>
        /// Show the analysis window. Create a new one if it doesn't exist.
        /// </summary>
        public void ShowAnalysisWindow()
        {
            if (this.analysisWindow == null)
            {
                this.SetupWindow(
                  this.analysisWindow = new AnalysisWindow(),
                  new AnalysisViewModel(this.controllers),
                  this.CloseAnalysisWindow,
                  this.AnalysisWindowClosed);
            }

            this.analysisWindow.Focus();
        }

        /// <summary>
        /// close the analysis window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseAnalysisWindow(object sender, EventArgs e)
        {
            this.analysisWindow.Close();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AnalysisWindowClosed(object sender, EventArgs e)
        {
            this.analysisWindow = null;
        }

        /// <summary>
        /// Show a configuration window. If none exists, then create one.
        /// </summary>
        public void ShowConfigurationWindow()
        {
            if (this.configWindow == null)
            {
                this.SetupWindow(
                  this.configWindow = new ConfigWindow(),
                  new ConfigViewModel(
                      this.controllers,
                      this.firstExamples),
                  this.CloseConfigurationWindow,
                  this.ConfigurationWindowClosed);
            }

            this.configWindow.Focus();
        }

        /// <summary>
        /// close the configuration window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseConfigurationWindow(object sender, EventArgs e)
        {
            this.configWindow.Close();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConfigurationWindowClosed(object sender, EventArgs e)
        {
            this.configWindow = null;
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        public void ExitProgram()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Show the log folder in file explorer.
        /// </summary>
        public void ShowLogFolder()
        {
            Logger.Instance.OpenLogDirectory();
        }

        /// <summary>
        /// Open the log file.
        /// </summary>
        public void ShowLog()
        {
            Logger.Instance.OpenLogFile();
        }

        /// <summary>
        /// Show the class index window.
        /// </summary>
        public void ShowClassIndexWindow()
        {
            if (this.classIndexWindow == null)
            {
                ClassIndexViewModel classIndexViewModel =
                  new ClassIndexViewModel(
                    this.controllers,
                    this.firstExamples);
                this.classIndexWindow = new ClassIndexWindow();

                this.SetupWindow(
                  this.classIndexWindow,
                  classIndexViewModel,
                  CloseClassIndexWindow,
                  ClassIndexWindowClosed);
            }

            this.classIndexWindow.Focus();
        }

        /// <summary>
        /// Show the location index window.
        /// </summary>
        public void ShowLocationIndexWindow()
        {
            if (this.locationsIndexWindow == null)
            {
                this.locationsIndexWindow = new LocationsIndexWindow();
                this.SetupWindow(
                  this.locationsIndexWindow,
                  this.LocationsIndexWindowClosed);
            }

            this.locationsIndexWindow.Focus();
        }

        /// <summary>
        /// Window has closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LocationsIndexWindowClosed(object sender, EventArgs e)
        {
            this.locationsIndexWindow.Closed -= this.LocationsIndexWindowClosed;
            this.locationsIndexWindow = null;
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseClassIndexWindow(object sender, EventArgs e)
        {
            this.classIndexWindow.Close();
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ClassIndexWindowClosed(object sender, EventArgs e)
        {
            this.classIndexWindow = null;
        }

        /// <summary>
        /// Show the Mileage details window.
        /// </summary>
        public void ShowJnyDetailsWindow()
        {
            if (this.jnyDetailsWindow == null)
            {
                this.jnyDetailsWindow = new MileageDetailsWindow();
                this.SetupWindow(
                    this.jnyDetailsWindow,
                    this.JnyDetailsWindowClosed);
            }

            this.jnyDetailsWindow.Focus();
        }

        /// <summary>
        /// The window has closed, release events and set to null.
        /// </summary>
        /// <param name="sender">the <see cref="MileageDetailsWindow"/></param>
        /// <param name="e">event arguments</param>
        public void JnyDetailsWindowClosed(object sender, EventArgs e)
        {
            this.jnyDetailsWindow.Closed -= this.JnyDetailsWindowClosed;
            this.jnyDetailsWindow = null;
        }

        /// <summary>
        /// Show the input jny data form.
        /// </summary>
        public void ShowInputWindow()
        {
            if (this.inputWindow == null)
            {
                this.inputWindow = new InputForm();
                this.SetupWindow(
                  this.inputWindow,
                  this.InputFormClosed);
            }

            this.inputWindow.Focus();
        }

        /// <summary>
        /// The window has closed, release events and set to null.
        /// </summary>
        /// <param name="sender">the <see cref="InputForm"/></param>
        /// <param name="e">event arguments</param>
        public void InputFormClosed(object sender, EventArgs e)
        {
            this.inputWindow.Closed -= this.JnyDetailsWindowClosed;
            this.inputWindow = null;
        }

        /// <summary>
        /// Setup and show a window.
        /// </summary>
        /// <param name="window">window to set up</param>
        /// <param name="viewModel">view model to assign to the view model</param>
        /// <param name="closedViewMethod">request from the view model to close the view</param>
        /// <param name="closedMethod">method to run when the window closes</param>
        private void SetupWindow(
          System.Windows.Window window,
          NynaeveLib.ViewModel.ViewModelBase viewModel,
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
        /// Setup and show a window.
        /// </summary>
        /// <param name="window">window to set up</param>
        /// <param name="closedMethod">method to run when the window closes</param>
        private void SetupWindow(
          Window window,
          EventHandler closedMethod)
        {
            window.Closed += closedMethod;
            window.Show();
            window.Activate();
        }
    }
}