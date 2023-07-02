namespace Shap.Interfaces.Locations.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for a navigation view model. This view model supports a combo box from which 
    /// the navigation search criteria is selected.
    /// </summary>
    public interface IDirectNavigationViewModel : INavigationViewModel
    {
        /// <summary>
        /// Gets the index of the selected search criteria.
        /// </summary>
        int SearchCriteriaIndex { get; set; }

        /// <summary>
        /// Gets the collection of all possible search criteria.
        /// </summary>
        List<string> SearchCriteria { get; }
    }
}