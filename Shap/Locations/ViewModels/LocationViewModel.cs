﻿namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Common;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Common.ViewModel;
    using Shap.Interfaces.Common.ViewModels;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Locations.Messages;
    using System.Collections.Generic;

    /// <summary>
    /// The view model which is used to display a location on a view.
    /// </summary>
    public class LocationViewModel : ObservableRecipient, ILocationViewModel
    {
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">
        /// IO controller manager object.
        /// </param>
        public LocationViewModel(
            IIoControllers ioControllers)
        {
            this.ioControllers = ioControllers;
            this.ClassCounters = new List<ITravelCounterViewModel>();
            this.YearCounters= new List<ITravelCounterViewModel>();

            this.Messenger.Register<DisplayLocationMessage>(
                this,
                (r, message) => this.OnDisplayLocationMessageReceived(message));
        }

        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the code of the location.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the size of the location.
        /// </summary>
        public string Size { get; private set; }

        /// <summary>
        /// Gets the year that the location opened.
        /// </summary>
        public string Opened { get; private set; }

        /// <summary>
        /// Gets the year that the location closed.
        /// </summary>
        public string Closed { get; private set; }

        /// <summary>
        /// Gets the region where the location is located.
        /// </summary>
        public string Region { get; private set; }

        /// <summary>
        /// Gets the type of the location.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets number from.
        /// </summary>
        public string TotalFrom { get; private set; }

        /// <summary>
        /// Gets the number to.
        /// </summary>
        public string TotalTo { get; private set; }

        /// <summary>
        /// Gets path to the location photo.
        /// </summary>
        public string PhotoPath { get; private set; }

        /// <summary>
        /// Gets the counters for all years.
        /// </summary>
        public List<ITravelCounterViewModel> YearCounters { get; }

        /// <summary>
        /// Gets the counters for all classes.
        /// </summary>
        public List<ITravelCounterViewModel> ClassCounters { get; }

        /// <summary>
        /// Load a new location into the view model.
        /// </summary>
        /// <param name="message">
        /// Message requesting that a new location is added.
        /// </param>
        private void OnDisplayLocationMessageReceived(DisplayLocationMessage message)
        {
            if (string.IsNullOrEmpty(message.Location))
            {
                return;
            }

            LocationDetails currentLocation =
                this.ioControllers.Location.Read(
                    message.Location);

            this.Name = currentLocation.Name;
            this.Code = currentLocation.Code;
            this.Size = currentLocation.Size;
            this.Opened = currentLocation.Opened;
            this.Closed = currentLocation.Closed;
            this.Region = currentLocation.County;
            this.Category = currentLocation.Category.ToString();
            this.TotalFrom = currentLocation.TotalFrom.ToString();
            this.TotalTo = currentLocation.TotalTo.ToString();

            if (currentLocation.Photos == null ||
                currentLocation.Photos.Count < 1 ||
                string.IsNullOrEmpty(currentLocation.Photos[0].Path))
            {
                this.PhotoPath = string.Empty;
            }
            else
            {
                this.PhotoPath =
                        BasePathReader.GetBasePathUri() +
                        StaticResources.locImgPath +
                        currentLocation.Photos[0].Path +
                        ".jpg";

            }

            this.YearCounters.Clear();

            foreach(LocationYear year in currentLocation.Years)
            {
                ITravelCounterViewModel counter =
                    new TravelCounterViewModel(
                        year.Year.ToString(),
                        year.TotalFrom.ToString(),
                        year.TotalTo.ToString());
                this.YearCounters.Add(counter);
            }

            this.ClassCounters.Clear();

            foreach (LocationClass thisClass in currentLocation.Classes)
            {
                ITravelCounterViewModel counter =
                    new TravelCounterViewModel(
                        thisClass.Name,
                        thisClass.TotalFrom.ToString(),
                        thisClass.TotalTo.ToString());
                this.ClassCounters.Add(counter);
            }

            this.OnPropertyChanged(nameof(this.Name));
            this.OnPropertyChanged(nameof(this.Code));
            this.OnPropertyChanged(nameof(this.Size));
            this.OnPropertyChanged(nameof(this.Opened));
            this.OnPropertyChanged(nameof(this.Closed));
            this.OnPropertyChanged(nameof(this.Region));
            this.OnPropertyChanged(nameof(this.Category));
            this.OnPropertyChanged(nameof(this.TotalFrom));
            this.OnPropertyChanged(nameof(this.TotalTo));
            this.OnPropertyChanged(nameof(this.PhotoPath));
            this.OnPropertyChanged(nameof(this.YearCounters));
            this.OnPropertyChanged(nameof(this.ClassCounters));
        }
    }
}