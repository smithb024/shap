namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.ViewModels.Icons;
    using System;

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
            this.MapCellViewModel1 = new MapCellViewModel(true, false);
            this.MapCellViewModel2 = new MapCellViewModel(false, true);
            this.MapCellViewModel3 = new MapCellViewModel(false, false);
            this.MapCellViewModel4 = new MapCellViewModel(true, true);
        }

        public IMapCellViewModel MapCellViewModel1 { get; }
        public IMapCellViewModel MapCellViewModel2 { get; }
        public IMapCellViewModel MapCellViewModel3 { get; }
        public IMapCellViewModel MapCellViewModel4 { get; }

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