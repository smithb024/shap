/// <summary>
/// Interface which display operators in a combo box.
/// </summary>
namespace Shap.Icons.ComboBoxItems
{
    public interface IOperatorItemViewModel
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