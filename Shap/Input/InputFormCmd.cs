namespace Shap.Input
{
  using System;
  using System.Windows.Input;
  using Shap.Types.Enum;

  public class InputFormCmd : ICommand
  {
    private InputFormViewModel viewModel = null;

    /// <summary>
    /// Creates a new instance of the <see cref="InputFormCmd"/> class
    /// </summary>
    /// <param name="viewModel">view model</param>
    public InputFormCmd(InputFormViewModel viewModel, Action command)
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