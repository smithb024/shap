namespace Shap.Config.GroupsAndClasses
{
    using NynaeveLib.ViewModel;

    /// <summary>
    /// View model which is used to display operators during configuration.
    /// </summary>
    public class OperatorConfigViewModel : ViewModelBase, IViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="OperatorConfigViewModel"/> class.
        /// </summary>
        /// <param name="name">The name of the operator</param>
        /// <param name="isActive">Indicates whether the operator is currently active</param>
        public OperatorConfigViewModel(
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
        public bool IsActive { get; private set; }

        /// <summary>
        /// Toggles the <see cref="IsActive"/> state.
        /// </summary>
        public void ToggleActive()
        {
            this.IsActive = !this.IsActive;
            this.RaisePropertyChangedEvent(nameof(this.IsActive));
        }
    }
}