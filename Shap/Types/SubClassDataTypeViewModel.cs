namespace Shap.Types
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Units.IO;
    using Common;

    using Shap.Interfaces.Types;

    public class SubClassDataTypeViewModel : ViewModelBase, ISubClassDataType
    {
        private string imageName;
        private ObservableCollection<VehicleNumberTypeViewModel> m_numberList = new ObservableCollection<VehicleNumberTypeViewModel>();

        private ObservableCollection<string> subClassImageList;
        private int subClassImageListIndex;

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SubClassDataType</name>
        /// <date>27/08/12</date>
        /// <summary>
        /// Creates a new instance of the SubClassDataType.
        /// </summary>
        /// <param name="thisSubClass">this sub class</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public SubClassDataTypeViewModel(
          UnitsIOController unitsIoController,
          string thisSubClass,
          string image)
        {
            this.SubClassNumber = thisSubClass;

            this.subClassImageList = new ObservableCollection<string>();

            List<string> imageFileNames = unitsIoController.GetImageFileList();
            foreach (string str in imageFileNames)
            {
                this.subClassImageList.Add(str);
            }

            this.imageName = image;
            for (int imageIndex = 0; imageIndex < this.SubClassImageList.Count; ++imageIndex)
            {
                if (this.SubClassImageList[imageIndex] == imageName)
                {
                    this.subClassImageListIndex = imageIndex;
                    break;
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SubClassDataTypeViewModel"/> class.
        /// </summary>
        /// <param name="xmlData">data from the file</param>
        public SubClassDataTypeViewModel(Subclass xmlData)
        {
            this.SubClassNumber = xmlData.Type;
            this.subClassImageList = new ObservableCollection<string>();
            this.VehicleNumbersList = new ObservableCollection<VehicleNumberTypeViewModel>();

            foreach(Image fileImage in xmlData.Images)
            {
                this.subClassImageList.Add(fileImage.Name);
            }

            foreach(Number unitNumber in xmlData.Numbers)
            {
                VehicleNumberTypeViewModel newNumber =
                    new VehicleNumberTypeViewModel(
                        unitNumber);

                this.VehicleNumbersList.Add(newNumber);
            }
        }

        /// <summary>
        /// Gets the sub class number.
        /// </summary>
        public string SubClassNumber { get; private set; }

        /// <summary>
        /// List of sub class image lists.
        /// </summary>
        public ObservableCollection<string> SubClassImageList
        {
            get
            {
                return this.subClassImageList;
            }

            set
            {
                this.subClassImageList = value;
                this.RaisePropertyChangedEvent("SubClassImageList");
            }
        }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        public int SubClassImageListIndex
        {
            get
            {
                return this.subClassImageListIndex;
            }

            set
            {
                this.subClassImageListIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.SubClassImageListIndex));
                this.RaisePropertyChangedEvent(nameof(this.SubClassImagePath));
            }
        }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        public string SubClassImagePath
        {
            get
            {
                //      return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                //return "C:\\_myDocs\\bert\\03_projects\\my_programing\\cSharpWPF\\ShapDevelopment\\Shap\\data\\uts\\img\\37.jpg";
                // string returnString = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\" +
                string returnString = BasePathReader.GetBasePathUri() +
                   StaticResources.classImgPath +
                   this.SubClassImageList[this.SubClassImageListIndex] +
                   ".jpg";

                return returnString;
            }
        }

        /// <summary>
        /// Name of the image file used.
        /// </summary>
        public string ImageName
        {
            get
            {
                return this.imageName;
            }

            set
            {
                this.imageName = value;
                this.RaisePropertyChangedEvent("ImageName");
            }
        }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        public ObservableCollection<VehicleNumberTypeViewModel> VehicleNumbersList
        {
            get
            {
                return this.m_numberList;
            }

            private set
            {
                this.m_numberList = value;
                this.RaisePropertyChangedEvent("VehicleNumberList");
                this.RaisePropertyChangedEvent("NumbersList");
            }
        }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        public ObservableCollection<string> NumbersList
        {
            get
            {
                ObservableCollection<string> numbers = new ObservableCollection<string>();

                foreach (VehicleNumberTypeViewModel number in this.VehicleNumbersList)
                {
                    numbers.Add(number.VehicleNumber.ToString());
                }

                return numbers;
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>addNewNumberToolStripMenuItem_Click</name>
        /// <date>30/09/12</date>
        /// <summary>
        ///   addCurrentNumber, append an new number to the m_numberList
        ///   It first checks to see if the number doesn't already exist.
        /// </summary>
        /// <param name="currentNumber">current number</param>
        /// <returns>success flag</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public bool AddCurrentNumber(int currentNumber)
        {
            bool success = true;

            foreach (VehicleNumberTypeViewModel number in this.VehicleNumbersList)
            {
                if (number.VehicleNumber == currentNumber)
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                VehicleNumberTypeViewModel localVehicleNumber = new VehicleNumberTypeViewModel(currentNumber);
                this.VehicleNumbersList.Add(localVehicleNumber);
                this.RaisePropertyChangedEvent("VehicleNumbersList");
                this.RaisePropertyChangedEvent("NumbersList");
            }

            return success;
        }

        public void AddNumber(VehicleNumberTypeViewModel newNumber)
        {
            this.VehicleNumbersList.Add(newNumber);
            this.RaisePropertyChangedEvent("VehicleNumberList");
            this.RaisePropertyChangedEvent("NumbersList");
        }

        public void RemoveNumber(VehicleNumberTypeViewModel oldNumber)
        {
            this.VehicleNumbersList.Remove(oldNumber);
            this.RaisePropertyChangedEvent("VehicleNumberList");
            this.RaisePropertyChangedEvent("NumbersList");
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AddOldNumber</name>
        /// <date>27/08/12</date>
        /// <summary>
        /// append an new oldNumber to the specified currentNumber.
        /// </summary>
        /// <param name="oldNumber">old number</param>
        /// <param name="currentNumber">current number</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public void AddOldNumber(int oldNumber,
                                 int currentNumber)
        {
            for (int i = 0; i < m_numberList.Count; i++)
            {
                if (currentNumber == m_numberList[i].VehicleNumber)
                {
                    m_numberList[i].AddFormerNumber(oldNumber);
                    return;
                }
            }
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>setImagePath</name>
        ///// <date>23/09/12</date>
        ///// <summary>
        /////   Adds the Image Path.
        ///// </summary>
        ///// <param name="path">image path</param>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public void SetImagePath(string path)
        //{
        //  imageName = path;
        //}

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetCurrentNumber</name>
        /// <date>07/09/12</date>
        /// <summary>
        ///   returns the vehicle number at the specified index.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <returns>current number</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetCurrentNumber(int numberIndex)
        {
            return m_numberList[numberIndex].VehicleNumber;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetCurrentNumberCount</name>
        /// <date>27/08/12</date>
        /// <summary>
        ///   returns the length of the numberList
        /// </summary>
        /// <returns>number count</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetCurrentNumberCount()
        {
            return m_numberList.Count();
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>getCurrentNumberIndex</name>
        /// <date>07/10/12</date>
        /// <summary>
        ///   The current number is known, but what index is it?
        /// </summary>
        /// <param name="currentNumber">current number</param>
        /// <returns>current number index</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetCurrentNumberIndex(string currentNumber)
        {
            for (int index = 0; index < m_numberList.Count(); ++index)
            {
                if (m_numberList[index].VehicleNumber.ToString() == currentNumber)
                {
                    return index;
                }
            }

            return -1;
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>GetSubClass</name>
        ///// <date>27/08/12</date>
        ///// <summary>
        /////   returns the subClass
        ///// </summary>
        ///// <returns>sub class</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public string GetSubClass()
        //{
        //  return subClassNumber;
        //}

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>GetSubClass</name>
        ///// <date>27/08/12</date>
        ///// <summary>
        /////   returns the list of numbers
        ///// </summary>
        ///// <returns>number list</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public List<VehicleNumberType> GetNumberList()
        //{
        //  return m_numberList;
        //}

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetOldNumber</name>
        /// <date>27/08/12</date>
        /// <summary>
        ///   returns the oldNumber at the specified current number.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <param name="oldNumberIndex">old number index</param>
        /// <returns>old number</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetOldNumber(int numberIndex,
                                int oldNumberIndex)
        {
            return this.VehicleNumbersList[numberIndex].FormerNumbers[oldNumberIndex];
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetOldNumber</name>
        /// <date>27/08/12</date>
        /// <summary>
        ///   returns the number of oldNumbers for the specified current
        ///   number.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <returns>old number count</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetOldNumberCount(int numberIndex)
        {
            return VehicleNumbersList[numberIndex].GetNumberOfFormerNumbers();
        }

        ///// ---------- ---------- ---------- ---------- ---------- ----------
        ///// <name>getImagePath</name>
        ///// <date>08/09/12</date>
        ///// <summary>
        /////   Returns the Image Path.
        ///// </summary>
        ///// <returns>image path</returns>
        ///// ---------- ---------- ---------- ---------- ---------- ----------
        //public string GetImagePath()
        //{
        //  return imageName;
        //}

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>sortNumbers</name>
        /// <date>30/09/12</date>
        /// <summary>
        ///   Sorts the m_subClassList by subclass number.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public void SortNumbers()
        {
            m_numberList =
             new ObservableCollection<VehicleNumberTypeViewModel>(
               from i in m_numberList orderby i.VehicleNumber select i);
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>deleteCurrentNumber</name>
        /// <date>07/10/12</date>
        /// <summary>
        ///   deletes number.
        /// </summary>
        /// <param name="currentNumberIndex">current number index</param>
        /// --------- ---------- ---------- ---------- ---------- ----------
        public void DeleteCurrentNumber(int currentNumberIndex)
        {
            m_numberList.RemoveAt(currentNumberIndex);
        }

        /// <summary>
        /// Gets a list of all the current numbers in this subclass.
        /// </summary>
        /// <returns>list of current numbers</returns>
        public ObservableCollection<int> GetAllNumbers()
        {
            ObservableCollection<int> numberList = new ObservableCollection<int>();

            foreach (VehicleNumberTypeViewModel vehicleNumbers in this.VehicleNumbersList)
            {
                numberList.Add(vehicleNumbers.VehicleNumber);
            }

            return numberList;
        }
    }
}