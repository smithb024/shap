namespace Shap.Interfaces.Locations.ViewModels.Helpers
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interface which describes the image selector view model.
    /// </summary>
    public interface ILocationImageSelectorViewModel
    {
        /// <summary>
        /// Event raised when a new selection has been raised.
        /// </summary>
        event Action SelectionMadeEvent;

        /// <summary>
        /// Gets the list of location image lists.
        /// </summary>
        ObservableCollection<string> LocationImageList { get; }

        /// <summary>
        /// Gets or sets the index for the list of location image lists.
        /// </summary>
        int LocationImageListIndex { get; set; }

        /// <summary>
        /// Gets the image which has been selected within in the view model.
        /// </summary>
        string SelectedImage { get; }

        /// <summary>
        /// Gets the path to the selected image.
        /// </summary>
        string Path { get; }
    }
}
