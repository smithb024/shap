namespace Shap.Units.Dialog
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// View model which is used to display operators on the 
    /// <see cref="UpdateOperatorsDialog"/> combo box.
    /// </summary>
    public class OperatorComboBoxItemViewModel : ViewModelBase, IViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorComboBoxItemViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <param name="isActive">Indicates whether the operator is currently active</param>
        public OperatorComboBoxItemViewModel(
            string name,
            bool isActive)
        {
            this.Name = name;
            this.IsActive = isActive;
        }

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        public bool IsActive { get; }
    }
}