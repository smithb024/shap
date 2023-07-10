namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.ViewModels.Icons;
    using System;
    using Types.Enum;

    /// <summary>
    /// View model which supports the lines selector view.
    /// </summary>
    /// <remarks>
    /// This view allows locations to be chosen for display on the locations view.
    /// This view contains a map connecting a number of different locations. It is used to select 
    /// the location for display on the main/configuration part of the window.
    /// </remarks>
    public class LinesSelectorViewModel : ObservableRecipient, ILinesSelectorViewModel
    {
        public LinesSelectorViewModel()
        {
            this.MapCellViewModel1 = new MapCellViewModel("b");
            this.MapCellViewModel2 = new MapCellViewModel("i");
            this.MapCellViewModel3 = new MapCellViewModel("n");
            this.MapCellViewModel4 = new MapCellViewModel("a", LocationCategories.A);
            this.MapCellViewModel5 = new MapCellViewModel("a", LocationCategories.B);
            this.MapCellViewModel6 = new MapCellViewModel("a", LocationCategories.C1);
            this.MapCellViewModel7 = new MapCellViewModel("a", LocationCategories.C2);
            this.MapCellViewModel8 = new MapCellViewModel("a", LocationCategories.D);
            this.MapCellViewModel9 = new MapCellViewModel("a", LocationCategories.E);
            this.MapCellViewModel10 = new MapCellViewModel("a", LocationCategories.F);
            this.MapCellViewModel11 = new MapCellViewModel("f");
        }

        public IMapCellViewModel MapCellViewModel1 { get; }
        public IMapCellViewModel MapCellViewModel2 { get; }
        public IMapCellViewModel MapCellViewModel3 { get; }
        public IMapCellViewModel MapCellViewModel4 { get; }
        public IMapCellViewModel MapCellViewModel5 { get; }
        public IMapCellViewModel MapCellViewModel6 { get; }
        public IMapCellViewModel MapCellViewModel7 { get; }
        public IMapCellViewModel MapCellViewModel8 { get; }
        public IMapCellViewModel MapCellViewModel9 { get; }
        public IMapCellViewModel MapCellViewModel10 { get; }
        public IMapCellViewModel MapCellViewModel11 { get; }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        /// <param name="disposing">Is the class being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}