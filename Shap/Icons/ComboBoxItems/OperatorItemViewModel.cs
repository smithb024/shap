namespace Shap.Icons.ComboBoxItems
{
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// View model which supports a row in an operators combo box.
    /// </summary>
    public class OperatorItemViewModel : ObservableRecipient, IOperatorItemViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorItemViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <param name="isActive">Indicates whether the operator is currently active</param>
        public OperatorItemViewModel(
            string name,
            bool isActive)
        {
            this.Name = name;
            this.IsOperatorActive = isActive;
        }

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        public bool IsOperatorActive { get; }
    }
}