namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.ViewModels.Icons;
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// View model which supports the locations selector view.
    /// </summary>
    /// <remarks>
    /// This view allows locations to be chosen for display on the locations view.
    /// </remarks>
    public class LocationsSelectorViewModel : ObservableRecipient, ILocationSelectorViewModel
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsSelectorViewModel"/> class.
        /// </summary>
        public LocationsSelectorViewModel()
        {
            this.Locations = new ObservableCollection<ISelectorRowViewModel>();

            ISelectorRowViewModel tempRow1 = new SelectorRowViewModel("Row 1");
            ISelectorRowViewModel tempRow2 = new SelectorRowViewModel("Row 2");
            ISelectorRowViewModel tempRow3 = new SelectorRowViewModel("Row 3");
            this.Locations.Add(tempRow1);
            this.Locations.Add(tempRow2);
            this.Locations.Add(tempRow3);
        }

        /// <summary>
        /// Gets the collection of locations.
        /// </summary>
        public ObservableCollection<ISelectorRowViewModel> Locations { get; }

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