/// <summary>
/// Really should be using the nynaeve lib one.
/// </summary>
namespace Shap.Common.Commands
{
  using System;
  using System.Windows.Input;

  public class CommonCommand : ICommand
  {
    public CommonCommand(Action command)
      : this (command, CanAlwaysRunCommand)
    {
    }

    public CommonCommand(
      Action command,
      Func<bool> canExecuteCommand)
    {
      this.RunCommand = command;
      this.CanRunCommand = canExecuteCommand;
    }

    /// <summary>
    /// Run command action.
    /// </summary>
    public Action RunCommand
    {
      get;
      private set;
    }

    /// <summary>
    /// Can run command function.
    /// </summary>
    public Func<bool> CanRunCommand
    {
      get;
      private set;
    }

    /// <summary>
    /// Determine if the command can be run.
    /// </summary>
    /// <param name="parameter">not used</param>
    /// <returns>always true</returns>
    public bool CanExecute(object parameter)
    {
      return CanRunCommand();
    }

    /// <summary>
    /// Can execute changed event.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      RunCommand();
    }

    /// <summary>
    /// Method which always returns true.
    /// </summary>
    /// <returns>always true</returns>
    private static bool CanAlwaysRunCommand()
    {
      return true;
    }
  }
}
