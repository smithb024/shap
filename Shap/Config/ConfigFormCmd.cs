namespace Shap.Config
{
  using System;
  using System.Windows.Input;

  public class ConfigFormCmd : ICommand
  {
    private ConfigViewModel viewModel = null;

    /// <summary>
    /// Creates a new instance of the <see cref="ConfigFormCmd"/> class
    /// </summary>
    /// <param name="viewModel">view model</param>
    public ConfigFormCmd(ConfigViewModel viewModel, Action command)
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

    /// <summary>
    /// Run the provided command
    /// </summary>
    /// <param name="parameter">unused parameter</param>
    public void Execute(object parameter)
    {
      RunCommand();
    }
  }
}