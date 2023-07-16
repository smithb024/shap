namespace Shap.Locations.ViewModels.Icons
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// a view model which supports a set of cell icons within the lines selector view.
    /// </summary>
    public class MapCellRowViewModel : ObservableRecipient, IMapCellRowViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MapCellRowViewModel"/> class.
        /// </summary>
        /// <param name="count">
        /// The number of cells to create.
        /// </param>
        /// <param name="codes">
        /// The collection of codes which are used to create the icons. 
        /// </param>
        public MapCellRowViewModel(
            int count,
            List<string> codes)
        {
            this.Icons = new ObservableCollection<IMapCellViewModel>();

            for (int i = 0; i < count; i++)
            {
                IMapCellViewModel cell;

                if (i < codes.Count)
                {
                    cell = new MapCellViewModel(codes[i]);
                }
                else
                {
                    cell = new MapCellViewModel(string.Empty);
                }

                this.Icons.Add(cell);
            }
        }

        /// <summary>
        /// Gets the collection of icons.
        /// </summary>
        public ObservableCollection<IMapCellViewModel> Icons { get; }
    }
}