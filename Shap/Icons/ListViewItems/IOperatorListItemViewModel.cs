namespace Shap.Icons.ListViewItems
{
    /// <summary>
    /// Interface for a view model which is used to display an individual operator on a list view.
    /// </summary>
    public interface IOperatorListItemViewModel
    {
        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        bool IsOperatorActive { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is currently active for the class.
        /// </summary>
        bool IsOperatorContemporary { get; }

        /// <summary>
        /// Sets a new state for the <see cref="IsContemporary"/> property.
        /// </summary>
        /// <param name="isContemporary">new value</param>
        void SetIsContemporary(bool isContemporary);
    }
}