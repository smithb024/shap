namespace Shap.Interfaces.Units
{
    using System.Collections.ObjectModel;

    using Shap.Units;

    /// <summary>
    /// Interface for the sub class view model.
    /// </summary>
    public interface ISubClassViewModel
    {
        /// <summary>
        /// Collection of units belonging to this <see cref="ISubClassViewModel"/>
        /// </summary>
        ObservableCollection<IUnitViewModel> Units { get; }

        /// <summary>
        /// Gets the alpha identifier for this <see cref="SubClassViewModel"/>.
        /// </summary>
        string AlphaIdentifier { get; }
    }
}