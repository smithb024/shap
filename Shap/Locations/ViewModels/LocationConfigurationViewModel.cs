namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Helpers;
    using Shap.Locations.Messages;
    using Shap.Locations.ViewModels.Helpers;
    using Shap.Types.Enum;
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    public class LocationConfigurationViewModel : ObservableRecipient, ILocationConfigurationViewModel
    {
        /// <summary>
        /// The IO Controllers for the application.
        /// </summary>
        private IIoControllers ioControllers;

        // <summary>
        /// the name of the location.
        /// </summary>
        private string name;

        /// <summary>
        /// the short code of the location.
        /// </summary>
        private string code;

        /// <summary>
        /// the size of the location.
        /// </summary>
        private string size;

        /// <summary>
        /// the year that the location opened.
        /// </summary>
        private string opened;

        /// <summary>
        /// the year that the location closed.
        /// </summary>
        private string closed;

        /// <summary>
        /// the index of the selected category.
        /// </summary>
        private int categoryIndex;

        /// <summary>
        /// The index of the selected region.
        /// </summary>
        private int regionIndex;

        /// <summary>
        /// The currently loaded location.
        /// </summary>
        private LocationDetails currentLocation;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationConfigurationViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO controller manager object</param>
        public LocationConfigurationViewModel(
            IIoControllers ioControllers)
        {
            this.ioControllers = ioControllers;

            this.SaveCmd = new CommonCommand(this.Save, () => true);
            this.CancelCmd = new CommonCommand(this.Cancel, () => true);
            this.Image =
                new LocationImageSelectorViewModel(
                    ioControllers,
                    string.Empty);

            this.Regions = this.ioControllers.Location.GetRegions();
            this.Regions.Insert(0, string.Empty);

            this.Messenger.Register<DisplayLocationMessage>(
                this,
                (r, message) => this.OnDisplayLocationMessageReceived(message));
        }

        /// <summary>
        /// Gets the name of the location.
        /// </summary>
        public string Name
        {
            get => this.name;
            private set => this.SetProperty(ref this.name, value);
        }

        /// <summary>
        /// Gets the short code of the location.
        /// </summary>
        public string Code
        {
            get => this.code;
            set => this.SetProperty(ref this.code, value);
        }

        /// <summary>
        /// Gets the size of the location.
        /// </summary>
        public string Size
        {
            get => this.size;
            set => this.SetProperty(ref this.size, value);
        }

        /// <summary>
        /// Gets the year that the location opened.
        /// </summary>
        public string Opened
        {
            get => this.opened;
            set => this.SetProperty(ref this.opened, value);
        }

        /// <summary>
        /// Gets the year that the location closed.
        /// </summary>
        public string Closed
        {
            get => this.closed;
            set => this.SetProperty(ref this.closed, value);
        }

        /// <summary>
        /// Gets the index of the selected category.
        /// </summary>
        public int CategoryIndex
        {
            get => this.categoryIndex;
            set => this.SetProperty(ref this.categoryIndex, value);
        }

        /// <summary>
        /// Gets the collection of all possible location categories.
        /// </summary>
        public List<string> Categories
        {
            get
            {
                List<string> categories = new List<string>();

                foreach(LocationCategories category in Enum.GetValues(typeof(LocationCategories)))
                {
                    categories.Add(
                        LocationCategoriesConverter.Convert(
                            category));
                }

                return categories; 
            }
        }

        /// <summary>
        /// Gets the index of the selected region.
        /// </summary>
        public int RegionIndex
        {
            get => this.regionIndex;
            set => this.SetProperty(ref this.regionIndex, value);
        }

        /// <summary>
        /// Gets the collection of all possible location categories.
        /// </summary>
        public List<string> Regions { get; private set; }

        /// <summary>
        /// Get the image selector view models.
        /// </summary>
        public ILocationImageSelectorViewModel Image { get; }

        /// <summary>
        /// Indicates whether the save command can be run.
        /// </summary>
        public bool CanSave { get; set; }

        /// <summary>
        /// Save command.
        /// </summary>
        public ICommand SaveCmd { get; }

        /// <summary>
        /// Cancel command.
        /// </summary>
        public ICommand CancelCmd { get; }

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
                this.Clear();
                return;
            }

            this.currentLocation =
                this.ioControllers.Location.Read(
                    message.Location);

            this.Name = this.currentLocation.Name;
            this.Code = this.currentLocation.Code ?? string.Empty;
            this.Size = this.currentLocation.Size ?? string.Empty;
            this.Opened = this.currentLocation.Opened ?? string.Empty;
            this.Closed = this.currentLocation.Closed ?? string.Empty;
            this.CategoryIndex = (int)this.currentLocation.Category;

            if (this.currentLocation.Photos.Count > 0)
            {
                this.Image.SetImage(this.currentLocation.Photos[0].Path);
            }
            else
            {
                this.Image.SetImage(string.Empty);
            }
        }

        /// <summary>
        /// Clear the view model.
        /// </summary>
        private void Clear()
        {
            this.Name = string.Empty;
            this.Code = string.Empty;
            this.Size = string.Empty;
            this.Opened = string.Empty;
            this.Closed = string.Empty;
            this.CategoryIndex = 0;
            this.Image.SetImage(string.Empty);
        }

        /// <summary>
        /// Save the current location.
        /// </summary>
        private void Save()
        {
            this.currentLocation.Code = this.Code;
            this.currentLocation.Size = this.Size;
            this.currentLocation.Opened = this.Opened;
            this.currentLocation.Closed = this.Closed;
            this.currentLocation.Category = (LocationCategories)this.CategoryIndex;

            this.currentLocation.Photos =
                new List<LocationPhotos>()
                {
                    new LocationPhotos()
                    {
                        Path = this.Image.SelectedImage
                    }
                };

            this.ioControllers.Location.Write(
                this.currentLocation,
                this.Name);
        }

        /// <summary>
        /// Cancel updates. Send message to close the window.
        /// </summary>
        private void Cancel()
        {
            this.Clear();
            DisplayLocationMessage message = new DisplayLocationMessage(string.Empty);
            this.Messenger.Send(message);
        }
    }
}