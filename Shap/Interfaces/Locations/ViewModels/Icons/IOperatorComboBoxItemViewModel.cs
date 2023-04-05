namespace Shap.Interfaces.Locations.ViewModels.Icons
{
    /// <summary>
    /// Interface which display operators in a combo box.
    /// </summary>
    public interface IOperatorComboBoxItemViewModel
    {
        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        bool IsOperatorActive { get; }
    }
}
