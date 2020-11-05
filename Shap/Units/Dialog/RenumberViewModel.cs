namespace Shap.Units.Dialog
{
  using System.ComponentModel;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Windows.Input;

  using Base;

  using NynaeveLib.Commands;
  using NynaeveLib.DialogService.Interfaces;

  public class RenumberViewModel : SubClassSelectorViewModel
  {
    private int currentSubClassNumbersIndex;
    private int newSubClassListIndex;
    private int newNumber;
    private int totalNumberToChange;

    public RenumberViewModel(
      ObservableCollection<string> subClasses,
      List<ObservableCollection<int>> allNumbers)
      : base(subClasses)
    {
      this.AllNumbers = allNumbers;
      this.totalNumberToChange = 1;
      this.PropertyChanged += this.PropertyHasChanged;
      this.OkCmd = new CommonCommand<ICloseable>(this.SelectOk, this.CanSelectOk);
    }

    /// <summary>
    /// Get or sets all number in the class.
    /// </summary>
    public List<ObservableCollection<int>> AllNumbers { get; set; }

    /// <summary>
    /// Gets the selected sub class.
    /// </summary>
    public ObservableCollection<int> CurrentSubClassNumbersList
    {
      get
      {
        return this.AllNumbers[this.SubClassIndex];
      }
    }

    /// <summary>
    /// Gets or sets the current sub classes numbers index.
    /// </summary>
    public int CurrentSubClassNumbersIndex
    {
      get
      {
        return this.currentSubClassNumbersIndex;
      }

      set
      {
        this.currentSubClassNumbersIndex = value;
        this.RaisePropertyChangedEvent(nameof(this.NewNumbersDescriptionsList));
      }
    }

    /// <summary>
    /// Gets or sets the sub classes index for the destination number.
    /// </summary>
    public int NewSubClassListIndex
    {
      get
      {
        return this.newSubClassListIndex;
      }

      set
      {
        this.newSubClassListIndex = value;
      }
    }

    /// <summary>
    /// Gets or sets the first new number.
    /// </summary>
    public int NewNumber
    {
      get
      {
        return this.newNumber;
      }

      set
      {
        this.newNumber = value;
        this.RaisePropertyChangedEvent("NewNumbersDescriptionsList");
      }
    }

    /// <summary>
    /// Gets or sets the current sub classes numbers index.
    /// </summary>
    public int TotalNumberToChange
    {
      get
      {
        return this.totalNumberToChange;
      }

      set
      {
        this.totalNumberToChange = value;
        this.RaisePropertyChangedEvent("TotalNumberToChange");
        this.RaisePropertyChangedEvent("NewNumbersDescriptionsList");
      }
    }

    /// <summary>
    /// Gets the selected sub class.
    /// </summary>
    public ObservableCollection<string> NewNumbersDescriptionsList
    {
      get
      {
        ObservableCollection<string> descriptions = new ObservableCollection<string>();

        //for (int index = 0; index < this.TotalNumberToChange; ++index)
        //{
        //  descriptions.Add("string");
        //}

        for (int index = 0; index < this.TotalNumberToChange; ++index)
        {
          if (this.CurrentSubClassNumbersIndex + index < this.CurrentSubClassNumbersList.Count)
          {
            descriptions.Add(
              $"{CurrentSubClassNumbersList[CurrentSubClassNumbersIndex + index]} changed to {NewNumber + index}");
          }
        }

        return descriptions;
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
      return true;
    }

    /// <summary>
    /// A property has changed.
    /// </summary>
    /// <param name="sender">origin object</param>
    /// <param name="e">property changed event arguments</param>
    private void PropertyHasChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "SubClassIndex")
      {
        this.RaisePropertyChangedEvent("CurrentSubClassNumbersList");
      }
    }
  }
}
