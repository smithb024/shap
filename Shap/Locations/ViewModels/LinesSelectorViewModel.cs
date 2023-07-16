namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Icons;
    using Shap.Locations.Messages;
    using Shap.Locations.Model;
    using Shap.Locations.ViewModels.Icons;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// The location analyser.
        /// </summary>
        private readonly ILocationAnalyser locationAnalyser;

        /// <summary>
        /// The criteria by which the locations are selected.
        /// </summary>
        private string searchCriteria;

        public LinesSelectorViewModel(
            IIoControllers ioControllers,
            ILocationAnalyser locationAnalyser)
        {
            this.ioControllers = ioControllers;
            this.locationAnalyser = locationAnalyser;
            this.Locations = new ObservableCollection<ISelectorRowViewModel>();
            this.Icons = new ObservableCollection<IMapCellRowViewModel>();

            this.Messenger.Register<LineSelectorMessage>(
               this,
               (r, message) => this.NewLineSelected(message));
            this.Messenger.Register<RequestLocationsRefreshMessage>(
                this,
                (r, message) => this.OnRequestLocationsRefreshMessage(message));
        }

        /// <summary>
        /// Gets the collection of locations.
        /// </summary>
        public ObservableCollection<ISelectorRowViewModel> Locations { get; }

        /// <summary>
        /// Gets the collection of rows of icons.
        /// </summary>
        public ObservableCollection<IMapCellRowViewModel> Icons { get; }

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

        /// <summary>
        /// Display all locations in the indicated line.
        /// </summary>
        /// <param name="message">
        /// The <see cref="LineSelectorMessage"/> message.
        /// </param>
        public void NewLineSelected(LineSelectorMessage message)
        {
            this.searchCriteria = message.Line;
            this.RebuildLocationsList();
        }

        /// <summary>
        /// Request that all of the currently displayed locations are refreshed.
        /// </summary>
        /// <param name="message">
        /// The <see cref="RequestLocationsRefreshMessage"/> message.
        /// </param>
        private void OnRequestLocationsRefreshMessage(RequestLocationsRefreshMessage message)
        {
            List<string> locations = new List<string>();

            foreach (ISelectorRowViewModel selector in this.Locations)
            {
                if (selector.IsValid)
                {
                    locations.Add(selector.Name);
                }
            }

            this.locationAnalyser.Analyse(locations);
        }

        /// <summary>
        /// Rebuild the locations list based on the current search criteria.
        /// </summary>
        private void RebuildLocationsList()
        {
            List<LineDetail> allLocations =
                this.ioControllers.Location.ReadLine(
                    this.searchCriteria);

            this.Locations.Clear();
            this.Icons.Clear();

            int iconCount = 0;
            foreach(LineDetail detail in allLocations)
            {
                if (detail.Count > iconCount)
                {
                    iconCount = detail.Count;
                }
            }

            foreach (LineDetail detail in allLocations)
            {
                bool isValid =
                    !string.IsNullOrEmpty(detail.Location) &&
                    this.ioControllers.Location.DoesFileExist(detail.Location);

                IMapCellRowViewModel cells =
                    new MapCellRowViewModel(
                        iconCount,
                        detail.Codes,
                        isValid,
                        this.ioControllers,
                        detail.Location);
                ISelectorRowViewModel row =
                    new SelectorRowViewModel(
                        this.ioControllers,
                        detail.Location,
                        isValid);
                this.Icons.Add(cells);
                this.Locations.Add(row);
            }

            this.OnPropertyChanged(nameof(this.Locations));
            this.OnPropertyChanged(nameof(this.Icons));
        }
    }
}