namespace Shap.Input
{
    using System;
    using System.Windows.Input;

    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;

    /// <summary>
    /// Delegate used to pass a new <see cref="DateTime"/>.
    /// </summary>
    /// <param name="newDate">date time to pass</param>
    public delegate void DateTimeDelegate(DateTime newDate);

    /// <summary>
    /// View models to describe a single day with a piece of supplementary information.
    /// </summary>
    public class DayViewModel : ViewModelBase
    {
        /// <summary>
        /// date time callback
        /// </summary>
        private DateTimeDelegate dateTimeCallback;

        /// <summary>
        /// Current date time
        /// </summary>
        private DateTime dateTime;

        /// <summary>
        /// Distance value
        /// </summary>
        private string dist;

        /// <summary>
        /// day is selected.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Initialise a new instance of the <see cref="DayViewModel"/> class.
        /// </summary>
        /// <param name="curentDate">current date</param>
        /// <param name="distance">current date distance</param>
        public DayViewModel(
          DateTime curentDate,
          string distance)
        {
            this.isSelected = false;
            this.dist = distance;
            this.dateTime = curentDate;
            this.SelectDayCmd = new CommonCommand(this.SendCurrentDate);
        }

        /// <summary>
        /// Gets the current day.
        /// </summary>
        public string CurrentDay
        {
            get
            {
                return this.dateTime.Day.ToString();
            }
        }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public DateTime CurrentDate => this.dateTime;

        /// <summary>
        /// Gets or sets a value which indicates whether this day is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.isSelected = value;
                RaisePropertyChangedEvent(nameof(this.IsSelected));
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the day is a weekend.
        /// </summary>
        public bool IsWeekend
        {
            get
            {
                return
                  this.dateTime.DayOfWeek == DayOfWeek.Saturday ||
                  this.dateTime.DayOfWeek == DayOfWeek.Sunday;
            }
        }

        /// <summary>
        /// Gets or sets the current dist.
        /// </summary>
        public string Dist
        {
            get
            {
                return
                  string.Compare("0", this.dist) == 0 ?
                  string.Empty :
                  this.dist;
            }

            set
            {
                this.dist = value;
                RaisePropertyChangedEvent(nameof(this.Dist));
            }
        }

        /// <summary>
        /// Gets or sets the callback to send the current date
        /// </summary>
        public DateTimeDelegate NewDateCallback
        {
            get
            {
                return this.dateTimeCallback;
            }

            set
            {
                this.dateTimeCallback = value;
            }
        }

        /// <summary>
        /// Show add jny and reset.
        /// </summary>
        public ICommand SelectDayCmd
        {
            get;
            private set;
        }

        /// <summary>
        /// Send the current date via <see cref="NewDateCallback"/>.
        /// </summary>
        public void SendCurrentDate()
        {
            if (this.NewDateCallback != null)
            {
                this.NewDateCallback.Invoke(this.dateTime);
            }
        }
    }
}