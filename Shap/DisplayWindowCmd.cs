namespace Shap
{
  using System;
  using System.Windows.Input;
  using Shap.Types.Enum;

  public class DisplayWindowCmd : ICommand
  {
    private MainWindowViewModel viewModel = null;

    /// <summary>
    /// Creates a new instance of the SeasonRegSaveCmd
    /// </summary>
    /// <param name="viewModel">view model</param>
    public DisplayWindowCmd(MainWindowViewModel viewModel, Action command)
    {
      this.viewModel = viewModel;
      RunCommand = command;
    }

    public Action RunCommand
    {
      get;
      private set;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      RunCommand();
    }
  }
}