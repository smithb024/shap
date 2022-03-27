namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using NynaeveLib.DialogService;
    using NynaeveLib.Logger;
    using NynaeveLib.ViewModel;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Common.SerialiseModel.Family;

    using Shap.Interfaces.Io;
    using Shap.Interfaces.Units;
    using Shap.Types;
    using Shap.Units.IO;
    using Shap.Units.Dialog;
    using Shap.Units.Factories;

    /// <summary>
    /// View model which supports the class configuration dialog.
    /// </summary>
    public class ClassConfigViewModel : ViewModelBase, IClassConfigViewModel
    {
        /// <summary>
        /// Indicates the number of images which can be assigned to a specific subclass.
        /// </summary>
        private const int MaxImages = 10;

        /// <summary>
        /// The model. This data has been serialised from a configuration file.
        /// </summary>
        private ClassDetails classFileConfiguration;

        /// <summary>
        /// The formation of the unit.
        /// </summary>
        private string formation;

        /// <summary>
        /// The alpha prefix.
        /// </summary>
        private string alphaIdentifier;

        /// <summary>
        /// The year of introduction.
        /// </summary>
        private int year;

        /// <summary>
        /// Maintains the index of the currently selected in service state from the
        /// <see cref="ServiceTypeList"/> list of in service states.
        /// </summary>
        private int serviceIndex;

        /// <summary>
        /// Maintains the index of the currently selected family.
        /// </summary>
        private int familyIndex;

        /// <summary>
        /// The index of the selected subclass.
        /// </summary>
        private int subClassListIndex;

        /// <summary>
        /// IO Controllers
        /// </summary>
        private IIoControllers ioControllers;

        /// <summary>
        /// ID of the class in this view model.
        /// </summary>
        private string classId = "0";
 
        /// <summary>
        /// Indicates whether there are any unsaved changes in the view model.
        /// </summary>
        private bool unsavedChanges;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClassConfigViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">IO controllers</param>
        /// <param name="classId">class id</param>
        public ClassConfigViewModel(
          IIoControllers ioControllers,
          string classId)
        {
            this.ioControllers = ioControllers;
            FamilyDetails serialisedFamilies = ioControllers.Family.Read();
            this.familyIndex = 0;

            this.unsavedChanges = false;

            this.SubClassNumbers = new ObservableCollection<string>();
            this.NumbersList = new ObservableCollection<int>();
            this.Images = new ObservableCollection<IClassConfigImageSelectorViewModel>();
            this.FamilyList =
                new ObservableCollection<string>
            {
                string.Empty
            };

            if (serialisedFamilies != null)
            {
                foreach (SingleFamily singleFamily in serialisedFamilies.Families)
                {
                    this.FamilyList.Add(singleFamily.Name);
                }
            }

            this.classId = classId;

            // If the file doesn't exist then leave m_classData, as initialised.
            if (this.ioControllers.UnitsXml.DoesFileExist(this.classId))
            {
                this.classFileConfiguration =
                    this.ioControllers.UnitsXml.Read(
                        this.classId);

                this.formation = this.classFileConfiguration.Formation;
                this.alphaIdentifier = this.classFileConfiguration.AlphaId;
                this.year = this.classFileConfiguration.Year;

                // Select the correct family
                for (int index = 0; index < this.FamilyList.Count; ++ index)
                {
                    if (string.Compare(this.classFileConfiguration.Family, this.FamilyList[index]) == 0)
                    {
                        this.familyIndex = index;
                        continue;
                    }
                }

                for (int index = 0; index < this.ServiceTypeList.Count; ++index)
                {
                    if (this.ServiceTypeList[index] == this.classFileConfiguration.ServiceType)
                    {
                        this.serviceIndex = index;
                        break;
                    }
                }

                foreach (Subclass subclass in this.classFileConfiguration.Subclasses)
                {
                    this.SubClassNumbers.Add(subclass.Type);
                }

                if (this.SubClassNumbers.Count > 0)
                {
                    this.subClassListIndex = 0;

                    foreach (Number number in this.classFileConfiguration.Subclasses[this.SubClassListIndex].Numbers)
                    {
                        this.NumbersList.Add(number.CurrentNumber);
                    }

                    for (int imageIndex = 0; imageIndex < MaxImages; ++imageIndex)
                    {
                        string imageName =
                            imageIndex < this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images.Count
                            ? this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images[imageIndex].Name
                            : string.Empty;

                        IClassConfigImageSelectorViewModel selector =
                                new ClassConfigImageSelectorViewModel(
                                    this.ioControllers,
                                    imageName);
                        selector.SelectionMadeEvent += this.UpdateImagesInModel;
                        this.Images.Add(selector);
                    }
                }
                else
                {
                    this.subClassListIndex = -1;

                    for (int imageIndex = 0; imageIndex < MaxImages; ++imageIndex)
                    {
                        IClassConfigImageSelectorViewModel selector =
                                new ClassConfigImageSelectorViewModel(
                                    this.ioControllers,
                                    string.Empty);
                        selector.SelectionMadeEvent += this.UpdateImagesInModel;
                        this.Images.Add(selector);
                    }
                }
            }
            else
            {
                this.classFileConfiguration = new ClassDetails();
            }

            this.CanSave = false;

            this.SaveCmd = new CommonCommand(this.SaveModel, () => true);
            this.CloseCmd = new CommonCommand(this.CloseWindow);
            this.AddNewSubClassCmd = new CommonCommand(this.AddNewSubClass, this.CanPerformAction);
            this.AddNewNumberCmd = new CommonCommand(this.AddNewNumber, this.CanPerformAction);
            this.AddNewNumberSeriesCmd = new CommonCommand(this.AddNewNumberSeries, this.CanPerformAction);
            this.RenumberCmd = new CommonCommand(this.Renumber, this.CanPerformAction);
        }

        /// <summary>
        /// Gets the version of this file.
        /// </summary>
        public int Version => this.classFileConfiguration.Version;

        /// <summary>
        /// Gets or sets the formation of the unit.
        /// </summary>
        public string Formation
        {
            get
            {
                return this.formation;
            }

            set
            {
                if (string.Compare(this.formation, value) == 0)
                {
                    return;
                }

                this.formation = value;
                this.RaisePropertyChangedEvent(nameof(this.Formation));
                this.classFileConfiguration.Formation = value;
                this.unsavedChanges = true;
            }
        }

        /// <summary>
        /// Gets or sets an alpha prefix.
        /// </summary>
        public string AlphaIdentifier
        {
            get
            {
                return this.alphaIdentifier;
            }

            set
            {
                if (string.Compare(this.alphaIdentifier, value) == 0)
                {
                    return;
                }

                this.alphaIdentifier = value;
                this.RaisePropertyChangedEvent(nameof(this.AlphaIdentifier));
                this.classFileConfiguration.AlphaId = value;
                this.unsavedChanges = true;
            }
        }

        /// <summary>
        /// Gets or sets the year of introduction.
        /// </summary>
        public int Year
        {
            get
            {
                return this.year;
            }

            set
            {
                if (this.year == value)
                {
                    return;
                }

                this.year = value;
                this.RaisePropertyChangedEvent(nameof(this.Year));
                this.classFileConfiguration.Year = value;
                this.unsavedChanges = true;
            }
        }

        /// <summary>
        /// Gets the current status of the class.
        /// </summary>
        public VehicleServiceType Status => this.ServiceTypeList[this.ServiceIndex];

        /// <summary>
        /// Gets a collection containing all the enumerations in <see cref="VehicleServiceType"/>.
        /// </summary>
        /// <remarks>
        /// This is bound to a combo box list.
        /// </remarks>
        public List<VehicleServiceType> ServiceTypeList =>
            Enum.GetValues(typeof(VehicleServiceType)).
            Cast<VehicleServiceType>().
            ToList();

        /// <summary>
        /// Gets or sets the index of the currently selected in service state from the
        /// <see cref="ServiceTypeList"/> list of in service states.
        /// </summary>
        public int ServiceIndex
        {
            get
            {
                return this.serviceIndex;
            }

            set
            {
                if (this.serviceIndex == value)
                {
                    return;
                }

                this.serviceIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.ServiceIndex));
                this.RaisePropertyChangedEvent(nameof(this.Status));

                this.classFileConfiguration.ServiceType = this.Status;
            }
        }

        /// <summary>
        /// Gets a collection of all known families.
        /// </summary>
        public ObservableCollection<string> FamilyList { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected family.
        /// </summary>
        public int FamilyIndex
        {
            get
            {
                return this.familyIndex;
            }

            set
            {
                if (this.familyIndex == value)
                {
                    return;
                }

                this.familyIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.FamilyIndex));

                if (this.familyIndex >= 0 && this.familyIndex < this.FamilyList.Count)
                {
                    this.classFileConfiguration.Family = this.FamilyList[this.familyIndex];
                }
                else
                {
                    this.classFileConfiguration.Family = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the collection of sub classes present in this class.
        /// </summary>
        public ObservableCollection<string> SubClassNumbers { get; }

        /// <summary>
        /// Gets or sets the index of the selected subclass.
        /// </summary>
        public int SubClassListIndex
        {
            get
            {
                return this.subClassListIndex;
            }

            set
            {
                if (this.subClassListIndex == value)
                {
                    return;
                }

                this.subClassListIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.SubClassListIndex));
                this.SelectNewSubClass();
            }
        }

        /// <summary>
        /// Get the collection of unit numbers in the current subclass.
        /// </summary>
        public ObservableCollection<int> NumbersList { get; }

        /// <summary>
        /// Get the collection of image selector view models.
        /// </summary>
        public ObservableCollection<IClassConfigImageSelectorViewModel> Images { get; }

        /// <summary>
        /// Indicates whether the save command can be run.
        /// </summary>
        public bool CanSave { get; set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand SaveCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand AddNewSubClassCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand AddNewNumberCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand AddNewNumberSeriesCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand RenumberCmd { get; private set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand CloseCmd { get; private set; }

        /// <summary>
        ///   save model
        /// </summary>
        private void SaveModel()
        {
            ++this.classFileConfiguration.Version;
            this.ioControllers.UnitsXml.Write(
                this.classFileConfiguration,
                this.classId);

            this.RaisePropertyChangedEvent(nameof(this.Version));
            this.unsavedChanges = false;
        }

        /// <summary>
        /// The model has changed.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void ModelChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CanSave = true;
        }

        /// <summary>
        /// Close the window.
        /// </summary>
        private void CloseWindow()
        {
            this.OnClosingRequest();
        }

        /// <summary>
        /// Add new sub class to the model.
        /// </summary>
        private void AddNewSubClass()
        {
            NewSubClassViewModel dialogViewModel = new NewSubClassViewModel();

            DialogService service = new DialogService();

            service.ShowDialog(
              new NewSubClassDialog()
              {
                  DataContext = dialogViewModel
              });

            if (dialogViewModel.Result == MessageBoxResult.OK)
            {
                Subclass newSubclass =
                    new Subclass()
                    {
                        Type = dialogViewModel.SubClass,
                        Images = new List<Image>(),
                        Numbers = new List<Number>()
                    };

                // This will be null the first time a class is created.
                if (this.classFileConfiguration.Subclasses == null)
                {
                    this.classFileConfiguration.Subclasses = new List<Subclass>();
                }

                this.classFileConfiguration.Subclasses.Add(newSubclass);
                this.SubClassNumbers.Add(newSubclass.Type);
                this.SaveModel();
            }
        }

        /// <summary>
        /// Add new number to a subclass.
        /// </summary>
        private void AddNewNumber()
        {
            NewNumberViewModel dialogViewModel =
              new NewNumberViewModel(
                this.SubClassNumbers);

            DialogService service = new DialogService();

            service.ShowDialog(
              new NewNumberDialog()
              {
                  DataContext = dialogViewModel
              });

            if (dialogViewModel.Result == MessageBoxResult.OK)
            {
                if (this.classFileConfiguration.Subclasses[dialogViewModel.SubClassIndex].Numbers.Find(n => n.CurrentNumber == dialogViewModel.Number) != null)
                {
                    return;
                }

                Number newNumber =
                    new Number()
                    {
                        CurrentNumber = dialogViewModel.Number,
                        Historical = new List<OldNumber>()
                    };
                this.classFileConfiguration.Subclasses[dialogViewModel.SubClassIndex].Numbers.Add(newNumber);

                VehicleDetailsViewModel newVehicle =
                  new VehicleDetailsViewModel(
                    dialogViewModel.Number.ToString());

                if (!IndividualUnitIOController.WriteIndividualUnitFile(newVehicle, classId))
                {
                    Logger l = Logger.Instance;
                    l.WriteLog("TRACE: Class Config Form - Save Failed - Failed to write " + newVehicle.UnitNumber.ToString() + ".txt");
                }

                this.SaveModel();
                this.SelectNewSubClass();
            }
        }

        /// <summary>
        /// Add series of new numbers to a subclass.
        /// </summary>
        private void AddNewNumberSeries()
        {
            NewNumberViewModel dialogViewModel =
              new NewNumberViewModel(
                this.SubClassNumbers,
                true);

            DialogService service = new DialogService();

            service.ShowDialog(
              new NewNumberSeriesDialog()
              {
                  DataContext = dialogViewModel
              });

            if (dialogViewModel.Result == MessageBoxResult.OK)
            {
                for (int number = dialogViewModel.Number; number <= dialogViewModel.UpperNumber; ++number)
                {
                    if (this.classFileConfiguration.Subclasses[dialogViewModel.SubClassIndex].Numbers.Find(n => n.CurrentNumber == number) != null)
                    {
                        return;
                    }

                    Number newNumber =
                        new Number()
                        {
                            CurrentNumber = number,
                            Historical = new List<OldNumber>()
                        };
                    this.classFileConfiguration.Subclasses[dialogViewModel.SubClassIndex].Numbers.Add(newNumber);

                    VehicleDetailsViewModel newVehicle =
                      new VehicleDetailsViewModel(
                        number.ToString());

                    if (!IndividualUnitIOController.WriteIndividualUnitFile(newVehicle, classId))
                    {
                        Logger l = Logger.Instance;
                        l.WriteLog("TRACE: Class Config Form - Save Failed - Failed to write " + newVehicle.UnitNumber.ToString() + ".txt");
                    }
                }

                this.SaveModel();
                this.SelectNewSubClass();
            }
        }

        /// <summary>
        /// Renumber things.
        /// </summary>
        private void Renumber()
        {
            RenumberViewModel dialogViewModel =
              new RenumberViewModel(
                this.SubClassNumbers,
                this.classFileConfiguration);

            DialogService service = new DialogService();

            service.ShowDialog(
              new RenumberDialog()
              {
                  DataContext = dialogViewModel
              });

            if (dialogViewModel.Result == MessageBoxResult.OK)
            {
                RenumberFactory.Renumber(
                    this.classFileConfiguration,
                    this.classId,
                    dialogViewModel.OriginalNumber,
                    dialogViewModel.OriginalSubClass,
                    dialogViewModel.NewNumber,
                    dialogViewModel.NewSubClass,
                    dialogViewModel.TotalNumberToChange);

                this.SaveModel();
                this.SelectNewSubClass();
            }
        }

        /// <summary>
        /// One of the image selectors has changed. Reflect the change in the model.
        /// </summary>
        private void UpdateImagesInModel()
        {
            this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images.Clear();

            foreach(IClassConfigImageSelectorViewModel selectedImage in Images)
            {
                if (string.IsNullOrEmpty(selectedImage.SelectedImage))
                {
                    continue;
                }

                Image newImage =
                    new Image()
                    {
                        Name = selectedImage.SelectedImage
                    };

                this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images.Add(newImage);
            }

            this.unsavedChanges = true;
        }

        /// <summary>
        /// A new subclass has been selected. Update the relevant fields from the model.
        /// </summary>
        private void SelectNewSubClass()
        {
            if (this.SubClassListIndex < 0 ||
                this.SubClassListIndex >= this.SubClassNumbers.Count)
            {
                return;
            }

            // Clear down
            this.NumbersList.Clear();

            foreach(IClassConfigImageSelectorViewModel image in this.Images)
            {
                image.SelectionMadeEvent -= this.UpdateImagesInModel;
            }

            this.Images.Clear();
            this.RaisePropertyChangedEvent(nameof(this.NumbersList));
            this.RaisePropertyChangedEvent(nameof(this.Images));

            // Refresh
            foreach (Number number in this.classFileConfiguration.Subclasses[this.SubClassListIndex].Numbers)
            {
                this.NumbersList.Add(number.CurrentNumber);
            }

            for (int imageIndex = 0; imageIndex < MaxImages; ++imageIndex)
            {
                string imageName =
                    imageIndex < this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images.Count
                    ? this.classFileConfiguration.Subclasses[this.SubClassListIndex].Images[imageIndex].Name
                    : string.Empty;

                IClassConfigImageSelectorViewModel selector =
                        new ClassConfigImageSelectorViewModel(
                            this.ioControllers,
                            imageName);
                selector.SelectionMadeEvent += this.UpdateImagesInModel;
                this.Images.Add(selector);
            }

            this.RaisePropertyChangedEvent(nameof(this.NumbersList));
            this.RaisePropertyChangedEvent(nameof(this.Images));
        }

        /// <summary>
        /// Indicates whether the commands are available. They should not be, if there are
        /// unsaved changes.
        /// </summary>
        /// <returns>Indicates whether the action can be performed.</returns>
        private bool CanPerformAction()
        {
            return !this.unsavedChanges;
        }
    }
}