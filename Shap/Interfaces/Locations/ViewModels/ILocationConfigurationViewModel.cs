namespace Shap.Interfaces.Locations.ViewModels
{
    using Helpers;
    using Icons;
    using Shap.Icons.ComboBoxItems;
    using Shap.Icons.ListViewItems;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// Collection of all known operators.
        /// </summary>
        ObservableCollection<IOperatorItemViewModel> Operators { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected operator on the combo box.
        /// </summary>
        int OperatorIndex { get; set; }

        /// <summary>
        /// Gets a command which is used to add an operator to the location.
        /// </summary>
        ICommand AddOperatorsCmd { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected operator from those assigned to the 
        /// current location.
        /// </summary>
        int LocationOperatorIndex { get; set; }

        /// <summary>
        /// Collection of all known operators assigned to the current location.
        /// </summary>
        ObservableCollection<IOperatorListItemViewModel> LocationOperators { get; }

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