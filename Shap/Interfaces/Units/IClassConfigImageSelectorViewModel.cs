namespace Shap.Interfaces.Units
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interface which describes the image selector view model.
    /// </summary>
    public interface IClassConfigImageSelectorViewModel
    {
        /// <summary>
        /// Event raised when a new selection has been raised.
        /// </summary>
        event Action SelectionMadeEvent;

        /// <summary>
        /// Gets the list of sub class image lists.
        /// </summary>
        ObservableCollection<string> SubClassImageList { get; }

        /// <summary>
        /// Gets or sets the index for the list of sub class image lists.
        /// </summary>
        int SubClassImageListIndex { get; set; }

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