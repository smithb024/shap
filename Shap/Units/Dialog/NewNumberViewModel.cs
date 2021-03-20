namespace Shap.Units.Dialog
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Base;

    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;

    /// <summary>
    /// View model which supports renumbering.
    /// </summary>
    public class NewNumberViewModel : SubClassSelectorViewModel
    {
        /// <summary>
        /// The new number, this is the lower number if a series is defined.
        /// </summary>
        private int number;

        /// <summary>
        /// The upper number of a number series.
        /// </summary>
        private int upperNumber;

        /// <summary>
        /// Indicates whether this is a number series or not.
        /// </summary>
        private bool addMultiple;

        /// <summary>
        /// Initialises a new instance of the <see cref="NewNumberViewModel"/> class.
        /// </summary>
        /// <param name="subClasses">
        /// Collection of all sub classes.
        /// </param>
        /// <param name="addMultiple">
        /// Indicates whether to add a collection or not
        /// </param>
        public NewNumberViewModel(
          ObservableCollection<string> subClasses,
          bool addMultiple = false)
          : base(subClasses)
        {
            this.addMultiple = addMultiple;
            this.Result = MessageBoxResult.Cancel;

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
        /// Gets or sets the result of this dialog.
        /// </summary>
        public MessageBoxResult Result { get; set; }


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