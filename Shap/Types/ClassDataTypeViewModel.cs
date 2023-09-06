namespace Shap.Types
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Shap.Common.SerialiseModel.ClassDetails;
    using NynaeveLib.ViewModel;

    /// <summary>
    /// View model used to support the class data.
    /// </summary>
    public class ClassDataTypeViewModel : ViewModelBase
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        private string myClass;

        /// <summary>
        /// Version of the class data file.
        /// </summary>
        private int classVersion;

        /// <summary>
        /// Introduction year.
        /// </summary>
        private int year;

        /// <summary>
        /// Class formation.
        /// </summary>
        private string formation;

        /// <summary>
        /// Class alpha identifier
        /// </summary>
        private string alphaIdentifier;

        /// <summary>
        /// Collection of sub classes.
        /// </summary>
        private ObservableCollection<SubClassDataTypeViewModel> subClassList;

        /// <summary>
        /// Index of the selected sub class.
        /// </summary>
        private int subClassListIndex;

        /// <summary>
        /// Initialise a new instance of the <see cref="ClassDataTypeViewModel"/> class.
        /// </summary>
        /// <param name="thisClass">this class</param>
        public ClassDataTypeViewModel(string thisClass)
        {
            this.myClass = thisClass;
            this.subClassList = new ObservableCollection<SubClassDataTypeViewModel>();

            this.classVersion = 0;
            this.year = 0;
            this.formation = string.Empty;
            this.alphaIdentifier = string.Empty;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ClassDataTypeViewModel"/> class.
        /// </summary>
        /// <param name="xmlData">data from file</param>
        public ClassDataTypeViewModel(ClassDetails xmlData)
        {
            this.myClass = xmlData.Id;
            this.classVersion = xmlData.Version;
            this.year = xmlData.Year;
            this.formation = xmlData.Formation;
            this.alphaIdentifier = xmlData.AlphaId;

            this.subClassList = new ObservableCollection<SubClassDataTypeViewModel>();

            foreach(Subclass subclass in xmlData.Subclasses)
            {
                SubClassDataTypeViewModel viewModel =
                    new SubClassDataTypeViewModel(
                        subclass);
                this.subClassList.Add(viewModel);
            }
        }

        public string MyClass
        {
            get
            {
                return this.myClass;
            }

            set
            {
                this.myClass = value;
                this.OnPropertyChanged("MyClass");
            }
        }

        public int Version
        {
            get
            {
                return this.classVersion;
            }

            set
            {
                this.classVersion = value;
                this.OnPropertyChanged("Version");
            }
        }

        public int Year
        {
            get
            {
                return this.year;
            }

            set
            {
                this.year = value;
                this.OnPropertyChanged("Year");
            }
        }

        public string Formation
        {
            get
            {
                return this.formation;
            }

            set
            {
                this.formation = value;
                this.OnPropertyChanged("Formation");
            }
        }

        public string AlphaIdentifier
        {
            get
            {
                return this.alphaIdentifier;
            }

            private set
            {
                this.alphaIdentifier = value;
                this.OnPropertyChanged("AlphaIdentifier");
            }
        }

        public ObservableCollection<SubClassDataTypeViewModel> SubClassList
        {
            get
            {
                return this.subClassList;
            }

            set
            {
                this.subClassList = value;
                this.OnPropertyChanged("SubClassList");
                this.OnPropertyChanged("SubClassNumbers");
            }
        }

        public ObservableCollection<string> SubClassNumbers
        {
            get
            {
                ObservableCollection<string> subClassNumbers = new ObservableCollection<string>();

                foreach (SubClassDataTypeViewModel subClass in this.SubClassList)
                {
                    subClassNumbers.Add(subClass.SubClassNumber);
                }

                return subClassNumbers;
            }
        }

        public int SubClassListIndex
        {
            get
            {
                return this.subClassListIndex;
            }

            private set
            {
                this.subClassListIndex = value;
                this.OnPropertyChanged("SubClassListIndex");
                this.OnPropertyChanged("SelectedSubClass");
            }
        }

        public SubClassDataTypeViewModel SelectedSubClass => this.SubClassList[this.subClassListIndex];

        ///// <summary>
        ///// setClassVersion, set classVersion.
        ///// </summary>
        ///// <param name="classVersion">class version</param>
        //public void SetClassVersion(int classVersion)
        //{
        //  m_classVersion = classVersion;
        //}

        ///// <summary>
        ///// setYear, set year.
        ///// </summary>
        ///// <param name="year">new year</param>
        //public void SetYear(int year)
        //{
        //  m_year = year;
        //}

        ///// <summary>
        ///// setFormation, set formation.
        ///// </summary>
        ///// <param name="formation">new formation</param>
        //public void SetFormation(string formation)
        //{
        //  m_formation = formation;
        //}

        ///// <summary>
        /////   set the current version.
        ///// </summary>
        ///// <param name="version">current version</param>
        //public void SetVersion(int version)
        //{
        //  m_classVersion = version;
        //}

        /// <summary>
        ///   set the alpha id
        /// </summary>
        /// <param name="alphaId">alpha id</param>
        public void SetAlphaIdentifier(string alphaId)
        {
            if (alphaId == null)
            {
                this.AlphaIdentifier = string.Empty;
            }
            else
            {
                this.AlphaIdentifier = alphaId;
            }
        }

        /// <summary>
        /// addNewSubClass, add a new subClass.
        /// </summary>
        /// <param name="subClass">sub class</param>
        public void AddNewSubClass(SubClassDataTypeViewModel subClass)
        {
            //SubClassDataType localSubClass = new SubClassDataType(subClass);
            subClass.PropertyChanged += this.SubClassChanged;
            this.SubClassList.Add(subClass);
            this.SubClassList = new ObservableCollection<SubClassDataTypeViewModel>(from i in this.SubClassList orderby i.SubClassNumber select i);
        }

        /// <summary>
        ///   Sets the Image Path.= to the subClass indicated by 
        ///     subClassIndex.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="path">image path</param>
        public void SetImagePath(int subClassIndex,
                                 string path)
        {
            subClassList[subClassIndex].ImageName = path;
        }


        /// <summary>
        ///   addCurrentNumber, add a new currentNumber to the subClass
        ///     indicated by subClassIndex.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="currentNumber">current number</param>
        /// <returns>success flag</returns>
        public void AddCurrentNumber(int subClassIndex,
                                     int currentNumber)
        {
            this.SubClassList[subClassIndex].AddCurrentNumber(currentNumber);
        }

        /// <summary>
        /// addOldNumber, append an old number to the relevant current 
        ///   Number.
        ///   The numbers are stored in their respective subClass.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="oldNumber">old number</param>
        /// <param name="currentNumber">current number</param>
        public void AddOldNumber(int subClassIndex,
                                 int oldNumber,
                                 int currentNumber)
        {
            subClassList[subClassIndex].AddOldNumber(oldNumber, currentNumber);
        }

        /// <summary>
        /// getSubClassCount - returns the number of entries in the subClass
        ///   list;
        /// </summary>
        /// <returns>number of sub classes</returns>
        public int GetSubClassCount()
        {
            return subClassList.Count();
        }

        /// <summary>
        /// getSubClass - returns the subClass data for the position 
        ///   indicated by the incoming argument.
        /// </summary>
        /// <param name="index">sub class index</param>
        /// <returns>sub class</returns>
        public string GetSubClass(int index)
        {
            return subClassList[index].SubClassNumber;
        }

        /// <summary>
        /// getSubClass - returns all SubClass data.
        /// </summary>
        /// <returns>sub class index</returns>
        public ObservableCollection<SubClassDataTypeViewModel> GetSubClassList()
        {
            return subClassList;
        }

        /// <summary>
        ///   Returns the Image Path.
        /// </summary>
        /// <param name="index">sub class index</param>
        /// <returns>image path</returns>
        public string GetImagePath(int index)
        {
            return subClassList[index].ImageName;
        }

        /// <summary>
        /// Sets the sub class index to the first value if available.
        /// </summary>
        public void InitaliseSubClassIndex()
        {
            if (this.SubClassList.Count > 0)
            {
                this.SubClassListIndex = 0;
            }
        }

        ///// <summary>
        ///// getYear - returns year.
        ///// </summary>
        ///// <returns>returns year</returns>
        //public int GetYear()
        //{
        //  return m_year;
        //}

        ///// <summary>
        ///// getFormation - returns the formation.
        ///// </summary>
        ///// <returns>returns formation</returns>
        //public string GetFormation()
        //{
        //  return m_formation;
        //}

        ///// <summary>
        /////   returns class version
        ///// </summary>
        ///// <returns>class version</returns>
        //public int GetClassVersion()
        //{
        //  return m_classVersion;
        //}

        ///// <summary>
        /////   returns alpha id
        ///// </summary>
        ///// <returns>alpha id</returns>
        //public string GetAlphaIdentifier()
        //{
        //  return m_alphaIdentifier;
        //}

        /// <summary>
        /// getCurrentNumber, get the currentNumber for the index provided.
        ///   The numbers are stored in their respective subClass.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="numberIndex">number index</param>
        /// <returns>current number</returns>
        public int GetCurrentNumber(int subClassIndex, int numberIndex)
        {
            return subClassList[subClassIndex].GetCurrentNumber(numberIndex);
        }

        /// <summary>
        /// getCurrentNumberCount, gets the total number of numbers in a 
        ///   specified subClass.
        ///   The numbers are stored in their respective subClass.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <returns>current number index</returns>
        public int GetCurrentNumberCount(int subClassIndex)
        {
            return subClassList[subClassIndex].GetCurrentNumberCount();
        }

        /// <summary>
        ///   The current number is known, but what index is it?
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="currentNumber">current number</param>
        /// <returns name="currentNumberIndex">current number index</returns>
        public int GetCurrentNumberIndex(int subClassIndex,
                                         string currentNumber)
        {
            return subClassList[subClassIndex].GetCurrentNumberIndex(currentNumber);
        }

        /// <summary>
        /// getOldNumber, gets the OldNumber at the specified position, 
        ///   firstly in the the subClass then in the number list.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="numberIndex">number index</param>
        /// <param name="oldNumberIndex">old number index</param>
        /// <returns>old number</returns>
        public string GetOldNumber(int subClassIndex,
                                   int numberIndex,
                                   int oldNumberIndex)
        {
            return subClassList[subClassIndex].GetOldNumber(numberIndex, oldNumberIndex).ToString();
        }

        /// <summary>
        ///   gets the OldNumber at the specified position, 
        ///   firstly in the the subClass then in the number list.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="numberIndex">number index</param>
        /// <param name="oldNumberIndex">old number index</param>
        /// <returns>old number</returns>
        public int GetOldNumberInt(int subClassIndex,
                                   int numberIndex,
                                   int oldNumberIndex)
        {
            return subClassList[subClassIndex].GetOldNumber(numberIndex, oldNumberIndex);
        }

        /// <summary>
        /// getOldNumberCount, gets the number of oldNumbers associated
        ///   with a specific current number. The current numbers are 
        ///   stored in their respective subClass.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="numberIndex">number Index</param>
        /// <returns>number of old numbers</returns>
        public int GetOldNumberCount(int subClassIndex,
                                     int numberIndex)
        {
            return subClassList[subClassIndex].GetOldNumberCount(numberIndex);
        }

        public void ReNumber(string oldSubClass, int oldNumber, string newSubClass, int newNumber)
        {
            int oldSubClassIndex = -1;
            int newSubClassIndex = -1;

            for (int s = 0; s < this.SubClassList.Count; ++s)
            {
                if (this.SubClassList[s].SubClassNumber == oldSubClass)
                {
                    oldSubClassIndex = s;
                }

                if (this.SubClassList[s].SubClassNumber == newSubClass)
                {
                    newSubClassIndex = s;
                }
            }

            if (oldSubClassIndex < 0 || newSubClassIndex < 0)
            {
                return;
            }

            foreach (VehicleNumberTypeViewModel vehicle in this.SubClassList[oldSubClassIndex].VehicleNumbersList)
            {
                if (vehicle.VehicleNumber == oldNumber)
                {
                    vehicle.AddFormerNumber(vehicle.VehicleNumber);
                    vehicle.VehicleNumber = newNumber;

                    if (oldSubClassIndex != newSubClassIndex)
                    {
                        this.SubClassList[newSubClassIndex].AddNumber(vehicle);
                        this.SubClassList[oldSubClassIndex].RemoveNumber(vehicle);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///   Sorts the m_subClassList by subclass number.
        /// </summary>
        public void SortSubClass()
        {
            subClassList =
              new ObservableCollection<SubClassDataTypeViewModel>(
                from i in subClassList orderby i.SubClassNumber select i);
        }

        /// <summary>
        ///   Sorts the m_subClassList by subclass number.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        public void SortNumbers(int subClassIndex)
        {
            subClassList[subClassIndex].SortNumbers();
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>deleteCurrentNumber</name>
        /// <date>07/10/12</date>
        /// <summary>
        ///   deletes number.
        /// </summary>
        /// <param name="subClassIndex">sub class index</param>
        /// <param name="currentNumberIndex">current number index</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public void DeleteCurrentNumber(int subClassIndex,
                                        int currentNumberIndex)
        {
            subClassList[subClassIndex].DeleteCurrentNumber(currentNumberIndex);
        }

        /// <summary>
        /// Gets a list of all the current numbers in the class by sub class.
        /// </summary>
        /// <returns>list of all numbers</returns>
        public List<ObservableCollection<int>> GetAllNumbers()
        {
            List<ObservableCollection<int>> numbersList = new List<ObservableCollection<int>>();

            foreach (SubClassDataTypeViewModel subClass in this.SubClassList)
            {
                numbersList.Add(subClass.GetAllNumbers());
            }

            return numbersList;
        }

        private void SubClassChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged("SubClassList");
        }

    }
}