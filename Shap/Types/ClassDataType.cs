namespace Shap.Types
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using NynaeveLib.ViewModel;

  public class ClassDataType : ViewModelBase
  {
    private string m_myClass;
    private int m_classVersion;
    private int m_year;
    private string m_formation;
    private string m_alphaIdentifier;
    private ObservableCollection<SubClassDataType> m_subClassList;
    private int subClassListIndex;

    /// <summary>
    /// Initialise a new instance of the <see cref="ClassDataType"/> class.
    /// </summary>
    /// <param name="thisClass">this class</param>
    public ClassDataType(string thisClass)
    {
      this.m_myClass = thisClass;
      this.m_subClassList = new ObservableCollection<SubClassDataType>();

      this.m_classVersion = 0;
      this.m_year = 0;
      this.m_formation = string.Empty;
      this.m_alphaIdentifier = string.Empty;
  }

    public string MyClass
    {
      get
      {
        return this.m_myClass;
      }

      set
      {
        this.m_myClass = value;
        this.RaisePropertyChangedEvent("MyClass");
      }
    }

    public int Version
    {
      get
      {
        return this.m_classVersion;
      }

      set
      {
        this.m_classVersion = value;
        this.RaisePropertyChangedEvent("Version");
      }
    }

    public int Year
    {
      get
      {
        return this.m_year;
      }

      set
      {
        this.m_year = value;
        this.RaisePropertyChangedEvent("Year");
      }
    }

    public string Formation
    {
      get
      {
        return this.m_formation;
      }

      set
      {
        this.m_formation = value;
        this.RaisePropertyChangedEvent("Formation");
      }
    }

    public string AlphaIdentifier
    {
      get
      {
        return this.m_alphaIdentifier;
      }

      private set
      {
        this.m_alphaIdentifier = value;
        this.RaisePropertyChangedEvent("AlphaIdentifier");
      }
    }

    public ObservableCollection<SubClassDataType> SubClassList
    {
      get
      {
        return this.m_subClassList;
      }

      set
      {
        this.m_subClassList = value;
        this.RaisePropertyChangedEvent("SubClassList");
        this.RaisePropertyChangedEvent("SubClassNumbers");
      }
    }

    public ObservableCollection<string> SubClassNumbers
    {
      get
      {
        ObservableCollection<string> subClassNumbers = new ObservableCollection<string>();

        foreach (SubClassDataType subClass in this.SubClassList)
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
        this.RaisePropertyChangedEvent("SubClassListIndex");
        this.RaisePropertyChangedEvent("SelectedSubClass");
      }
    }

    public SubClassDataType SelectedSubClass => this.SubClassList[this.subClassListIndex];

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
    public void AddNewSubClass(SubClassDataType subClass)
    {
      //SubClassDataType localSubClass = new SubClassDataType(subClass);
      subClass.PropertyChanged += this.SubClassChanged;
      this.SubClassList.Add(subClass);
      this.SubClassList = new ObservableCollection<SubClassDataType>(from i in this.SubClassList orderby i.SubClassNumber select i);
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
      m_subClassList[subClassIndex].ImageName = path;
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
      m_subClassList[subClassIndex].AddOldNumber(oldNumber, currentNumber);
    }

    /// <summary>
    /// getSubClassCount - returns the number of entries in the subClass
    ///   list;
    /// </summary>
    /// <returns>number of sub classes</returns>
    public int GetSubClassCount()
    {
      return m_subClassList.Count();
    }

    /// <summary>
    /// getSubClass - returns the subClass data for the position 
    ///   indicated by the incoming argument.
    /// </summary>
    /// <param name="index">sub class index</param>
    /// <returns>sub class</returns>
    public string GetSubClass(int index)
    {
      return m_subClassList[index].SubClassNumber;
    }

    /// <summary>
    /// getSubClass - returns all SubClass data.
    /// </summary>
    /// <returns>sub class index</returns>
    public ObservableCollection<SubClassDataType> GetSubClassList()
    {
      return m_subClassList;
    }

    /// <summary>
    ///   Returns the Image Path.
    /// </summary>
    /// <param name="index">sub class index</param>
    /// <returns>image path</returns>
    public string GetImagePath(int index)
    {
      return m_subClassList[index].ImageName;
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
      return m_subClassList[subClassIndex].GetCurrentNumber(numberIndex);
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
      return m_subClassList[subClassIndex].GetCurrentNumberCount();
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
      return m_subClassList[subClassIndex].GetCurrentNumberIndex(currentNumber);
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
      return m_subClassList[subClassIndex].GetOldNumber(numberIndex, oldNumberIndex).ToString();
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
      return m_subClassList[subClassIndex].GetOldNumber(numberIndex, oldNumberIndex);
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
      return m_subClassList[subClassIndex].GetOldNumberCount(numberIndex);
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

      foreach (VehicleNumberType vehicle in this.SubClassList[oldSubClassIndex].VehicleNumbersList)
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
      m_subClassList =
        new ObservableCollection<SubClassDataType>(
          from i in m_subClassList orderby i.SubClassNumber select i);
    }

    /// <summary>
    ///   Sorts the m_subClassList by subclass number.
    /// </summary>
    /// <param name="subClassIndex">sub class index</param>
    public void SortNumbers(int subClassIndex)
    {
      m_subClassList[subClassIndex].SortNumbers();
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
      m_subClassList[subClassIndex].DeleteCurrentNumber(currentNumberIndex);
    }

    /// <summary>
    /// Gets a list of all the current numbers in the class by sub class.
    /// </summary>
    /// <returns>list of all numbers</returns>
    public List<ObservableCollection<int>> GetAllNumbers()
    {
      List<ObservableCollection<int>> numbersList = new List<ObservableCollection<int>>();

      foreach (SubClassDataType subClass in this.SubClassList)
      {
        numbersList.Add(subClass.GetAllNumbers());
      }

      return numbersList;
    }

    private void SubClassChanged(object sender, PropertyChangedEventArgs e)
    {
      this.RaisePropertyChangedEvent("SubClassList");
    }

  }
}