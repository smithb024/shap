namespace Shap
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using CommunityToolkit.Mvvm.DependencyInjection;
    using NynaeveLib.Logger;
    using Shap.Analysis.ViewModels;
    using Shap.Analysis.Windows;
    using Shap.Common.Commands;
    using Shap.Config;
    using Shap.Input;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Stats;
    using Shap.StationDetails;
    using Shap.Stats;
    using Shap.Units;
    using Shap.Units.IO;

    /// <summary>
    /// View model for the main window.
    /// </summary>
    public class MainWindowViewModel
    {
        InputForm inputWindow;
        MileageDetailsWindow jnyDetailsWindow;
        EditMileageWindow editMileageWindow;
        ClassIndexWindow classIndexWindow;
        AnalysisWindow analysisWindow;
        ConfigWindow configWindow;

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
        /// <param name="firstExamples">
        /// First examples manager
        /// </param>
        public MainWindowViewModel(
          IIoControllers controllers)
        {
            this.controllers = controllers;
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

        public ICommand AddEditJnyDetailsCommand { get; private set; }

        public ICommand AnalysisCommand { get; private set; }

        public ICommand ConfigurationCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }

        public ICommand OpenLogCommand { get; private set; }

        public ICommand OpenLogFolderCommand { get; private set; }

        /// <summary>
        /// Gets the command which is used to display the class index window.
        /// </summary>
        public ICommand ShowClassIndexCommand { get; private set; }

        /// <summary>
        /// Gets the command which is used to dispayed the location index window.
        /// </summary>
        public ICommand ShowLocationIndexCommand { get; private set; }

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
        /// Form closed, set to null.
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
                SetupWindow(
                  this.analysisWindow = new AnalysisWindow(),
                  new AnalysisViewModel(this.controllers),
                  CloseAnalysisWindow,
                  AnalysisWindowClosed);
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
                SetupWindow(
                  this.configWindow = new ConfigWindow(),
                  new ConfigViewModel(
                      this.controllers,
                      this.firstExamples),
                  CloseConfigurationWindow,
                  ConfigurationWindowClosed);
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

        public void ExitProgram()
        {
            Application.Current.Shutdown();
            //NynaeveLib.DialogService.DialogService service = new NynaeveLib.DialogService.DialogService();

            //MessageBoxResult result = service.ShowDialog("TestMessage");
        }

        public void ShowLogFolder()
        {
            Logger.Instance.OpenLogDirectory();
        }

        public void ShowLog()
        {
            Logger.Instance.OpenLogFile();
        }

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
        /// 
        /// </summary>
        public void ShowLocationIndexWindow()
        {

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