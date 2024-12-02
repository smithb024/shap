namespace Shap.Analysis.ViewModels.Data
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// The view model which supports the totals analysis grid.
    /// </summary>
    public class TotalsViewModel : ViewModelBase
    {
        /// <summary>
        /// Current index of this location. It is used as a counter on the view.
        /// </summary>
        private int index;

        /// <summary>
        /// Initialises a new instance of the <see cref="TotalsViewModel"/> class.
        /// </summary>
        /// <param name="name">The name to display.</param>
        /// <param name="total">The total to associate with the <paramref name="name"/></param>
        public TotalsViewModel(
            string name,
            int total)
        {
            index = 0;
            this.Name = name;
            this.Total = total;
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
        /// Gets the name to display.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the total of <see cref="Name"/> objects.
        /// </summary>
        public int Total { get; }
    }
}