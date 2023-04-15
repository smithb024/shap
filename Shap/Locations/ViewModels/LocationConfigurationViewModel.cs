namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Icons.ComboBoxItems;
    using Shap.Icons.ListViewItems;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Interfaces.Locations.ViewModels.Helpers;
    using Shap.Locations.Messages;
    using Shap.Locations.ViewModels.Helpers;
    using Shap.Types.Enum;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        /// The index of the currently selected operator on the combo box.
        /// </summary>
        private int operatorsIndex;

        /// <summary>
        /// The index of the currently selected operator from those assigned to the current 
        /// location.
        /// </summary>
        private int locationOperatorsIndex;

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
            this.AddOperatorsCmd = new CommonCommand(this.AddOperator, () => true);
            this.Image =
                new LocationImageSelectorViewModel(
                    ioControllers,
                    string.Empty);

            this.Regions = this.ioControllers.Location.GetRegions();
            this.Regions.Insert(0, string.Empty);
            this.regionIndex = 0;

            this.categoryIndex = 0;

            this.Operators = new ObservableCollection<IOperatorItemViewModel>();
            OperatorDetails operatorDetails = ioControllers.Operator.Read();
            this.LocationOperators = new ObservableCollection<IOperatorListItemViewModel>();

            foreach (SingleOperator singleOperator in operatorDetails.Operators)
            {
                IOperatorItemViewModel viewModel =
                    new OperatorItemViewModel(
                        singleOperator.Name,
                        singleOperator.IsActive);
                this.Operators.Add(viewModel);
            }

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
        /// Collection of all known operators.
        /// </summary>
        public ObservableCollection<IOperatorItemViewModel> Operators { get; }
        /// <summary>
        /// Gets or sets the index of the currently selected operator on the combo box.
        /// </summary>
        public int OperatorIndex
        {
            get => this.operatorsIndex;

            set => this.SetProperty(ref this.operatorsIndex, value);
        }

        /// <summary>
        /// Gets or sets the index of the currently selected operator from those assigned to the 
        /// current location.
        /// </summary>
        public int LocationOperatorIndex
        {
            get => this.locationOperatorsIndex;

            set
            {
                if (this.locationOperatorsIndex == value)
                {
                    return;
                }

                this.locationOperatorsIndex = value;
                this.OnPropertyChanged(nameof(this.LocationOperatorIndex));
            }
        }

        /// <summary>
        /// Collection of all operators assigned to this location.
        /// </summary>
        public ObservableCollection<IOperatorListItemViewModel> LocationOperators { get; }

        /// <summary>
        /// Gets a command which is used to add an operator to the location.
        /// </summary>
        public ICommand AddOperatorsCmd { get; }

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

            this.RegionIndex = 0;
            for (int index = 0; index < this.Regions.Count; ++index)
            {
                if (string.Compare(this.currentLocation.County, this.Regions[index]) == 0)
                {
                    this.RegionIndex = index;
                    break;
                }
            }

            this.LocationOperators.Clear();
            if (this.currentLocation.Operators.Count > 0)
            {
                foreach(LocationOperator modelOperator in this.currentLocation.Operators)
                {
                    bool isOperatorActive =
                        this.FindActiveState(
                            modelOperator.Name);
                    IOperatorListItemViewModel viewModel =
                        new OperatorListItemViewModel(
                            modelOperator.Name,
                            isOperatorActive,
                            modelOperator.IsContemporary);

                    this.LocationOperators.Add(viewModel);
                }
            }

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
            this.RegionIndex = 0;
            this.Image.SetImage(string.Empty);
        }

        /// <summary>
        /// Add an operator to the location. UI only. Nothing will happen if the operator
        /// is already present.
        /// </summary>
        private void AddOperator()
        {
            bool? isAlreadyAssigned = this.IsAlreadyAssigned();

            if (isAlreadyAssigned == null ||
                isAlreadyAssigned == true)
            {
                return;
            }

            IOperatorItemViewModel selectedOperator = this.Operators[this.OperatorIndex];

            IOperatorListItemViewModel viewModel =
                new OperatorListItemViewModel(
                    selectedOperator.Name,
                    selectedOperator.IsOperatorActive,
                    true);

            this.LocationOperators.Add(viewModel);
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
            this.currentLocation.County = this.Regions[this.RegionIndex];

            // Add photo
            this.currentLocation.Photos =
                new List<LocationPhotos>()
                {
                    new LocationPhotos()
                    {
                        Path = this.Image.SelectedImage
                    }
                };

            // Add operators
            this.currentLocation.Operators.Clear();

            foreach (IOperatorListItemViewModel locationOperator in this.LocationOperators)
            {
                LocationOperator newOperator =
                    new LocationOperator()
                    {
                        Name = locationOperator.Name,
                        IsContemporary = locationOperator.IsOperatorContemporary
                    };

                this.currentLocation.Operators.Add(newOperator);
            }

            this.currentLocation.Operators = this.currentLocation.Operators.OrderBy(c => c.Name).ToList();

            // Save
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

        /// <summary>
        /// Indicate whether the currently selected operator has already been assigned to this
        /// location. 
        /// It returns null if operator is not valid.
        /// </summary>
        /// <returns>Has been assigned</returns>
        private bool? IsAlreadyAssigned()
        {
            if (this.OperatorIndex >= 0 && this.OperatorIndex < this.Operators.Count)
            {
                foreach (IOperatorListItemViewModel op in this.LocationOperators)
                {
                    if (string.Compare(op.Name, this.Operators[this.OperatorIndex].Name) == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return null;
            }

            return false;
        }

        /// <summary>
        /// Find the operator with a given name and return its active state.
        /// </summary>
        /// <param name="name">name to find</param>
        /// <returns>is active state</returns>
        private bool FindActiveState(string name)
        {
            foreach (OperatorItemViewModel op in this.Operators)
            {
                if (string.Compare(op.Name, name) == 0)
                {
                    return op.IsOperatorActive;
                }
            }

            return false;
        }
    }
}