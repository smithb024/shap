namespace Shap.Locations.ViewModels.Icons
{
    using System.Windows.Input;
    using Common.Commands;
    using Interfaces.Locations.ViewModels.Icons;

    /// <summary>
    /// Supports a row on the selector view.
    /// </summary>
    /// <remarks>
    /// This contains a location and any details about it. It allows the location to be selected
    /// on the location view.
    /// </remarks>
    public class SelectorRowViewModel : ISelectorRowViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SelectorRowViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// The location name.
        /// </param>
        public SelectorRowViewModel(string name)
        {
            this.Name = name;
            this.SelectLocationCmd = new CommonCommand(this.SelectLocation);
        }

        /// <summary>
        /// Gets the name of the current locaation.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Select the location for display on the location view.
        /// </summary>
        public ICommand SelectLocationCmd { get; }

        /// <summary>
        /// Select this location.
        /// </summary>
        private void SelectLocation()
        {
        }
    }
}