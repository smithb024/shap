namespace Shap.Units.Dialog
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// View model which is used to display operators on the 
    /// <see cref="UpdateOperatorsDialog"/> combo box.
    /// </summary>
    public class OperatorListItemViewModel : ViewModelBase, IViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorComboBoxItemViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <param name="isActive">Indicates whether the operator is currently active</param>
        /// <param name="isContemporary">
        /// Indicates whether the operator is active for the class..
        /// </param>
        public OperatorListItemViewModel(
            string name,
            bool isActive,
            bool isContemporary)
        {
            this.Name = name;
            this.IsActive = isActive;
            this.IsContemporary = isContemporary;
        }

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is currently active for the class.
        /// </summary>
        public bool IsContemporary { get; private set; }

        /// <summary>
        /// Sets a new state for the <see cref="IsContemporary"/> property.
        /// </summary>
        /// <param name="isContemporary">new value</param>
        public void SetIsContemporary(bool isContemporary)
        {
            this.IsContemporary = isContemporary;
            this.RaisePropertyChangedEvent(nameof(this.IsContemporary));
        }
    }
}