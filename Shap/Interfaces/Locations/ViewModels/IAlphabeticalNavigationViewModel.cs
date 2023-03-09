namespace Shap.Interfaces.Locations.ViewModels
{
    using Shap.Icon;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IAlphabeticalNavigationViewModel : INavigationViewModel
    {
        /// <summary>
        /// Gets the collection of letter icons.
        /// </summary>
        ObservableCollection<ILetterIconViewModel> Letters { get; }
    }
}
