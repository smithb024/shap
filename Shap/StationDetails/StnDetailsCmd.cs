namespace Shap.StationDetails
{
  using System;
  using System.Windows.Input;
  using Shap.StationDetails;

  /// <summary>
  /// Command class for stn details view model.
  /// </summary>
  public class StnDetailsCmd : ICommand
  {
    private MileageDetailsViewModel viewModel = null;

    /// <summary>
    /// Creates a new instance of the <see cref="StnDetailsCmd"/> class
    /// </summary>
    /// <param name="viewModel">view model</param>
    public StnDetailsCmd(MileageDetailsViewModel viewModel, Action command)
    {
      this.viewModel = viewModel;
      RunCommand = command;
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
    /// Always available
    /// </summary>
    /// <param name="parameter">not used</param>
    /// <returns>always true</returns>
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