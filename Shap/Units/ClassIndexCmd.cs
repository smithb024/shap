namespace Shap.Units
{
  using System;
  using System.Windows.Input;
  using Shap.Units;

  public class ClassIndexCmd : ICommand
  {
    /// <summary>
    /// Creates a new instance of the <see cref="StnDetailsCmd"/> class
    /// </summary>
    /// <param name="command">command to run</param>
    /// <param name="commandMessage">information to put on command</param>
    public ClassIndexCmd(Action<string> command, string commandMessage)
    {
      this.RunCommand = command;
      this.CmdMessage = commandMessage;
    }

    /// <summary>
    /// Run command action.
    /// </summary>
    public Action<string> RunCommand
    {
      get;
      private set;
    }

    /// <summary>
    /// 
    /// </summary>
    public string CmdMessage
    {
      get;
      private set;
    }

    /// <summary>
    /// Always available
    /// </summary>
    /// <param name="parameter">not used</param>
    /// <returns>always true</returns>
    public bool CanExecute(object parameter)
    {
      return false;
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
      this.RunCommand(this.CmdMessage);
    }
  }
}