namespace Shap.Stats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Shap.Common;
    using Shap.Input.Factories;
    using Shap.Interfaces.Stats;
    using Shap.Interfaces.Types;
    using Shap.Types;

    // TODO - Singleton? What is each list, refactor and rename.
    // TODO high priority
    public class FirstExampleManager : IFirstExampleManager
    {
        private static FirstExampleManager m_instance = null;

        private List<FirstExampleType> m_completeListNumber = new List<FirstExampleType>(); // contains complete list, always available
        private List<FirstExampleType> m_completeListLocation = new List<FirstExampleType>();
        private List<FirstExampleType> m_annualListNumber = new List<FirstExampleType>(); // used for the input form
        private List<FirstExampleType> m_annualListLocation = new List<FirstExampleType>();
        private List<FirstExampleType> m_listNumber = new List<FirstExampleType>(); // all purpose lists
        private List<FirstExampleType> m_listLocation = new List<FirstExampleType>();

        ///// <summary>
        ///// Manages rading of the individual jnys.
        ///// </summary>
        //private IDailyInputFactory dayFactory;

        /// <summary>
        ///   Creates an instance of the FirstExampleManager class.
        /// </summary>
        public FirstExampleManager()
        {
            LoadCompleteList();
            this.AllPurposeYear = string.Empty;
        }

        ///// <summary>
        /////   Gets an instance of this class.
        ///// </summary>
        ///// <returns name="_instance">instance of this class</returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //public static FirstExampleManager GetInstance()
        //{
        //  if (m_instance == null)
        //  {
        //    m_instance = new FirstExampleManager();
        //  }

        //  return m_instance;
        //}

        /// <summary>
        /// Gets or sets the currently selected all purpose year.
        /// </summary>
        public string AllPurposeYear { get; private set; }

        /// <summary>
        /// Loops through each journey to see if all the components current exist with the first
        /// examples lists. If not, they are added.
        /// </summary>
        /// <param name="jnyList"></param>
        public void CheckNewJnyList(
          List<IJourneyDetailsType> jnyList)
        {
            int rowIndex = 0;

            foreach (IJourneyDetailsType journey in jnyList)
            {
                this.CheckNewJnyListLocation(
                  journey.From,
                  journey.JnyId.Date,
                  rowIndex.ToString());
                this.CheckNewJnyListLocation(
                  journey.To,
                  journey.JnyId.Date,
                  rowIndex.ToString());

                foreach (string unit in journey.Units)
                {
                    this.CheckNewJnyListVcl(
                      unit,
                      journey.JnyId.Date,
                      rowIndex.ToString());
                }

                ++rowIndex;
            }
        }

        private void CheckNewJnyListLocation(
          string location,
          DateTime date,
          string rowIndex)
        {
            if (this.IsCopLocation(location, LocalListType.annual))
            {
                FirstExampleType firstExample =
                  new FirstExampleType()
                  {
                      Item = location,
                      Date = date,
                      Index = rowIndex
                  };

                this.AppendLocation(
                  firstExample,
                  LocalListType.annual,
                  date.Year.ToString());
            }

            if (this.IsCopLocation(location, LocalListType.complete))
            {
                FirstExampleType firstExample =
                  new FirstExampleType()
                  {
                      Item = location,
                      Date = date,
                      Index = rowIndex
                  };

                this.AppendLocation(
                  firstExample,
                  LocalListType.complete);
            }
        }

        private void CheckNewJnyListVcl(
          string vehicle,
          DateTime date,
          string rowIndex)
        {
            if (string.IsNullOrEmpty(vehicle))
            {
                return;
            }

            if (this.IsCopNumber(vehicle, LocalListType.annual))
            {
                FirstExampleType firstExample =
                new FirstExampleType()
                {
                    Item = vehicle,
                    Date = date,
                    Index = rowIndex
                };

                this.AppendNumber(
                  firstExample,
                  LocalListType.annual,
                  date.Year.ToString());
            }

            if (this.IsCopNumber(vehicle, LocalListType.complete))
            {
                FirstExampleType firstExample =
                new FirstExampleType()
                {
                    Item = vehicle,
                    Date = date,
                    Index = rowIndex
                };

                this.AppendNumber(
                  firstExample,
                  LocalListType.complete);
            }
        }

        // ---------- ---------- ---------- ---------- ---------- ----------
        // The following methods load the lists.
        // ---------- ---------- ---------- ---------- ---------- ----------

        /// <summary>
        ///   loads complete list.
        /// </summary>
        public void LoadCompleteList()
        {
            FirstExampleIOController firstExampleController = FirstExampleIOController.GetInstance();

            m_completeListNumber.Clear();
            m_completeListNumber = firstExampleController.GetFirstExampleListNumber();
            m_completeListLocation.Clear();
            m_completeListLocation = firstExampleController.GetFirstExampleListLocation();
        }

        /// <summary>
        /// loads the annual lists for the provided year.
        /// </summary>
        /// <param name="year">year to load the lists for.</param>
        public void LoadAnnualList(string year)
        {
            FirstExampleIOController firstExampleController = FirstExampleIOController.GetInstance();

            m_annualListNumber.Clear();
            m_annualListNumber = firstExampleController.GetFirstExampleListNumber(year);
            m_annualListLocation.Clear();
            m_annualListLocation = firstExampleController.GetFirstExampleListLocation(year);
        }

        /// <summary>
        ///   loads complete list.
        /// </summary>
        /// <remarks>
        /// 04/06/16 BS: This looks like the annual list? What is the difference.
        /// </remarks>
        /// <param name="year">current year</param>
        public void LoadAllPurposeList(string year)
        {
            // TODO, check out the remarks when ported.

            if (string.Compare(year, this.AllPurposeYear) != 0)
            {
                this.AllPurposeYear = year;
                FirstExampleIOController firstExampleController = FirstExampleIOController.GetInstance();

                m_listNumber.Clear();
                m_listNumber = firstExampleController.GetFirstExampleListNumber(year);
                m_listLocation.Clear();
                m_listLocation = firstExampleController.GetFirstExampleListLocation(year);
            }
        }

        // ---------- ---------- ---------- ---------- ---------- ----------
        // The following methods are used to determine if the argument
        //   item already exists in the list defined by listType
        // ---------- ---------- ---------- ---------- ---------- ----------

        /// <summary>
        ///   does number already exist in the list specified by listType.
        /// </summary>
        /// <param name="number">number to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>success flag</returns>
        public bool IsCopNumber(string number,
                                LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    return !CompareFirstExample(number, m_listNumber);
                case LocalListType.complete:
                    return !CompareFirstExample(number, m_completeListNumber);
                case LocalListType.annual:
                    return !CompareFirstExample(number, m_annualListNumber);
                default:
                    return true;
            }
        }

        /// <summary>
        ///   does first example already exist in the list specified by 
        ///   listType.
        /// </summary>
        /// <param name="firstExample">example to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>is cop flag</returns>
        public bool IsCopNumber(FirstExampleType firstExample,
                                LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    return !m_listNumber.Any(localExample => localExample.Compare(firstExample));
                case LocalListType.complete:
                    return !m_completeListNumber.Any(localExample => localExample.Compare(firstExample));
                case LocalListType.annual:
                    return !m_annualListNumber.Any(localExample => localExample.Compare(firstExample));
                default:
                    return false;
            }
        }

        /// <summary>
        ///   does first example already exist in the list specified by 
        ///   listType.
        /// </summary>
        /// <param name="firstExample">example to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>is cop flag</returns>
        public bool IsCopNumber(
          string number,
          IJnyId jnyId,
          LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    this.LoadAllPurposeList(jnyId.Date.Year.ToString());
                    foreach (FirstExampleType example in m_listNumber)
                    {
                        if (number == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;

                case LocalListType.complete:
                    foreach (FirstExampleType example in m_completeListNumber)
                    {
                        if (number == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;

                case LocalListType.annual:
                    foreach (FirstExampleType example in m_annualListNumber)
                    {
                        if (number == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        ///   does number already exist in the list specified by listType.
        /// </summary>
        /// <param name="location">location to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>is cop flag</returns>
        public bool IsCopLocation(
          string location,
          LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    return !m_listLocation.Any(firstExample => firstExample.Item == location);
                case LocalListType.complete:
                    return !m_completeListLocation.Any(firstExample => firstExample.Item == location);
                case LocalListType.annual:
                    return !m_annualListLocation.Any(firstExample => firstExample.Item == location);
                default:
                    return true;
            }
        }

        /// <summary>
        ///   does first example already exist in the list specified by listType.
        /// </summary>
        /// <param name="firstExample">first example to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>is cop flag</returns>
        public bool IsCopLocation(
          FirstExampleType firstExample,
          LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    return !m_listLocation.Any(localExample => localExample.Compare(firstExample));
                case LocalListType.complete:
                    return !m_completeListLocation.Any(localExample => localExample.Compare(firstExample));
                case LocalListType.annual:
                    return !m_annualListLocation.Any(localExample => localExample.Compare(firstExample));
                default:
                    return false;
            }
        }

        /// <summary>
        ///   does first example already exist in the list specified by listType.
        /// </summary>
        /// <param name="firstExample">first example to compare</param>
        /// <param name="listType">type of list</param>
        /// <returns>is cop flag</returns>
        public bool IsCopLocation(
          string location,
          IJnyId jnyId,
          LocalListType listType)
        {
            switch (listType)
            {
                case LocalListType.allPurpose:
                    this.LoadAllPurposeList(jnyId.Date.Year.ToString());
                    foreach (FirstExampleType example in m_listLocation)
                    {
                        if (location == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;
                case LocalListType.complete:
                    foreach (FirstExampleType example in m_completeListLocation)
                    {
                        if (location == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;
                case LocalListType.annual:
                    foreach (FirstExampleType example in m_annualListLocation)
                    {
                        if (location == example.Item &&
                          jnyId.Date == example.Date &&
                          jnyId.JnyNumber == example.Index)
                        {
                            return true;
                        }
                    }

                    return false;
                default:
                    return false;
            }
        }

        // ---------- ---------- ---------- ---------- ---------- ----------
        // The following methods are used to search through the raw data
        //   and compile the lists of first examples. They will be used
        //   by the configuration form.
        // ---------- ---------- ---------- ---------- ---------- ----------

        /// <summary>
        /// Search though everything to determine a complete list of first
        ///   examples
        /// </summary>
        public void RunSearchAll()
        {
            string[] dirNamesArray = System.IO.Directory.GetDirectories(BasePathReader.GetBasePath() + StaticResources.baPath);
            string dirName = string.Empty;
            int currentYear = 0;

            m_listNumber = new List<FirstExampleType>();
            m_listLocation = new List<FirstExampleType>();

            // looping through years
            for (int index = 0; index < dirNamesArray.Count(); ++index)
            {
                dirName = dirNamesArray[index].Substring(dirNamesArray[index].LastIndexOf('\\') + 1);
                if (int.TryParse(dirName, out currentYear))
                {
                    string[] fileNamesArray = System.IO.Directory.GetFiles(dirNamesArray[index]);
                    SearchThroughSingleYear(currentYear, fileNamesArray);
                }
            }

            FirstExampleIOController firstExampleController = FirstExampleIOController.GetInstance();
            firstExampleController.WriteFileNumber(m_listNumber);
            firstExampleController.WriteFileLocation(m_listLocation);
        }

        /// <summary>
        /// Search though everything to determine an annual list of first
        ///   examples as determined by the year argument.
        /// </summary>
        /// <param name="year">year to search</param>
        public void RunSearchYear(string year)
        {
            int currentYear = 0;

            m_listNumber = new List<FirstExampleType>();
            m_listLocation = new List<FirstExampleType>();

            if (int.TryParse(year, out currentYear))
            {
                string[] fileNamesArray = System.IO.Directory.GetFiles(BasePathReader.GetBasePath() +
                                                                       StaticResources.baPath +
                                                                       year);
                SearchThroughSingleYear(currentYear, fileNamesArray);
            }

            FirstExampleIOController firstExampleController = FirstExampleIOController.GetInstance();
            firstExampleController.WriteFileNumber(m_listNumber, year);
            firstExampleController.WriteFileLocation(m_listLocation, year);
        }

        /// <summary>
        /// Searches for the numbers in journey and see if they are already 
        ///   in the list. If they aren't then it adds them.
        /// </summary>
        /// <param name="journey">journey details</param>
        public void SearchNumbersForMatch(IJourneyDetailsType journey)
        {
            foreach (string unit in journey.Units)
            {
                bool cop =
                  this.IsCopNumber(
                    unit,
                    LocalListType.allPurpose);

                if (cop)
                {
                    FirstExampleType firstExamples = new FirstExampleType();
                    firstExamples.Item = unit;
                    firstExamples.Date = journey.Date;
                    firstExamples.Index = journey.JnyId.JnyNumber;

                    m_listNumber.Add(firstExamples);
                    firstExamples = null;
                }
            }

            //if (IsCopNumber(journey.GetJourneyFirstNumber(), LocalListType.allPurpose))
            //{
            //  FirstExampleType firstExamples = new FirstExampleType();
            //  firstExamples.Item  = journey.GetJourneyFirstNumber();
            //  firstExamples.Date  = journey.GetJourneyDate();
            //  firstExamples.Index = journey.GetJourneyIndex();

            //  m_listNumber.Add(firstExamples);
            //  firstExamples = null;
            //}

            //if (journey.GetJourneyNumberOfVehicles() >= 2)
            //{
            //  if (IsCopNumber(journey.GetJourneySecondNumber(), LocalListType.allPurpose))
            //  {
            //    FirstExampleType firstExamples = new FirstExampleType();
            //    firstExamples.Item  = journey.GetJourneySecondNumber();
            //    firstExamples.Date  = journey.GetJourneyDate();
            //    firstExamples.Index = journey.GetJourneyIndex();

            //    m_listNumber.Add(firstExamples);
            //    firstExamples = null;
            //  }
            //}

            //if (journey.GetJourneyNumberOfVehicles() >= 3)
            //{
            //  if (IsCopNumber(journey.GetJourneyThirdNumber(), LocalListType.allPurpose))
            //  {
            //    FirstExampleType firstExamples = new FirstExampleType();
            //    firstExamples.Item  = journey.GetJourneyThirdNumber();
            //    firstExamples.Date  = journey.GetJourneyDate();
            //    firstExamples.Index = journey.GetJourneyIndex();

            //    m_listNumber.Add(firstExamples);
            //    firstExamples = null;
            //  }
            //}

            //if (journey.GetJourneyNumberOfVehicles() >= 4)
            //{
            //  if (IsCopNumber(journey.GetJourneyFourthNumber(), LocalListType.allPurpose))
            //  {
            //    FirstExampleType firstExamples = new FirstExampleType();
            //    firstExamples.Item  = journey.GetJourneyFourthNumber();
            //    firstExamples.Date  = journey.GetJourneyDate();
            //    firstExamples.Index = journey.GetJourneyIndex();

            //    m_listNumber.Add(firstExamples);
            //    firstExamples = null;
            //  }
            //}
        }

        /// <summary>
        /// Searches for the numbers in journey and see if they are already 
        ///   in the list. If they aren't then it adds them.
        /// </summary>
        /// <param name="journey">journey details</param>
        public void SearchLocationsForMatch(IJourneyDetailsType journey)
        {
            if (IsCopLocation(journey.From, LocalListType.allPurpose))
            {
                FirstExampleType firstExamples = new FirstExampleType();
                firstExamples.Item = journey.From;
                firstExamples.Date = journey.Date;
                firstExamples.Index = journey.JnyId.JnyNumber;

                m_listLocation.Add(firstExamples);
                firstExamples = null;
            }

            if (IsCopLocation(journey.To, LocalListType.allPurpose))
            {
                FirstExampleType firstExamples = new FirstExampleType();
                firstExamples.Item = journey.To;
                firstExamples.Date = journey.Date;
                firstExamples.Index = journey.JnyId.JnyNumber;

                m_listLocation.Add(firstExamples);
                firstExamples = null;
            }
        }

        // ---------- ---------- ---------- ---------- ---------- ----------
        // The following methods are used to append the argument fet to
        //   the list indicated by localList.
        // ---------- ---------- ---------- ---------- ---------- ----------

        /// <summary>
        /// Appends first example to the relevant list and file. localList 
        /// determines which list/file. If looking at a specific year, then
        /// this is defined by the year argument.
        /// </summary>
        /// <param name="firstExamples">first example to append</param>
        /// <param name="localList">type of list</param>
        /// <param name="year">current year</param>
        public void AppendLocation(FirstExampleType firstExamples,
                                   LocalListType localList,
                                   string year = "1970")
        {
            FirstExampleIOController firstExampleController;

            switch (localList)
            {
                case LocalListType.complete:
                    m_completeListLocation.Add(firstExamples);
                    firstExampleController = FirstExampleIOController.GetInstance();
                    firstExampleController.AppendFileLocation(firstExamples);
                    break;

                case LocalListType.annual:
                    m_annualListLocation.Add(firstExamples);
                    firstExampleController = FirstExampleIOController.GetInstance();
                    firstExampleController.AppendFileLocation(firstExamples, year);
                    break;
            }
        }

        /// <summary>
        /// Appends first example to the relevant list and file. localList
        /// determines which list/file. If looking at a specific year, then
        /// this is defined by the year argument.
        /// </summary>
        /// <param name="firstExamples">first example to append</param>
        /// <param name="localList">type of list</param>
        /// <param name="year">current year</param>
        public void AppendNumber(FirstExampleType firstExamples,
                                 LocalListType localList,
                                 string year = "1970")
        {
            FirstExampleIOController firstExampleController;

            switch (localList)
            {
                case LocalListType.complete:
                    m_completeListNumber.Add(firstExamples);
                    firstExampleController = FirstExampleIOController.GetInstance();
                    firstExampleController.AppendFileNumber(firstExamples);
                    break;

                case LocalListType.annual:
                    m_annualListNumber.Add(firstExamples);
                    firstExampleController = FirstExampleIOController.GetInstance();
                    firstExampleController.AppendFileNumber(firstExamples, year);
                    break;
            }
        }

        ///// <summary>
        ///// Set the day factory field.
        ///// </summary>
        ///// <remarks>
        ///// This is a crap solution, this really shouldn't be a singleton, but am working on something else at the moment.
        ///// </remarks>
        ///// <param name="dayFactory"></param>
        //public void SetDailyInputFactory(
        //  IDailyInputFactory dayFactory)
        //{
        //  this.dayFactory = dayFactory;
        //}

        /// <summary>
        /// fileNamesArray contains a list of all the filenames (and paths)
        ///   for the raw data of a single year, currentYear). This searches
        ///   through each one looking for numbers and locations which have 
        ///   previously not been used. 
        /// </summary>
        /// <param name="currentYear">current year</param>
        /// <param name="fileNamesArray">array of file names</param>
        private void SearchThroughSingleYear(
          int currentYear,
          string[] fileNamesArray)
        {
            //string   fileName        = string.Empty;
            //int      currentMonth    = 0;

            //DailyIOController dailyController         = DailyIOController.GetInstance();

            // loop through months
            //foreach (string currentFilename in fileNamesArray) 
            //{
            for (int month = 1; month <= 12; ++month)
            {
                // Get file name from the path and convert it into it's integer value.
                //fileName      = currentFilename.Substring(currentFilename.LastIndexOf('\\') + 1);
                //fileName      = fileName.Substring(0, fileName.LastIndexOf('.'));
                //if (fileName.Length == 4)
                //{
                //fileName      = fileName.Substring(2);
                //if (int.TryParse(fileName, out currentMonth))
                //{
                // Searching all lines from this point forward.
                //List<string> journeysList =
                List<IJourneyDetailsType> journeysList =
              DailyInputFactory.LoadMonth(
                currentYear,
                month);

                // loop through everything in the month
                //foreach (string currentJourney in journeysList)
                //{
                foreach (IJourneyDetailsType currentJourneyDetails in journeysList)
                {
                    //JourneyDetailsType currentJourneyDetails = new JourneyDetailsType(currentJourney, currentYear, currentMonth);
                    SearchNumbersForMatch(currentJourneyDetails);
                    SearchLocationsForMatch(currentJourneyDetails);
                }
                //}
                //}
            }
        }

        /// <summary>
        /// Compare to see if first example is in the list.
        /// </summary>
        /// <param name="firstExample">value to test</param>
        /// <param name="firstExampleList">list of examples</param>
        /// <returns>match flag</returns>
        private bool CompareFirstExample(
          FirstExampleType firstExample,
          List<FirstExampleType> firstExampleList)
        {
            return firstExampleList.Any(localExample => localExample.Compare(firstExample));
        }

        /// <summary>
        /// Compare to see if first example is in the list (item).
        /// </summary>
        /// <param name="firstExample">value to test</param>
        /// <param name="firstExampleList">list of examples</param>
        /// <returns>match flag</returns>
        private bool CompareFirstExample(
          string firstExample,
          List<FirstExampleType> firstExampleList)
        {
            return firstExampleList.Any(localExample => localExample.Item == firstExample);
        }
    }
}