namespace Shap.Common.ViewModel
{
    using Shap.Interfaces.Common.ViewModels;

    /// <summary>
    /// View model which supports a counter view.
    /// </summary>
    public class TravelCounterViewModel : ITravelCounterViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TravelCounterViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the component</param>
        /// <param name="from">the number of times from the component</param>
        /// <param name="to">the number of times to the component</param>
        public TravelCounterViewModel(
            string name,
            string from,
            string to)
        {
            this.Name = name;
            this.From = from;
            this.To = to;
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of times from.
        /// </summary>
        public string From { get; }

        /// <summary>
        /// Gets the number of times to.
        /// </summary>
        public string To { get; }
    }
}
