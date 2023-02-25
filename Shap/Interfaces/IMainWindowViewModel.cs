namespace Shap.Interfaces
{
    using System;
    using System.Windows.Input;

    public interface IMainWindowViewModel
    {
        ICommand AddEditJnyDetailsCommand { get; }
        ICommand AnalysisCommand { get; }
        ICommand ConfigurationCommand { get; }
        ICommand ExitCommand { get; }
        ICommand OpenLogCommand { get; }
        ICommand OpenLogFolderCommand { get; }
        ICommand ShowClassIndexCommand { get; }
        ICommand ShowInputDataCommand { get; }
        ICommand ShowJnyDetailsCommand { get; }
        ICommand ShowLocationIndexCommand { get; }

        void AnalysisWindowClosed(object sender, EventArgs e);
        void ClassIndexWindowClosed(object sender, EventArgs e);
        void CloseAnalysisWindow(object sender, EventArgs e);
        void CloseClassIndexWindow(object sender, EventArgs e);
        void CloseConfigurationWindow(object sender, EventArgs e);
        void ConfigurationWindowClosed(object sender, EventArgs e);
        void EditJnyDetailsWindowClosed(object sender, EventArgs e);
        void ExitProgram();
        void InputFormClosed(object sender, EventArgs e);
        void JnyDetailsWindowClosed(object sender, EventArgs e);
        void LocationsIndexWindowClosed(object sender, EventArgs e);
        void ShowAddEditJnyDetailsWindow();
        void ShowAnalysisWindow();
        void ShowClassIndexWindow();
        void ShowConfigurationWindow();
        void ShowInputWindow();
        void ShowJnyDetailsWindow();
        void ShowLocationIndexWindow();
        void ShowLog();
        void ShowLogFolder();
    }
}