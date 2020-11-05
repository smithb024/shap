namespace Shap.StationDetails
{
  using System;
  using System.Windows.Input;
  using Shap.StationDetails;

  /// <summary>
  /// Command class for edit details view model.
  /// </summary>
  public class EditViewModelCmd : ICommand
  {
    private EditMileageViewModel viewModel = null;

    /// <summary>
    /// Creates a new instance of the <see cref="EditViewModelCmd"/> class
    /// </summary>
    /// <param name="viewModel">view model</param>
    public EditViewModelCmd(EditMileageViewModel viewModel, Action command)
      : this(viewModel, command, CanAlwaysRunCommand)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="EditViewModelCmd"/> class
    /// </summary>
    /// <param name="viewModel">view model</param>
    /// <param name="command">command to run</param>
    /// <param name="canExecuteCommand">indicates whether the command can be run</param>
    public EditViewModelCmd(
      EditMileageViewModel viewModel,
      Action command,
      Func<bool> canExecuteCommand)
    {
      this.viewModel = viewModel;
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

    /// <summary>
    /// Run the provided command
    /// </summary>
    /// <param name="parameter">unused parameter</param>
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