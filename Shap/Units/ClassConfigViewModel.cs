// TODO Only Add sub class has difference OK Cancel behavour
namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using NynaeveLib.DialogService;
    using NynaeveLib.Logger;
    using NynaeveLib.ViewModel;
    using Shap.Common;
    using Shap.Common.Commands;
    using Shap.Common.Factories;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Interfaces.Units;
    using Shap.Types;
    using Shap.Units.IO;
    using Shap.Units.Dialog;

    public class ClassConfigViewModel : ViewModelBase, IClassConfigViewModel
    {
        // Constants
        private const string c_noSubClassSelectedWarning = "No sub class has been selected";

        /// <summary>
        /// Indicates the number of images which can be assigned to a specific subclass.
        /// </summary>
        private const int MaxImages = 10;

        /// <summary>
        /// The model. This data has been serialised from a configuration file.
        /// </summary>
        private ClassDetails classFileConfiguration;

        /// <summary>
        /// The version of this file.
        /// </summary>
        private int version;

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
        /// The index of the selected subclass.
        /// </summary>
        private int subClassListIndex;

        //private const int c_formerNumberPositionX = 175;
        //private const int c_formerNumberPositionY = 47;
        //private const int c_formerNumberRowHeight = 30;

        private UnitsXmlIOController unitsXmlIoController;
        private UnitsIOController unitsIoController;
        //private IndividualUnitIOController individualUnitIoController;

        //private ClassIndexForm m_parent;
        private string classId = "0";
        //private ClassDataType m_classData;
        private bool m_classChanged = false;
        //private List<List<string>> m_formerNumberArray = new List<List<string>>();
        //private List<Label> m_formerNumberLabelList = new List<Label>();
        private int formerNumbersAvailable = 0;

        private List<int> m_newNumberList = new List<int>();    // maintains new vehicles which are currently unsaved.
        private List<int>[] m_renumberedList = new List<int>[2];   // maintains tht renumbered vehicles which are currently unsaved.
        private List<string> m_renumberedListSubClass = new List<string>(); // maintains the subclass data corresponding to m_renumberedList

        //private Point m_mouseDownLocation = new Point();
        //private Point m_origLocation = new Point();

        //private ObservableCollection<string> subClassImageList;
        //private int subClassImageListIndex;

        //private List<string> subClassList;
        //private int subClassListIndex;

        //private string formation;
        //private string originYear;
        //private string version;
        //private string alphaId;

        /// <summary>
        ///   creates a new instance of the class config form
        /// </summary>
        /// <param name="unitsIoController">units IO controller</param>
        /// <param name="unitsXmlIoController">units XML IO controller</param>
        /// <param name=")">individual units XML IO controller</param>
        /// <param name="classId">class id</param>
        public ClassConfigViewModel(
          UnitsIOController unitsIoController,
          UnitsXmlIOController unitsXmlIoController,
          string classId)
        {
            this.unitsIoController = unitsIoController;
            this.unitsXmlIoController = unitsXmlIoController;

            this.SubClassNumbers = new ObservableCollection<string>();
            this.NumbersList = new ObservableCollection<int>();
            this.Images = new ObservableCollection<IClassConfigImageSelectorViewModel>();

            //this.individualUnitIoController = individualUnitIoController;
            //int formerNumberRowPosition = c_formerNumberPositionY;

            //m_parent = classIndexForm;
            this.classId = classId;
            //InitializeComponent();
            //this.Text = this.classId;
            //labelTitleBar.Text = "Class Config - " + this.classId;
            //this.ClassData = new ClassDataType(classId);

            ////// location
            ////this.Location = new Point(450, 0);
            ////this.StartPosition = FormStartPosition.Manual;

            m_renumberedList[0] = new List<int>();
            m_renumberedList[1] = new List<int>();

            //// read number of former Numbers
            //if (File.Exists(BasePathReader.GetBasePath() +
            //                StaticResources.miscellaneousPath +
            //                StaticResources.FileNameOldNumbersAvailable))
            //{
            //  using (StreamReader reader = new StreamReader(BasePathReader.GetBasePath() +
            //                                                StaticResources.miscellaneousPath +
            //                                                StaticResources.FileNameOldNumbersAvailable))
            //  {
            //    string fileContent = reader.ReadLine();
            //    if (!int.TryParse(fileContent, out this.formerNumbersAvailable))
            //    {
            //      this.formerNumbersAvailable = 0;
            //      Logger logger = Logger.Instance;
            //      logger.WriteLog("ClassConfigForm constructor: Can't convert formNumbersAvailable value: " + fileContent);
            //    }
            //  }
            //}
            //else
            //{
            //  this.formerNumbersAvailable = 0;
            //  Logger logger = Logger.Instance;
            //  logger.WriteLog("TRACE: ClassConfigForm constructor: Can't find file " +
            //             BasePathReader.GetBasePath() +
            //             StaticResources.miscellaneousPath +
            //             StaticResources.FileNameOldNumbersAvailable);
            //}

            //// create former number labels
            //for (int i = 0; i < this.formerNumbersAvailable; ++i)
            //{
            //  Label newLabel = new Label();
            //  newLabel.Location = new Point(c_formerNumberPositionX, formerNumberRowPosition);
            //  newLabel.Text = formerNumberRowPosition.ToString();
            //  this.Controls.Add(newLabel);
            //  m_formerNumberLabelList.Add(newLabel);

            //  formerNumberRowPosition += c_formerNumberRowHeight;
            //}

            //// create new former number lists.
            //for (int i = 0; i < m_formerNumbersAvailable; ++i)
            //{
            //  List<string> newList = new List<string>();
            //  m_formerNumberArray.Add(newList);
            //}

            // Code below was from the load method.

            //UnitsXmlIOController xmlController = UnitsXmlIOController.GetInstance();
            //UnitsIOController unitsController = UnitsIOController.GetInstance();

            //this.subClassImageList = new ObservableCollection<string>();

            //// Load title bar image at start up
            //try
            //{
            //  panelTitleBar.BackgroundImage = Image.FromFile(BasePathReader.GetBasePath() +
            //                                                 StaticResources.imgPath +
            //                                                 "titlebarwide.jpg");
            //}
            //catch (Exception ex)
            //{
            //  Logger l = Logger.Instance;
            //  l.WriteLog("ERROR: ClassConfigForm: Loading - " + ex.ToString());
            //}

            // If the file doesn't exist then leave m_classData, as initialised.
            if (this.unitsXmlIoController.DoesFileExist(this.classId))
            {
                this.classFileConfiguration =
                    this.unitsXmlIoController.Read(
                        this.classId);

                this.version = this.classFileConfiguration.Version;
                this.formation = this.classFileConfiguration.Formation;
                this.alphaIdentifier = this.classFileConfiguration.AlphaId;
                this.year = this.classFileConfiguration.Year;

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
                                    this.unitsIoController,
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
                                    this.unitsIoController,
                                    string.Empty);
                        selector.SelectionMadeEvent += this.UpdateImagesInModel;
                        this.Images.Add(selector);
                    }

                    //this.ClassData =
                    //  this.unitsXmlIoController.ReadClassDetailsXML(
                    //    this.unitsIoController,
                    //    classId);

                    ////if (this.unitsXmlIoController.ReadClassDetailsXML(classId))
                    ////{
                    ////this.ClassData = this.unitsXmlIoController.GetClassData();
                    //this.ClassData?.InitaliseSubClassIndex();

                    //if (this.ClassData != null)
                    //{
                    //    this.ClassData.PropertyChanged += ModelChanged;
                    //}

                    //List<string> imageFileNames = unitsController.GetImageFileList();
                    //foreach (string str in imageFileNames)
                    //{
                    //  this.subClassImageList.Add(str);
                    //}

                    ////          RepopulateClassConfigForm();
                    //this.subClassList = new List<string>();
                    ////      comboBoxSubClass.Items.Clear();
                    //for (int i = 0; i < m_classData.GetSubClassCount(); ++i)
                    //{
                    //  this.subClassList.Add(m_classData.GetSubClass(i));
                    //}
                    //this.subClassListIndex = 0;

                    //this.formation = m_classData.GetFormation();
                    //this.originYear = m_classData.GetYear().ToString();
                    //this.version = m_classData.GetClassVersion().ToString();
                    //this.alphaId = m_classData.GetAlphaIdentifier();
                    //this.subClassImageListIndex = 0;

                    //this.subClassList = new ObservableCollection<SubClassDataType>();
                    //this.currentNumbersList = new ObservableCollection<VehicleNumberType>();
                    //foreach (SubClassDataType subClass in this.ClassData.SubClassList)
                    //{
                    //  this.subClassList.Add(subClass);
                    //}

                    //if (this.subClassList.Count > 0)
                    //{
                    //  this.subClassListIndex = 0;

                    //  foreach (VehicleNumberType number in this.subClassList[subClassListIndex].GetNumberList())
                    //  {
                    //    this.currentNumbersList.Add(number);
                    //  }
                    //}

                    //}
                }
            }
            else
            {
                this.classFileConfiguration = new ClassDetails();
                // it's new, so mark it as too be saved.
                //ClassDataHasChanged();

                // make sure the old number text boxes are properly displayed
                //ShowHideOldNumTextBoxes();
            }

            this.CanSave = false;

            this.SaveCmd = new CommonCommand(this.SaveModel, () => true);
            this.CloseCmd = new CommonCommand(this.CloseWindow);
            this.AddNewSubClassCmd = new CommonCommand(this.AddNewSubClass);
            this.AddNewNumberCmd = new CommonCommand(this.AddNewNumber);
            this.AddNewNumberSeriesCmd = new CommonCommand(this.AddNewNumberSeries);
            this.RenumberCmd = new CommonCommand(this.Renumber);
        }

        ///// <summary>
        ///// List of sub class image lists.
        ///// </summary>
        //public ObservableCollection<string> SubClassImageList
        //{
        //  get
        //  {
        //    return this.subClassImageList;
        //  }

        //  set
        //  {
        //    this.subClassImageList = value;
        //    this.RaisePropertyChangedEvent("SubClassImageList");
        //  }
        //}

        ///// <summary>
        ///// Index for the list of sub class image lists.
        ///// </summary>
        //public int SubClassImageListIndex
        //{
        //  get
        //  {
        //    return this.subClassImageListIndex;
        //  }

        //  set
        //  {
        //    this.subClassImageListIndex = value;
        //    this.RaisePropertyChangedEvent("SubClassImageListIndex");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the class data.
        ///// </summary>
        //public ClassDataType ClassData { get; set; }

        /// <summary>
        /// Gets or sets the version of this file.
        /// </summary>
        public int Version
        {
            get
            {
                return this.version;
            }

            set
            {
                if (this.version != value)
                {
                    return;
                }

                this.version = value;
                this.RaisePropertyChangedEvent(nameof(this.Version));
                this.classFileConfiguration.Version = value;
            }
        }

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

        //public ObservableCollection<VehicleNumberType> CurrentNumbersList
        //{
        //  get
        //  {
        //    return this.currentNumbersList;
        //  }

        //  set
        //  {
        //    this.currentNumbersList = value;
        //    this.RaisePropertyChangedEvent("CurrentNumbersList");
        //  }
        //}

        ///// <summary>
        ///// List of sub class lists.
        ///// </summary>
        //public ObservableCollection<SubClassDataType> SubClassList
        //{
        //  get
        //  {
        //    return this.subClassList;
        //  }

        //  set
        //  {
        //    this.subClassList = value;
        //    this.RaisePropertyChangedEvent("SubClassList");
        //  }
        //}

        /// <summary>
        /// List of sub class image lists.
        /// </summary>
        //public int SubClassListIndex
        //{
        //  get
        //  {
        //    return this.subClassImageListIndex;
        //  }

        //  set
        //  {
        //    this.subClassImageListIndex = value;
        //    this.RaisePropertyChangedEvent("SubClassListIndex");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the formation.
        ///// </summary>
        //public string Formation
        //{
        //  get
        //  {
        //    return this.formation;
        //  }

        //  set
        //  {
        //    this.formation = value;
        //    this.RaisePropertyChangedEvent("Formation");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the origin year.
        ///// </summary>
        //public string OriginYear
        //{
        //  get
        //  {
        //    return this.originYear;
        //  }

        //  set
        //  {
        //    this.originYear = value;
        //    this.RaisePropertyChangedEvent("OriginYear");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the version
        ///// </summary>
        //public string Version
        //{
        //  get
        //  {
        //    return this.version;
        //  }

        //  set
        //  {
        //    this.version = value;
        //    this.RaisePropertyChangedEvent("Version");
        //  }
        //}

        ///// <summary>
        ///// Gets or sets the alpha id.
        ///// </summary>
        //public string AlphaId
        //{
        //  get
        //  {
        //    return this.alphaId;
        //  }

        //  set
        //  {
        //    this.alphaId = value;
        //    this.RaisePropertyChangedEvent("AlphaId");
        //  }
        //}


        //    /// ---------- ---------- ---------- ---------- ---------- ----------
        //    /// <name>repopulateClassConfigForm</name>
        //    /// <date>12/05/13</date>
        //    /// <summary>
        //    ///   Clears the data on the form and then repopulates all 
        //    ///     relevant fields with the data from m_classData. 
        //    /// </summary>
        //    /// ---------- ---------- ---------- ---------- ---------- ----------
        //    public void RepopulateClassConfigForm()
        //    {
        //      this.subClassList = new List<string>();
        ////      comboBoxSubClass.Items.Clear();
        //      for (int i = 0; i < m_classData.GetSubClassCount(); ++i)
        //      {
        //        this.subClassList.Add(m_classData.GetSubClass(i));
        //      }

        //      this.formation = m_classData.GetFormation();
        //      this.originYear = m_classData.GetYear().ToString();
        //      this.version = m_classData.GetClassVersion().ToString();
        //      this.alphaId = m_classData.GetAlphaIdentifier();
        //    }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>GetClassID</name>
        ///// <date>28/04/12</date>
        ///// <summary>
        ///// returns class id.
        ///// </summary>
        ///// <returns>class id</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public string GetClassId()
        //{
        //  return classId;
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>getSubClassCount</name>
        ///// <date>30/09/12</date>
        ///// <summary>
        /////   return m_classData.getSubClassCount()
        ///// </summary>
        ///// <returns>sub class count</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public int GetSubClassCount()
        //{
        //    return this.ClassData.GetSubClassCount();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>getSubClass</name>
        ///// <date>30/09/12</date>
        ///// <summary>
        /////   return m_classData.getSubClassCount()
        ///// </summary>
        ///// <param name="index">sub class index</param>
        ///// <returns>sub class</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public string GetSubClass(int index)
        //{
        //    return this.ClassData.GetSubClass(index);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>getCurrentNumberCount</name>
        ///// <date>05/10/12</date>
        ///// <summary>
        /////   Gets the number of CurrentNumbers stored in m_classData
        ///// </summary>
        ///// <param name="subClassIndex">sub class index</param>
        ///// <returns>current number count</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public int GetCurrentNumberCount(int subClassIndex)
        //{
        //    return this.ClassData.GetCurrentNumberCount(subClassIndex);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>getCurrentNumberCount</name>
        ///// <date>05/10/12</date>
        ///// <summary>
        /////   Gets the current number in m_classData for the subClass at
        /////     subClassIndex and the number at numberIndex.
        ///// </summary>
        ///// <param name="subClassIndex">sub class index</param>
        ///// <param name="numberIndex">number index</param>
        ///// <returns>current number</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public int GetCurrentNumber(int subClassIndex, int numberIndex)
        //{
        //    return this.ClassData.GetCurrentNumber(subClassIndex, numberIndex);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>showHideOldNumTextBoxes</name>
        ///// <date>12/02/13</date>
        ///// <summary>
        /////   Hides the OldNumber check boxes if they are blank and shows
        /////     them if they are populated.
        ///// </summary>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public void ShowHideOldNumTextBoxes()
        //{
        //  for (int i = 0; i < m_formerNumberLabelList.Count(); ++i)
        //  {
        //    if (m_formerNumberLabelList[i].Text == string.Empty)
        //    {
        //      m_formerNumberLabelList[i].Hide();
        //    }
        //    else
        //    {
        //      m_formerNumberLabelList[i].Show();
        //    }
        //  }
        //}

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>setupSubClassImage</name>
        /// <date>13/12/12</date>
        /// <summary>
        ///   Sets the Image Text box and displays whatever it points to 
        ///     in the picture box.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void SetupSubClassImage(int subClassIndex)
        {
            // ****
            // TODO - need to learn how to show an image via mvvm.
            // ****

            //comboBoxSubClassImage.Text = m_classData.GetImagePath(subClassIndex);
            //pictureBoxSubClassImage.Image = null;
            //DisplaySubClassImage();
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>displaySubClassImage</name>
        /// <date>06/05/13</date>
        /// <summary>
        ///   Displays the image corresponding to the comboBoxSubClassImage.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void DisplaySubClassImage()
        {
            //try
            //{
            //  pictureBoxSubClassImage.Image = Image.FromFile(BasePathReader.GetBasePath() +
            //                                                 StaticResources.classImgPath +
            //                                                 comboBoxSubClassImage.Text +
            //                                                 ".jpg");
            //}
            //catch (Exception ex)
            //{
            //  Logger l = Logger.Instance;
            //  l.log("ERROR: ClassConfigForm: Image Flault - " + ex.ToString());
            //}
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>classDataHasChanged</name>
        /// <date>24/02/13</date>
        /// <summary>
        ///   One of the controls has changed. Update the version if needed
        ///     and make a visual note.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void ClassDataHasChanged()
        {
            //ColourResourcesClass colourResources = ColourResourcesClass.GetInstance();
            //int versionNumber = 0;

            //if (!m_classChanged)
            //{
            //  m_classChanged = true;
            //  pictureBoxHasChanged.BackColor = colourResources.ItemChangedColour;

            //  // update the class version to increase by 1.
            //  versionNumber = m_classData.GetClassVersion() + 1;
            //  m_classData.SetClassVersion(versionNumber);
            //  textBoxVersion.Text = versionNumber.ToString();
            //}
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>updateClassImage</name>
        /// <date>13/12/12</date>
        /// <summary>
        ///   The sub class image textbox has changed, change the class
        ///     data member variable and make a note that the class has 
        ///     changed. Change the image.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void UpdateClassImage()
        {
            //if (comboBoxSubClass.SelectedIndex >= 0)
            //{
            //  m_classData.SetImagePath(comboBoxSubClass.SelectedIndex, comboBoxSubClassImage.Text);
            //  DisplaySubClassImage();
            //  ClassDataHasChanged();
            //}
            //else
            //{
            //  // no subclass has been selected. Make a note.
            //  UpdateStatusBarCurrentStatus(c_noSubClassSelectedWarning,
            //                               true);
            //  comboBoxSubClassImage.Text = string.Empty;
            //}
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>updateFormation</name>
        ///// <date>29/09/12</date>
        ///// <summary>
        /////   The formation textbox has changed, change the class
        /////     data member variable and make a note that the class has 
        /////     changed.
        ///// </summary>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void UpdateFormation()
        //{
        //  if (comboBoxSubClass.SelectedIndex >= 0)
        //  {
        //    m_classData.SetFormation(textBoxFormation.Text);
        //    ClassDataHasChanged();
        //  }
        //  else
        //  {
        //    // no subclass has been selected. Make a note.
        //    UpdateStatusBarCurrentStatus(c_noSubClassSelectedWarning,
        //                                 true);
        //    textBoxFormation.Text = string.Empty;
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>updateFormation</name>
        ///// <date>06/05/13</date>
        ///// <summary>
        /////   The formation textbox has changed, change the class
        /////     data member variable and make a note that the class has 
        /////     changed.
        ///// </summary>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void UpdateYear()
        //{
        //  if (comboBoxSubClass.SelectedIndex >= 0)
        //  {
        //    try
        //    {
        //      int intYear = Convert.ToInt32(textBoxYear.Text);
        //      m_classData.SetYear(intYear);
        //      ClassDataHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //      Logger l = Logger.Instance;
        //      l.log("ERROR: ClassConfigForm: updateYear: " + ex.ToString());
        //    }
        //  }
        //  else
        //  {
        //    // no subclass has been selected. Make a note.
        //    UpdateStatusBarCurrentStatus(c_noSubClassSelectedWarning,
        //                                 true);
        //    textBoxYear.Text = string.Empty;
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>updateAlphaId</name>
        ///// <date>12/05/13</date>
        ///// <summary>
        /////   save the alpha Id
        ///// </summary>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void UpdateAlphaId()
        //{
        //  m_classData.SetAlphaIdentifier(textBoxAlphaId.Text);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>updateStatusBarCurrentStatus</name>
        ///// <date>25/02/13</date>
        ///// <summary>
        /////   If warning is true the the foreColour is set to Red, otherwise
        /////     it's black. This method then sets 
        /////     toolStripStatusLabelCurrentStatus to the incoming newValue.
        ///// </summary>
        ///// <param name="newValue">new value</param>
        ///// <param name="warning">warning flag</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void UpdateStatusBarCurrentStatus(string newValue, bool warning)
        //{
        //  ColourResourcesClass colourResources = ColourResourcesClass.GetInstance();

        //  if (warning)
        //  {
        //    toolStripStatusLabelCurrentStatus.ForeColor = colourResources.ErrorColour;
        //  }
        //  else
        //  {
        //    toolStripStatusLabelCurrentStatus.ForeColor = colourResources.FooterForeColour;
        //  }

        //  toolStripStatusLabelCurrentStatus.Text = newValue;
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>populateListBoxNumbersList</name>
        ///// <date>06/05/13</date>
        ///// <summary>
        /////   It searches for all numbers with the same subclass as the 
        /////     new selection and adds them to the list box. For each 
        /////     number it adds any old numbers to the old numbers list.
        /////     Note: currently there can only be 3 old numbers. This is
        /////       hardcoded.
        /////   Finally it selects the first number in the list box.
        ///// </summary>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void PopulateListBoxNumbersList()
        //{
        //  listBoxNumbersList.Items.Clear();

        //  // Clear the old numbers list
        //  for (int i = 0; i < m_formerNumbersAvailable; ++i)
        //  {
        //    m_formerNumberArray[i].Clear();
        //  }

        //  SetupSubClassImage(comboBoxSubClass.SelectedIndex);

        //  int oldNumberCount = 0;

        //  for (int vehIndex = 0; vehIndex < m_classData.GetCurrentNumberCount(comboBoxSubClass.SelectedIndex); ++vehIndex)
        //  {
        //    listBoxNumbersList.Items.Add(m_classData.GetCurrentNumber(comboBoxSubClass.SelectedIndex, vehIndex).ToString());

        //    oldNumberCount = m_classData.GetOldNumberCount(comboBoxSubClass.SelectedIndex, vehIndex);

        //    for (int formerIndex = 0; formerIndex < m_formerNumbersAvailable; ++formerIndex)
        //    {
        //      if (oldNumberCount > formerIndex)
        //      {
        //        m_formerNumberArray[formerIndex].Add(m_classData.GetOldNumber(comboBoxSubClass.SelectedIndex, vehIndex, formerIndex).ToString());
        //      }
        //      else
        //      {
        //        m_formerNumberArray[formerIndex].Add(string.Empty);
        //      }
        //    }
        //  }

        //  // Only select a value if the list box is not empty. 
        //  // If the list box is empty then make sure the old nnumber boxes are empty.
        //  if (listBoxNumbersList.Items.Count > 0)
        //  {
        //    listBoxNumbersList.SetSelected(0, true);
        //  }
        //  else
        //  {
        //    foreach (Label lbl in m_formerNumberLabelList)
        //    {
        //      lbl.Text = string.Empty;
        //    }

        //    ShowHideOldNumTextBoxes();
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>MouseButtonUp</name>
        ///// <date>15/12/12</date>
        ///// <summary>
        /////   mouse down event
        ///// </summary>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void MouseButtonDown(MouseEventArgs e)
        //{
        //  m_mouseDownLocation = new Point(e.X, e.Y);
        //  m_origLocation = new Point(this.Location.X, this.Location.Y);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>MouseButtonUp</name>
        ///// <date>15/12/12</date>
        ///// <summary>
        /////   mouse up event
        ///// </summary>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void MouseButtonUp(MouseEventArgs e)
        //{
        //  this.Location = new Point((e.X - m_mouseDownLocation.X) + m_origLocation.X,
        //                            (e.Y - m_mouseDownLocation.Y) + m_origLocation.Y);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// 28/04/12
        ///// <summary>
        ///// VehicleConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        /////   Close and destroy the form.
        ///// </summary>
        ///// <param name="sender">form object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void VehicleConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //  m_parent.CloseVehicleConfigForm(classId);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>ClassConfigForm_Load</name>
        ///// <date>06/05/13</date>
        ///// <summary>
        /////   Open the form.
        ///// </summary>
        ///// <param name="sender">form object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void ClassConfigForm_Load(object sender, EventArgs e)
        //{
        //  //UnitsXmlIOController xmlController = UnitsXmlIOController.GetInstance();
        //  //UnitsIOController unitsController = UnitsIOController.GetInstance();

        //  //// Load title bar image at start up
        //  //try
        //  //{
        //  //  panelTitleBar.BackgroundImage = Image.FromFile(BasePathReader.GetBasePath() +
        //  //                                                 StaticResources.imgPath +
        //  //                                                 "titlebarwide.jpg");
        //  //}
        //  //catch (Exception ex)
        //  //{
        //  //  Logger l = Logger.Instance;
        //  //  l.log("ERROR: ClassConfigForm: Loading - " + ex.ToString());
        //  //}

        //  //// If the file doesn't exist then leave m_classData, as initialised.
        //  //if (xmlController.DoesFileExist(classId))
        //  //{
        //  //  if (xmlController.ReadClassDetailsXML(classId))
        //  //  {
        //  //    m_classData = xmlController.GetClassData();

        //  //    List<string> imageFileNames = unitsController.GetImageFileList();
        //  //    foreach (string str in imageFileNames)
        //  //    {
        //  //      comboBoxSubClassImage.Items.Add(str);
        //  //    }

        //  //    RepopulateClassConfigForm();
        //  //    comboBoxSubClass.SelectedIndex = 0;
        //  //  }
        //  //}
        //  //else
        //  //{
        //  //  // it's new, so mark it as too be saved.
        //  //  ClassDataHasChanged();

        //  //  // make sure the old number text boxes are properly displayed
        //  //  ShowHideOldNumTextBoxes();
        //  //}
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>buttonClose_Click</name>
        ///// <date>13/11/12</date>
        ///// <summary>
        /////   Close the form.
        ///// </summary>
        ///// <param name="sender">button object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void ButtonClose_Click(object sender, EventArgs e)
        //{
        //  this.Close();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>closeToolStripMenuItem_Click</name>
        ///// <date>13/11/12</date>
        ///// <summary>
        /////   Close the form.
        ///// </summary>
        ///// <param name="sender">menu item object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  this.Close();
        //}

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>comboBoxSubClass_SelectedIndexChanged</name>
        /// <date>26/09/12</date>
        /// <summary>
        ///   Runs when a the when a new value in the subClass combo box is
        ///     selected.
        ///   Calls populateListBoxNumbersList to populate the numbers list
        ///     for the selected subclass
        /// </summary>
        /// <param name="sender">combo box object</param>
        /// <param name="e">event argument</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void ComboBoxSubClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PopulateListBoxNumbersList();
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>ListBoxNumbersList_SelectedIndexChanged</name>
        ///// <date>10/02/13</date>
        ///// <summary>
        /////   Runs when a the when a new value in the numbersList list box
        /////     is selected.
        /////     Populates the three OldNumber text boxes.
        /////     Hides any which are blank and shows the rest.
        ///// </summary>
        ///// <param name="sender">list box object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void ListBoxNumbersList_SelectedIndexChanged(object sender,
        //                                                     EventArgs e)
        //{
        //  for (int i = 0; i < m_formerNumbersAvailable; ++i)
        //  {
        //    m_formerNumberLabelList[i].Text = m_formerNumberArray[i][listBoxNumbersList.SelectedIndex];
        //  }

        //  ShowHideOldNumTextBoxes();
        //}

        /// <date>12/08/18</date>
        /// <summary>
        ///   save model
        /// </summary>
        private void SaveModel()
        {
            ++this.classFileConfiguration.Version;
            this.unitsXmlIoController.Write(
                this.classFileConfiguration,
                this.classId);

            //bool success = true;

            //// create a new file for each new number
            ////IndividualUnitIOController individualUnitController = IndividualUnitIOController.GetInstance();
            //foreach (int vehicleNumber in m_newNumberList)
            //{
            //    VehicleDetailsViewModel newVehicle =
            //      new VehicleDetailsViewModel(
            //        vehicleNumber.ToString());

            //    if (!IndividualUnitIOController.WriteIndividualUnitFile(newVehicle, classId))
            //    {
            //        success = false;
            //        Logger l = Logger.Instance;
            //        l.WriteLog("TRACE: Class Config Form - Save Failed - Failed to write " + newVehicle.UnitNumber.ToString() + ".txt");
            //        break;
            //    }
            //}

            //// renumber all files for each renumbered vehicle
            //for (int i = 0; i < m_renumberedList[0].Count; ++i)
            //{
            //    IndividualUnitIOController.RenameIndividualUnitFile(
            //      m_renumberedList[1][i],
            //      classId,
            //      m_renumberedList[0][i]);
            //}

            //if (success)
            //{
            //    m_newNumberList.Clear();

            //    //UnitsXmlIOController unitsController = UnitsXmlIOController.GetInstance();
            //    //this.unitsXmlIoController.WriteClassDetailsXML(classId, this.ClassData);

            //    m_classChanged = false;
            //    //pictureBoxHasChanged.BackColor = SystemColors.Control;
            //}
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>comboBoxSubClassImage_SelectedIndexChanged</name>
        /// <date>23/09/12</date>
        /// <summary>
        ///   When focus is lost enter the sub class data.
        /// </summary>
        /// <param name="sender">combo box object</param>
        /// <param name="e">event argument</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private void ComboBoxSubClassImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateClassImage();
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxFormation_KeyDown</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   Check for the enter key, if so, enter the formation data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">Key event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxFormation_KeyDown(object sender, KeyEventArgs e)
        //{
        //  if (e.KeyCode == Keys.Enter)
        //  {
        //    UpdateFormation();
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxFormation_Leave</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   When focus is lost enter the formation data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxFormation_Leave(object sender, EventArgs e)
        //{
        //  UpdateFormation();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxYear_KeyDown</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   Check for the enter key, if so, enter the year data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">Key event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxYear_KeyDown(object sender, KeyEventArgs e)
        //{
        //  if (e.KeyCode == Keys.Enter)
        //  {
        //    UpdateYear();
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxYear_Leave</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   When focus is lost enter the year data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxYear_Leave(object sender, EventArgs e)
        //{
        //  UpdateYear();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxAlphaId_KeyDown</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   Enter the alpha identifier data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">Key Event Arguments</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxAlphaId_KeyDown(object sender, KeyEventArgs e)
        //{
        //  UpdateAlphaId();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>textBoxAlphaId_Leave</name>
        ///// <date>12/05/13</date>
        ///// <summary>
        /////   When focus is lost enter the alpha identifier data.
        ///// </summary>
        ///// <param name="sender">text box object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void TextBoxAlphaId_Leave(object sender, EventArgs e)
        //{
        //  UpdateAlphaId();
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>addNewSubClassToolStripMenuItem_Click</name>
        ///// <date>26/09/12</date>
        ///// <summary>
        /////   Open the SubClassDialog and check the return value.
        /////   If return is ok, the the return value is ok. Add it to the 
        /////     sub class, repopulate the form and make a note that the 
        /////     class data is not the currently saved version.
        ///// </summary>
        ///// <param name="sender">tool strip object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void AddNewSubClassToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  List<int> subClassList = new List<int>();
        //  for (int i = 0; i < m_classData.GetSubClassCount(); ++i)
        //  {
        //    subClassList.Add(Convert.ToInt32(m_classData.GetSubClass(i)));
        //  }

        //  NewSubClassDialog result = new NewSubClassDialog(subClassList);
        //  result.ShowDialog();
        //  if (result.DialogResult == DialogResult.OK)
        //  {
        //    m_classData.AddNewSubClass(result.GetSubClass());
        //    m_classData.SortSubClass();
        //    RepopulateClassConfigForm(); //Don't, just add
        //    ClassDataHasChanged();
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>addNewNumberToolStripMenuItem_Click</name>
        ///// <date>03/11/12</date>
        ///// <summary>
        /////   click event
        ///// </summary>
        ///// <param name="sender">tool strip object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void AddNewNumberToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  AddNumberDialog result = new AddNumberDialog(true, this);
        //  int subClassIndex = 0;
        //  bool success = false;

        //  if (m_renumberedList[0].Count == 0)
        //  {
        //    result.ShowDialog();
        //    if (result.DialogResult == DialogResult.OK)
        //    {
        //      subClassIndex = result.GetSubClassIndex();
        //      success = m_classData.AddCurrentNumber(subClassIndex,
        //                                             result.GetNumberFrom());
        //      if (success)
        //      {
        //        m_classData.SortNumbers(subClassIndex);
        //        ClassDataHasChanged();
        //        PopulateListBoxNumbersList();
        //        m_newNumberList.Add(result.GetNumberFrom());
        //      }
        //      else
        //      {
        //        System.Windows.Forms.MessageBox.Show(
        //          result.GetNumberFrom().ToString() + " already exists",
        //          string.Empty,
        //          MessageBoxButtons.OK,
        //          MessageBoxIcon.Information);
        //      }
        //    }
        //  }
        //  else
        //  {
        //    UpdateStatusBarCurrentStatus("Please save renumbered vehicles before creating new ones.", true);
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>addNewNumberSeriesToolStripMenuItem_Click</name>
        ///// <date>04/11/12</date>
        ///// <summary>
        /////   click event
        ///// </summary>
        ///// <param name="sender">tool strip object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void AddNewNumberSeriesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  AddNumberDialog result = new AddNumberDialog(false, this);
        //  int subClassIndex = 0;
        //  bool success = false;
        //  bool showDialog = false;
        //  string dialogString = string.Empty;

        //  if (m_renumberedList[0].Count() == 0)
        //  {
        //    result.ShowDialog();
        //    if (result.DialogResult == DialogResult.OK)
        //    {
        //      subClassIndex = result.GetSubClassIndex();
        //      for (int i = result.GetNumberFrom(); i <= result.GetNumberTo(); i++)
        //      {
        //        success = m_classData.AddCurrentNumber(subClassIndex,
        //                                               i);
        //        if (success)
        //        {
        //          m_classData.SortNumbers(subClassIndex);
        //          ClassDataHasChanged();
        //          PopulateListBoxNumbersList();
        //          m_newNumberList.Add(i);
        //        }
        //        else
        //        {
        //          showDialog = true;
        //          dialogString = dialogString + i.ToString() + " ";
        //        }
        //      }

        //      if (showDialog)
        //      {
        //        System.Windows.Forms.MessageBox.Show(
        //          dialogString + " already exists",
        //          string.Empty,
        //          MessageBoxButtons.OK,
        //          MessageBoxIcon.Information);
        //      }
        //    }
        //  }
        //  else
        //  {
        //    UpdateStatusBarCurrentStatus("Please save renumbered vehicles before creating new ones.", true);
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>addNewNumberSeriesToolStripMenuItem_Click</name>
        ///// <date>06/05/13</date>
        ///// <summary>
        /////   click event 
        ///// </summary>
        ///// <param name="sender">tool strip object</param>
        ///// <param name="e">event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void RenumberToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //  ChangeNumberDialog result = new ChangeNumberDialog(this);
        //  List<int> oldNumberList = new List<int>();
        //  List<int> newNumberList = new List<int>();
        //  List<int> prevNumberList = new List<int>(); // used as a temp store for older numbers
        //  bool numberExists = false;
        //  int failingNumber = 0;
        //  int oldSubClassIndex = 0;
        //  int newSubClassIndex = 0;
        //  int numberIndex = -1;              // used to record the index of the current number in m_classData.

        //  if (m_newNumberList.Count() == 0)
        //  {
        //    result.ShowDialog();
        //    if (result.DialogResult == DialogResult.OK)
        //    {
        //      // get the data from the dialog.
        //      oldNumberList = result.GetOldNumberList();
        //      newNumberList = result.GetNewNumberList();
        //      oldSubClassIndex = result.GetOldSubClassIndex();
        //      newSubClassIndex = result.GetNewSubClassIndex();

        //      // check to see if the new data is valid (number already exists check)
        //      foreach (int newNumber in newNumberList)
        //      {
        //        for (int subClassIndex = 0; subClassIndex < m_classData.GetSubClassCount(); ++subClassIndex)
        //        {
        //          for (int oldNumberIndex = 0; oldNumberIndex < m_classData.GetCurrentNumberCount(subClassIndex); ++oldNumberIndex)
        //          {
        //            if (newNumber == m_classData.GetCurrentNumber(subClassIndex, oldNumberIndex))
        //            {
        //              numberExists = true;
        //              failingNumber = newNumber;
        //              break;
        //            }
        //          }

        //          if (numberExists)
        //          {
        //            break;
        //          }
        //        }

        //        if (numberExists)
        //        {
        //          break;
        //        }
        //      }

        //      if (numberExists)
        //      {
        //        // one of the new numbers already exists so stop.
        //        System.Windows.Forms.MessageBox.Show("Renumber Failed, Number already exists");
        //        Logger l = Logger.Instance;
        //        l.log("TRACE: ClassConfigForm - Renumber Failed, "
        //            + failingNumber.ToString()
        //            + " already exists.");
        //      }
        //      else
        //      {
        //        // loop through each number to be changed as identified by the dialog.
        //        for (int listIndex = 0; listIndex < oldNumberList.Count; ++listIndex)
        //        {
        //          // re-initalise relevant data.
        //          prevNumberList.Clear();
        //          numberIndex = m_classData.GetCurrentNumberIndex(oldSubClassIndex,
        //                                                          oldNumberList[listIndex].ToString());

        //          // get all previous numbers, add the latest old number
        //          for (int prevNumber = 0; prevNumber < m_classData.GetOldNumberCount(oldSubClassIndex, numberIndex); ++prevNumber)
        //          {
        //            prevNumberList.Add(m_classData.GetOldNumberInt(oldSubClassIndex,
        //                                                           numberIndex,
        //                                                           prevNumber));
        //          }

        //          prevNumberList.Add(oldNumberList[listIndex]);

        //          // delete the old number and a the new one as new. It's done this way
        //          // because number may have changed subclasses.
        //          m_classData.DeleteCurrentNumber(oldSubClassIndex,
        //                                          numberIndex);
        //          m_classData.AddCurrentNumber(newSubClassIndex,
        //                                       newNumberList[listIndex]);
        //          foreach (int pN in prevNumberList)
        //          {
        //            m_classData.AddOldNumber(newSubClassIndex,
        //                                     pN,
        //                                     newNumberList[listIndex]);
        //          }

        //          m_classData.SortNumbers(newSubClassIndex);

        //          m_renumberedList[0].Add(oldNumberList[listIndex]);                       // former number
        //          m_renumberedList[1].Add(newNumberList[listIndex]);                       // new number
        //          m_renumberedListSubClass.Add(m_classData.GetSubClass(newSubClassIndex)); // subclass
        //        }

        //        ClassDataHasChanged();
        //        PopulateListBoxNumbersList();
        //      }

        //      // Reopen the dialog
        //      renumberToolStripMenuItem.PerformClick();
        //    }
        //  }
        //  else
        //  {
        //    UpdateStatusBarCurrentStatus("Please save any new vehicles before attempting to renumber them.", true);
        //  }
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>panelTitleBar_MouseDown</name>
        ///// <date>27/12/12</date>
        ///// <summary>
        /////   mouse down event
        ///// </summary>
        ///// <param name="sender">panel object</param>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
        //{
        //  MouseButtonDown(e);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>panelTitleBar_MouseUp</name>
        ///// <date>27/12/12</date>
        ///// <summary>
        /////   mouse up event
        ///// </summary>
        ///// <param name="sender">panel object</param>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void PanelTitleBar_MouseUp(object sender, MouseEventArgs e)
        //{
        //  MouseButtonUp(e);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>labelTitleBar_MouseDown</name>
        ///// <date>27/12/12</date>
        ///// <summary>
        /////   mouse down event
        ///// </summary>
        ///// <param name="sender">label object</param>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void LabelTitleBar_MouseDown(object sender, MouseEventArgs e)
        //{
        //  MouseButtonDown(e);
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>labelTitleBar_MouseUp</name>
        ///// <date>27/12/12</date>
        ///// <summary>
        /////   mouse up event
        ///// </summary>
        ///// <param name="sender">label object</param>
        ///// <param name="e">mouse event argument</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //private void LabelTitleBar_MouseUp(object sender, MouseEventArgs e)
        //{
        //  MouseButtonUp(e);
        //}

        private void ModelChanged(object sender, PropertyChangedEventArgs e)
        {
            //      this.ClassData.Version++;
            this.CanSave = true;
        }

        private bool SaveCmdAvailable()
        {
            return this.CanSave;
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
                //this.ClassData.AddNewSubClass(
                //  new SubClassDataType(
                //    this.unitsIoController,
                //    dialogViewModel.SubClass,
                //    string.Empty));
            }
        }

        /// <summary>
        /// Add new number to a subclass.
        /// </summary>
        private void AddNewNumber()
        {
            //NewNumberViewModel dialogViewModel =
            //  new NewNumberViewModel(
            //    this.ClassData.SubClassNumbers);

            //DialogService service = new DialogService();

            //service.ShowDialog(
            //  new NewNumberDialog()
            //  {
            //      DataContext = dialogViewModel
            //  });

            //this.ClassData.AddCurrentNumber(dialogViewModel.SubClassIndex, dialogViewModel.Number);
            //this.m_newNumberList.Add(dialogViewModel.Number);
        }

        /// <summary>
        /// Add series of new numbers to a subclass.
        /// </summary>
        private void AddNewNumberSeries()
        {
            //NewNumberViewModel dialogViewModel =
            //  new NewNumberViewModel(
            //    this.ClassData.SubClassNumbers,
            //    true);

            //DialogService service = new DialogService();

            //service.ShowDialog(
            //  new NewNumberSeriesDialog()
            //  {
            //      DataContext = dialogViewModel
            //  });

            //for (int number = dialogViewModel.Number; number <= dialogViewModel.UpperNumber; ++number)
            //{
            //    this.ClassData.AddCurrentNumber(dialogViewModel.SubClassIndex, number);
            //    this.m_newNumberList.Add(number);
            //}

        }

        /// <summary>
        /// Renumber things.
        /// </summary>
        private void Renumber()
        {
            //RenumberViewModel dialogViewModel =
            //  new RenumberViewModel(
            //    this.ClassData.SubClassNumbers,
            //    this.ClassData.GetAllNumbers());

            //DialogService service = new DialogService();

            //service.ShowDialog(
            //  new RenumberDialog()
            //  {
            //      DataContext = dialogViewModel
            //  });

            //for (int index = 0; index < dialogViewModel.TotalNumberToChange; ++index)
            //{
            //    if (dialogViewModel.CurrentSubClassNumbersIndex + index < dialogViewModel.CurrentSubClassNumbersList.Count)
            //    {
            //        m_renumberedList[0].Add(dialogViewModel.CurrentSubClassNumbersList[dialogViewModel.CurrentSubClassNumbersIndex + index]);                   // former number
            //        m_renumberedList[1].Add(dialogViewModel.NewNumber + index);                                     // new number
            //        m_renumberedListSubClass.Add(dialogViewModel.SubClasses[dialogViewModel.NewSubClassListIndex]); // subclass

            //        this.ClassData.ReNumber(
            //          dialogViewModel.SubClasses[dialogViewModel.SubClassIndex],
            //          dialogViewModel.CurrentSubClassNumbersList[dialogViewModel.CurrentSubClassNumbersIndex + index],
            //          dialogViewModel.SubClasses[dialogViewModel.NewSubClassListIndex],
            //          dialogViewModel.NewNumber + index);
            //    }
            //}
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
                            this.unitsIoController,
                            imageName);
                selector.SelectionMadeEvent += this.UpdateImagesInModel;
                this.Images.Add(selector);
            }

            this.RaisePropertyChangedEvent(nameof(this.NumbersList));
            this.RaisePropertyChangedEvent(nameof(this.Images));
        }
    }
}