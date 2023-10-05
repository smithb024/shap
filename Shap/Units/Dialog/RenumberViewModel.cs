namespace Shap.Units.Dialog
{
    using System.ComponentModel;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Base;

    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using Shap.Common.SerialiseModel.ClassDetails;
    using System.Windows;

    /// <summary>
    /// View model which supports the renumber dialog.
    /// </summary>
    public class RenumberViewModel : SubClassSelectorViewModel
    {
        /// <summary>
        /// Serialisation of the class configuration file.
        /// </summary>
        private ClassDetails classFileConfiguration;

        private int currentSubClassNumbersIndex;
        private int newSubClassListIndex;
        private int newNumber;
        private int totalNumberToChange;

        /// <summary>
        /// Initialises a new instance of the <see cref="RenumberViewModel"/> class.
        /// </summary>
        /// <param name="subClasses">
        /// The subclasses available to the class.
        /// </param>
        /// <param name="classFileConfiguration">
        /// Class configuration file contents
        /// </param>
        public RenumberViewModel(
          ObservableCollection<string> subClasses,
          ClassDetails classFileConfiguration)
          : base(subClasses)
        {
            this.classFileConfiguration = classFileConfiguration;
            this.totalNumberToChange = 0;
            this.Result = MessageBoxResult.Cancel;
            this.PropertyChanged += this.PropertyHasChanged;
            this.OkCmd = new CommonCommand<ICloseable>(this.SelectOk, this.CanSelectOk);
            this.CurrentSubClassNumbersList = new ObservableCollection<int>();
            this.PopulateCurrentSubClassNumbers();
        }

        /// <summary>
        /// Gets or sets the result of this dialog.
        /// </summary>
        public MessageBoxResult Result { get; set; }

        /// <summary>
        /// Gets the selected sub class.
        /// </summary>
        public ObservableCollection<int> CurrentSubClassNumbersList { get; }

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
                this.OnPropertyChanged(nameof(this.NewNumbersDescriptionsList));
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
        /// Gets the first number to change.
        /// </summary>
        public string OriginalSubClass =>
            this.SubClassIndex >= 0 && this.SubClassIndex < this.SubClasses.Count
            ? this.SubClasses[this.SubClassIndex]
            : string.Empty;

        /// <summary>
        /// Gets the sub class to move the numbers to.
        /// </summary>
        public string NewSubClass =>
            this.NewSubClassListIndex >= 0 && this.NewSubClassListIndex < this.SubClasses.Count
            ? this.SubClasses[this.NewSubClassListIndex]
            : string.Empty;

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
                this.OnPropertyChanged("NewNumbersDescriptionsList");
            }
        }

        /// <summary>
        /// Gets the first number to change.
        /// </summary>
        public int OriginalNumber =>
            this.CurrentSubClassNumbersIndex >= 0 && this.CurrentSubClassNumbersIndex < this.CurrentSubClassNumbersList.Count
            ? this.CurrentSubClassNumbersList[this.CurrentSubClassNumbersIndex]
            : 0;

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
                this.OnPropertyChanged("TotalNumberToChange");
                this.OnPropertyChanged("NewNumbersDescriptionsList");
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
            this.Result = MessageBoxResult.OK;
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
            if (e.PropertyName == nameof(this.SubClassIndex))
            {
                this.PopulateCurrentSubClassNumbers();
                this.OnPropertyChanged(nameof(this.CurrentSubClassNumbersList));
            }
        }

        /// <summary>
        /// Populate the <see cref="CurrentSubClassNumbersList"/> property with all the numbers
        /// from the currently selected subclass.
        /// </summary>
        private void PopulateCurrentSubClassNumbers()
        {
            this.CurrentSubClassNumbersList.Clear();

            Subclass subclass =
                this.classFileConfiguration.Subclasses.Find(
                    s => string.Compare(s.Type, this.SubClasses[this.SubClassIndex]) == 0);

            if (subclass != null)
            {
                foreach (Number number in subclass.Numbers)
                {
                    this.CurrentSubClassNumbersList.Add(number.CurrentNumber);
                }
            }

            this.CurrentSubClassNumbersIndex =
                this.CurrentSubClassNumbersList.Count > 0
                ? 0
                : -1;
        }
    }
}