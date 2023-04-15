namespace Shap.Interfaces.Common.ViewModels
{
    /// <summary>
    /// Interface for a counter view model.
    /// </summary>
    public interface ITravelCounterViewModel
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the number of times from.
        /// </summary>
        string From { get; }

        /// <summary>
        /// Gets the number of times to.
        /// </summary>
        string To { get; }
    }
}