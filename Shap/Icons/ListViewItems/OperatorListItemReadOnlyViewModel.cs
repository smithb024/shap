namespace Shap.Icons.ListViewItems
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    ///  A view model which is used to display an individual operator on a list view.
    /// </summary>
    public class OperatorListItemReadOnlyViewModel : ObservableRecipient, IOperatorListItemViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorComboBoxItemViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <param name="isActive">Indicates whether the operator is currently active</param>
        /// <param name="isContemporary">
        /// Indicates whether the operator is active for the class..
        /// </param>
        public OperatorListItemReadOnlyViewModel(
            string name,
            bool isActive,
            bool isContemporary)
        {
            this.Name = name;
            this.IsOperatorActive = isActive;
            this.IsOperatorContemporary = isContemporary;
        }

        /// <summary>
        /// Gets the name of the operator.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is active.
        /// </summary>
        public bool IsOperatorActive { get; }

        /// <summary>
        /// Gets a value indicating whether the operator is currently active for the class.
        /// </summary>
        public bool IsOperatorContemporary { get; private set; }

        /// <summary>
        /// Left mouse click action.
        /// </summary>
        public ICommand OnLeftClick => null;
    }
}