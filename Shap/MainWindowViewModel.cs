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
    using Shap.Feedback.ViewModels;
    using Shap.Feedback.Windows;
    using Shap.Input;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Stats;
    using Shap.Locations.Views;
    using Shap.Messages;
    using Shap.StationDetails;
    using Shap.Types.Enum;
    using Shap.Units;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

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
        /// The instance of the <see cref="FeedbackWindow"/>.
        /// </summary>
        private FeedbackWindow feedbackWindow;

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
            this.OpenFeedbackCommand = new CommonCommand(this.ShowFeedbackCommand);
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
        /// Gets the command which opens the feedback window.
        /// </summary>
        public ICommand OpenFeedbackCommand { get; private set; }

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
            this.ShowWindowFeedback(WindowType.AddEditDistance, true);
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
            this.ShowWindowFeedback(WindowType.AddEditDistance, false);
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
            this.ShowWindowFeedback(WindowType.Analysis, true);
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
            this.ShowWindowFeedback(WindowType.Analysis, false);
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
            this.ShowWindowFeedback(WindowType.Configuration, true);
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
            this.ShowWindowFeedback(WindowType.Configuration, false);
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
            FeedbackMessage message =
              new FeedbackMessage(
                  FeedbackType.Command,
                  "Show log folder");
            NynaeveMessenger.Default.Send(message);
        }

        /// <summary>
        /// Open the log file.
        /// </summary>
        public void ShowLog()
        {
            Logger.Instance.OpenLogFile();
            FeedbackMessage message =
                new FeedbackMessage(
                    FeedbackType.Command,
                    "Show log file");
            NynaeveMessenger.Default.Send(message);
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
                  this.CloseClassIndexWindow,
                  this.ClassIndexWindowClosed);
            }

            this.classIndexWindow.Focus();
            this.ShowWindowFeedback(WindowType.ClassIndex, true);
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
            this.ShowWindowFeedback(WindowType.LocationIndex, true);
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
            this.ShowWindowFeedback(WindowType.LocationIndex, false);
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
            this.ShowWindowFeedback(WindowType.ClassIndex, false);
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
            this.ShowWindowFeedback(WindowType.DistanceDetails, true);
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
            this.ShowWindowFeedback(WindowType.DistanceDetails, false);
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
            this.ShowWindowFeedback(WindowType.DailyInput, true);
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
            this.ShowWindowFeedback(WindowType.DailyInput, false);
        }

        /// <summary>
        /// Show the feedback window. Create a new one if it doesn't exist.
        /// </summary>
        public void ShowFeedbackCommand()
        {
            if (this.feedbackWindow == null)
            {
                FeedbackViewModel viewModel = new FeedbackViewModel();
                this.feedbackWindow = new FeedbackWindow();

                this.SetupWindow(
                  this.feedbackWindow,
                  viewModel,
                  this.CloseFeedbackWindow,
                  this.FeedbackWindowClosed);
            }

            this.feedbackWindow.Focus();
            this.ShowWindowFeedback(WindowType.Feedback, true);
        }

        /// <summary>
        /// close the feedback window.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event arguments</param>
        public void CloseFeedbackWindow(object sender, EventArgs e)
        {
            this.feedbackWindow.Close();
        }

        /// <summary>
        /// Feedback window closed, set to null.
        /// </summary>
        /// <param name="sender">The <see cref="AnalysisWindow"/></param>
        /// <param name="e">Event arguments</param>
        public void FeedbackWindowClosed(object sender, EventArgs e)
        {
            this.feedbackWindow = null;
            this.ShowWindowFeedback(WindowType.Feedback, false);
        }

        /// <summary>
        /// Setup and show a window.
        /// </summary>
        /// <param name="window">window to set up</param>
        /// <param name="viewModel">view model to assign to the view model</param>
        /// <param name="closedViewMethod">request from the view model to close the view</param>
        /// <param name="closedMethod">method to run when the window closes</param>
        private void SetupWindow(
          Window window,
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

        /// <summary>
        /// Show navigation feedback.
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="isOpen"></param>
        private void ShowWindowFeedback(
            WindowType windowType,
            bool isOpen)
        {
            string messageType = isOpen ? "Open" : "Close";

            string windowName;

            switch (windowType)
            {
                case WindowType.AddEditDistance:
                    windowName = "Add/Edit Distance";
                    break;

                case WindowType.Analysis:
                    windowName = "Analysis";
                    break;

                case WindowType.ClassIndex:
                    windowName = "Class Index";
                    break;

                case WindowType.Configuration:
                    windowName = "Configuration";
                    break;

                case WindowType.DailyInput:
                    windowName = "Daily Input";
                    break;

                case WindowType.DistanceDetails:
                    windowName = "Distance Details";
                    break;

                case WindowType.Feedback:
                    windowName = "Feedback";
                    break;

                case WindowType.LocationIndex:
                    windowName = "Location Index";
                    break;

                default:
                    windowName = "Unknown Window";
                    break;
            }

            FeedbackMessage message =
                new FeedbackMessage(
                    FeedbackType.Navigation,
                    $"{messageType} {windowName} Window.");
            NynaeveMessenger.Default.Send(message);
        }
    }
}