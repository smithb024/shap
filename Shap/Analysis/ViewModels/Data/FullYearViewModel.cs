namespace Shap.Analysis.ViewModels.Data
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// The view model which supports the totals analysis grid.
    /// </summary>
    public class FullYearViewModel : ViewModelBase
    {
        /// <summary>
        /// Current index of this location. It is used as a counter on the view.
        /// </summary>
        private int index;

        /// <summary>
        /// Initialises a new instance of the <see cref="FullYearViewModel"/> class.
        /// </summary>
        /// <param name="name">The name to display.</param>
        /// <param name="total">The total to associate with the <paramref name="name"/></param>
        /// <param name="jan">The january total</param>
        /// <param name="feb">The february total</param>
        /// <param name="mar">The march total</param>
        /// <param name="apr">The april total</param>
        /// <param name="may">The may total</param>
        /// <param name="jun">The june total</param>
        /// <param name="jul">The july total</param>
        /// <param name="aug">The august total</param>
        /// <param name="sept">The september total</param>
        /// <param name="oct">The october total</param>
        /// <param name="nov">The november total</param>
        /// <param name="dec">The december total</param>
        public FullYearViewModel(
            string name,
            int total,
            int jan,
            int feb,
            int mar,
            int apr,
            int may,
            int jun,
            int jul,
            int aug,
            int sept,
            int oct,
            int nov,
            int dec)
        {
            this.index = 0;
            this.Name = name;
            this.Total = total;

            this.Jan = jan;
            this.Feb = feb;
            this.Mar = mar;
            this.Apr = apr;
            this.May = may;
            this.Jun = jun;
            this.Jul = jul;
            this.Aug = aug;
            this.Sept = sept;
            this.Oct = oct;
            this.Nov = nov;
            this.Dec = dec;
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

        /// <summary>
        /// Gets the January total.
        /// </summary>
        public int Jan { get; }

        /// <summary>
        /// Gets the February total.
        /// </summary>
        public int Feb { get; }

        /// <summary>
        /// Gets the March total.
        /// </summary>
        public int Mar { get; }

        /// <summary>
        /// Gets the April total.
        /// </summary>
        public int Apr { get; }

        /// <summary>
        /// Gets the May total.
        /// </summary>
        public int May { get; }

        /// <summary>
        /// Gets the June total.
        /// </summary>
        public int Jun { get; }

        /// <summary>
        /// Gets the July total.
        /// </summary>
        public int Jul { get; }

        /// <summary>
        /// Gets the August total.
        /// </summary>
        public int Aug { get; }

        /// <summary>
        /// Gets the September total.
        /// </summary>
        public int Sept { get; }

        /// <summary>
        /// Gets the October total.
        /// </summary>
        public int Oct { get; }

        /// <summary>
        /// Gets the November total.
        /// </summary>
        public int Nov { get; }

        /// <summary>
        /// Gets the December total.
        /// </summary>
        public int Dec { get; }
    }
}