using Shap.Interfaces.Locations.ViewModels.Icons;

namespace Shap.Interfaces.Locations.ViewModels
{
    public interface ILinesSelectorViewModel : ISelectorViewModel
    {
        IMapCellViewModel MapCellViewModel1 { get; }
        IMapCellViewModel MapCellViewModel2 { get; }
        IMapCellViewModel MapCellViewModel3 { get; }
        IMapCellViewModel MapCellViewModel4 { get; }
    }
}
