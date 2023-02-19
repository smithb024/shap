namespace Shap.Interfaces.Locations.ViewModels
{
    using System.Windows.Input;

    /// <summary>
    /// Interface which supports the locations index view model.
    /// </summary>
    public interface ILocationsIndexViewModel
    {
        /// <summary>
        /// Gets the command which is used to dispayed the alphabetical navigation view.
        /// </summary>
        ICommand AlphabeticalNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the operators navigation view.
        /// </summary>
        ICommand OperatorsNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the regions navigation view.
        /// </summary>
        ICommand RegionsNavigationCommand { get; }

        /// <summary>
        /// Gets the command which is used to dispayed the lines navigation view.
        /// </summary>
        ICommand LinesNavigationCommand { get; }

        /// <summary>
        /// Gets the naviation view model.
        /// </summary>
        INavigationViewModel Navigation { get; }

        /// <summary>
        /// Gets the selector view model.
        /// </summary>
        ISelectorViewModel Selector { get; }

        /// <summary>
        /// Gets the locations details view model.
        /// </summary>
        IDetailsViewModel LocationDetails { get; }
    }
}