namespace Shap.Analysis.ViewModels.Data
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// View model for a view which shows a single location and its details.
    /// </summary>
    public class LocationViewModel : ViewModelBase
    {
        /// <summary>
        /// Current index of this location. It is used as a counter on the view.
        /// </summary>
        private int index;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationViewModel"/> class.
        /// </summary>
        /// <param name="name">name of the location</param>
        /// <param name="total">number of times location used</param>
        /// <param name="numberFrom">number of times departed from the location</param>
        /// <param name="numberTo">number of times arrived at the location</param>
        /// <param name="isYear">is this year or total analysis</param>
        public LocationViewModel(
            string name,
            int total,
            int numberFrom,
            int numberTo,
            bool isYear)
        {
            this.index = 0;
            this.Name = name;
            this.Total = total;
            this.NumberFrom = numberFrom;
            this.NumberTo = numberTo;
            this.IsYear = isYear;
        }

        /// <summary>
        /// Gets or sets the current index.
        /// </summary>
        public int Index
        {
            get => this.index;
            set => this.SetProperty(ref this.index, value);
        }

        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number from the location.
        /// </summary>
        public int NumberFrom { get; }

        /// <summary>
        /// Gets the number to the location.
        /// </summary>
        public int NumberTo { get; }

        /// <summary>
        /// Gets the total uses of the location.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Gets a value indicating whether this is year analysis or not.
        /// </summary>
        public bool IsYear { get; }
    }
}