namespace Shap.Interfaces.Locations.ViewModels
{
    using Helpers;
    using Shap.Types.Enum;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// Interface which defines the location configuration view model.
    /// </summary>
    public interface ILocationConfigurationViewModel : IDetailsViewModel
    {
        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the short code of the location.
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// Gets the size of the location.
        /// </summary>
        string Size { get; set; }

        /// <summary>
        /// Gets the year that the location opened.
        /// </summary>
        string Opened { get; set; }

        /// <summary>
        /// Gets the year that the location closed.
        /// </summary>
        string Closed { get; set; }

        /// <summary>
        /// Gets the index of the selected category.
        /// </summary>
        int CategoryIndex { get; set; }

        /// <summary>
        /// Gets the collection of all possible location categories.
        /// </summary>
        List<string> Categories { get; }

        /// <summary>
        /// Gets the index of the selected regions.
        /// </summary>
        int RegionIndex { get; set; }

        /// <summary>
        /// Gets the collection of all possible location regions.
        /// </summary>
        List<string> Regions { get; }

        /// <summary>
        /// Get the collection of image selector view models.
        /// </summary>
        ILocationImageSelectorViewModel Image { get; }

        /// <summary>
        /// Indicates whether the save command can be run.
        /// </summary>
        bool CanSave { get; set; }

        /// <summary>
        /// Save command.
        /// </summary>
        ICommand SaveCmd { get; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        ICommand CancelCmd { get; }
    }
}