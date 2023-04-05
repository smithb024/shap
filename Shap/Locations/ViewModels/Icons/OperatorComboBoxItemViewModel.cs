namespace Shap.Locations.ViewModels.Icons
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels.Icons;

    /// <summary>
    /// View model which supports a row in an operators combo box.
    /// </summary>
    public class OperatorComboBoxItemViewModel : ObservableRecipient, IOperatorComboBoxItemViewModel
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