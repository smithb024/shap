namespace Shap.Units.Dialog
{
  using System.Collections.ObjectModel;
  using System.Windows.Input;

  using Base;

  using NynaeveLib.Commands;
  using NynaeveLib.DialogService.Interfaces;

  public class NewNumberViewModel : SubClassSelectorViewModel
  {
    private int number;
    private int upperNumber;
    private bool addMultiple;

    public NewNumberViewModel(
      ObservableCollection<string> subClasses,
      bool addMultiple = false)
      : base (subClasses)
    {
      this.addMultiple = addMultiple;

      this.OkCmd = new CommonCommand<ICloseable>(this.SelectOk, this.CanSelectOk);
    }

    /// <summary>
    /// Gets or sets the new number.
    /// </summary>
    public int Number
    {
      get
      {
        return this.number;
      }

      set
      {
        this.number = value;
        this.RaisePropertyChangedEvent("Number");
        this.RaisePropertyChangedEvent("Status");
      }
    }

    /// <summary>
    /// Gets or sets the new upper number.
    /// </summary>
    public int UpperNumber
    {
      get
      {
        return this.upperNumber;
      }

      set
      {
        this.upperNumber = value;
        this.RaisePropertyChangedEvent("UpperNumber");
        this.RaisePropertyChangedEvent("Status");
      }
    }

    /// <summary>
    /// Gets any information for the user.
    /// </summary>
    public string Status
    {
      get
      {
        if (this.addMultiple && this.NumbersValid())
        {
          int totalToAdd = this.UpperNumber - this.Number + 1;
          return $"{totalToAdd} numbers to add";
        }

        return string.Empty;
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
      window?.CloseObject();
    }

    /// <summary>
    /// Checks to see if Ok can be selected.
    /// </summary>
    /// <returns>can only select if not null or empty</returns>
    private bool CanSelectOk(ICloseable window)
    {
      if (!this.addMultiple)
      {
        return true;
      }

      return this.NumbersValid();
    }

    /// <summary>
    /// Ensure that the number range is ok.
    /// </summary>
    /// <returns>numbers valid flag</returns>
    private bool NumbersValid()
    {
      return this.UpperNumber > this.Number;
    }
  }
}