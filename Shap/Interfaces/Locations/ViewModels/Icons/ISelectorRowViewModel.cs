namespace Shap.Interfaces.Locations.ViewModels.Icons
{
    using System.Windows.Input;

    /// <summary>
    /// Interface which supports a row on the selector view.
    /// </summary>
    public interface ISelectorRowViewModel
    {
        /// <summary>
        /// Gets the name of the current locaation.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Select the location for display on the location view.
        /// </summary>
        ICommand SelectLocationCmd { get; }
    }
}