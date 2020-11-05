namespace Shap.Units.Dialog
{
  using System.Windows;
  using System.Windows.Input;
  using NynaeveLib.Commands;
  using NynaeveLib.DialogService.Interfaces;
  using NynaeveLib.ViewModel;

  public class NewSubClassViewModel : ViewModelBase
  {
    private string subClass;
    private MessageBoxResult result;

    /// <summary>
    /// Initialises a  new instance of the <see cref="NewSubClassViewModel"/> class.
    /// </summary>
    public NewSubClassViewModel()
    {
      this.subClass = string.Empty;
      this.OkCmd = new CommonCommand<ICloseable>(this.SelectOk, this.CanSelectOk);
    }

    /// <summary>
    /// Gets or sets the new sub class.
    /// </summary>
    public string SubClass
    {
      get
      {
        return this.subClass;
      }

      set
      {
        this.subClass = value;
        this.RaisePropertyChangedEvent("SubClass");
      }
    }

    public MessageBoxResult Result
    {
      get
      {
        return this.result;
      }

      set
      {
        this.result = value;
      }
    }

    /// <summary>
    /// Ok command.
    /// </summary>
    public ICommand OkCmd
    {
      get;
      private set;
    }

    /// <summary>
    /// Select the Ok command.
    /// </summary>
    private void SelectOk(ICloseable window)
    {
      this.Result = MessageBoxResult.OK;
      window?.CloseObject();
    }

    /// <summary>
    /// Checks to see if Ok can be selected.
    /// </summary>
    /// <returns>can only select if not null or empty</returns>
    private bool CanSelectOk(ICloseable window)
    {
      return !string.IsNullOrEmpty(this.SubClass);
    }
  }
}
