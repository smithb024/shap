namespace Shap.Locations.ViewModels.Icons
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Types.Enum;
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
        /// <param name="isValid">
        /// Indicates whether the location is monitored by the application.
        /// </param>
        /// <param name="controllers">
        /// The IO controllers. Only relevant if <paramref name="isValid"/> is true.
        /// </param>
        /// <param name="name">
        /// The name of the location. Only relevant if <paramref name="isValid"/> is true.
        /// </param>
        public MapCellRowViewModel(
            int count,
            List<string> codes,
            bool isValid,
            IIoControllers controllers,
            string name)
        {
            this.Icons = new ObservableCollection<IMapCellViewModel>();

            for (int i = 0; i < count; i++)
            {
                IMapCellViewModel cell;

                if (i < codes.Count)
                {
                    LocationCategories category;

                    if (isValid)
                    {
                        LocationDetails location =
                            controllers.Location.Read(
                                name);
                        category = location.Category;
                    }
                    else
                    {
                        category = LocationCategories.ND;
                    }

                    cell =
                        new MapCellViewModel(
                            codes[i],
                            category);
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