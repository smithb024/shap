namespace Shap.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Common;
    using Input.Factories;

    using NynaeveLib.Logger;

    using Shap.Interfaces.Io;
    using Shap.Interfaces.Types;

    using Types;

    /// <summary>
    /// Factory class used to create class reports.
    /// </summary>
    public static class ClassReportFactory
    {
        /// <summary>
        /// Run a general class report.
        /// This provides the count for each class across all records.
        /// </summary>
        /// <param name="controllers">IO Controllers</param>
        /// <param name="fullList">
        /// return a full list of locations or just none zero ones.
        /// </param>
        /// <returns>success flag</returns>
        public static ReportCounterManager<ClassCounter> RunGeneralReportForAllCls(
          IIoControllers controllers,
          bool fullList)
        {
            // Set up paths.
            string basePath =
              BasePathReader.GetBasePath();

            string[] dirNamesArray =
              System.IO.Directory.GetDirectories(
                $"{basePath}{StaticResources.baPath}");

            // Load the groups and set up the report class with an entry for each group.
            List<GroupsType> groupsList =
              controllers.Gac.LoadFile();

            ReportCounterManager<ClassCounter> classTotals =
              new ReportCounterManager<ClassCounter>();

            foreach (GroupsType group in groupsList)
            {
                classTotals.AddNewCounter(
                  new ClassCounter(
                    group.Name));
            }

            // Loop through all paths.
            for (int directoryIndex = 0; directoryIndex < dirNamesArray.Count(); ++directoryIndex)
            {
                // get directory name from the path and convert it into it's integer value.
                string dirName =
                  dirNamesArray[directoryIndex].Substring(
                    dirNamesArray[directoryIndex].LastIndexOf('\\') + 1);
                ClassReportFactory.UpdateClassesForYear(
                  classTotals,
                  groupsList,
                  dirName);
            }

            if (!fullList)
            {
                classTotals.RemoveEmptyClasses();
            }

            return classTotals;

            //classTotals.WriteCSVFile(
            //  $"ClsReport_Gen_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv",
            //  "ReportBuilder: Failed to write General Cls Report.");
        }

        /// <summary>
        /// Run a location report based on a set of classes.
        /// </summary>
        /// <param name="controllers">IO Controllers</param>
        /// <param name="classes">collection of class names to run the report on</param>
        /// <param name="fullList">full list of locations</param>
        /// <param name="year">
        /// Optional parameter. If set, the search is restricted to the specified year.
        /// </param>
        /// <returns>
        /// The results of the report. This is a list of location with a count attached to 
        /// each one.
        /// </returns>
        public static ReportCounterManager<LocationCounter> RunReportForClasses(
          IIoControllers controllers,
          List<string> classes,
          bool fullList,
          string year = "")
        {
            ReportCounterManager<LocationCounter> locationTotals =
              new ReportCounterManager<LocationCounter>();

            // Only continue if there are classes to report on.
            if (classes.Count == 0)
            {
                return locationTotals;
            }

            // Set up paths.
            string basePath = BasePathReader.GetBasePath();
            string[] dirNamesArray =
                  System.IO.Directory.GetDirectories(
                    $"{basePath}{StaticResources.baPath}");

            List<FirstExampleType> firstExampleList =
              Stats.FirstExampleIOController.GetInstance().GetFirstExampleListLocation();
            firstExampleList = firstExampleList.OrderBy(loc => loc.Item).ToList();

            foreach (FirstExampleType location in firstExampleList)
            {
                locationTotals.AddNewCounter(
                  new LocationCounter(
                    location.Item));
            }

            // Run a report on each class one at a time.
            foreach (string cls in classes)
            {
                if (string.IsNullOrEmpty(year))
                {
                    // Handle a specific year.
                    // Loop through all paths.
                    for (int directoryIndex = 0; directoryIndex < dirNamesArray.Count(); ++directoryIndex)
                    {
                        // get directory name from the path and convert it into it's integer value.
                        string dirName =
                          dirNamesArray[directoryIndex].Substring(
                            dirNamesArray[directoryIndex].LastIndexOf('\\') + 1);
                        ClassReportFactory.UpdateLocationsForYear(
                          controllers,
                          dirName,
                          locationTotals,
                          cls);
                    }
                }
                else
                {
                    // Handle all years.
                    ClassReportFactory.UpdateLocationsForYear(
                        controllers,
                        year,
                        locationTotals,
                        cls);
                }
            }

            if (!fullList)
            {
                locationTotals.RemoveEmptyClasses();
            }

            return locationTotals;
        }

        /// <summary>
        ///   Run a cls report based on the year. It counts the number of 
        ///     each cls for each month in the given year.
        /// </summary>
        /// <param name="controllers">IO Controllers</param>
        /// <param name="year">current year</param>
        /// <param name="fullList">fullList</param>
        /// <returns>is successful</returns>
        public static ReportCounterManager<YearCounter> RunYearReportForAllCls(
          IIoControllers controllers,
          string year,
          bool fullList)
        {
            ReportCounterManager<YearCounter> classTotals =
              new ReportCounterManager<YearCounter>();

            List<GroupsType> groupsList =
              controllers.Gac.LoadFile();

            foreach (GroupsType group in groupsList)
            {
                classTotals.AddNewCounter(new YearCounter(group.Name));
            }

            //string writeName = $"ClsReport_{year}_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv";
            //string faultMessage = $"ReportBuilder: Failed to write Annual Cls Report for {year}.";

            ClassReportFactory.UpdateClassesForYear(
                classTotals,
                groupsList,
                year);

            if (!fullList)
            {
                classTotals.RemoveEmptyClasses();
            }

            return classTotals;

            //classTotals.WriteCSVFile(
            //  writeName,
            //  faultMessage);
        }

        /// <summary>
        ///   Loops through all month files in a year directory and analyses
        ///     each journey. It works out the class of each number in the 
        ///     journey and updates the relevant arrays.
        ///   If singleCls is set then it is measuring all journeys for the
        ///     specified cls argument. If not set then it is measuring each
        ///     cls and counting examples per month.
        /// </summary>
        /// <param name="classTotals">class counter for the current search</param>
        /// <param name="year">year to update</param>
        /// <param name="cls">cls name</param>
        /// <returns name="success">is successful</returns>
        private static void UpdateClassesForYear(
          ReportCounterManager<ClassCounter> classTotals,
          List<GroupsType> groups,
          string year)
        {
            int yearInteger = 0;

            // Convert year string to an integer
            if (!int.TryParse(year, out yearInteger))
            {
                Logger.Instance.WriteLog("ReportBuilder: Can't convert year " + year);
                return;
            }

            for (int month = 1; month <= 12; ++month)
            {
                List<IJourneyDetailsType> journeysList =
                  DailyInputFactory.LoadMonth(
                    yearInteger,
                    month);

                foreach (IJourneyDetailsType currentJourneyDetails in journeysList)
                {
                    // Update the class totals for each unit in the journey.
                    foreach (string unit in currentJourneyDetails.Units)
                    {
                        ClassReportFactory.UpdateClassTotals(
                          unit,
                          groups,
                          classTotals);
                    }
                }
            }
        }

        /// <summary>
        ///   Loops through all month files in a year directory and analyses
        ///     each journey. It works out the class of each number in the 
        ///     journey and updates the relevant arrays.
        ///   If singleCls is set then it is measuring all journeys for the
        ///     specified cls argument. If not set then it is measuring each
        ///     cls and counting examples per month.
        /// </summary>
        /// <param name="classTotals">class counter for the current search</param>
        /// <param name="year">year to update</param>
        /// <param name="cls">cls name</param>
        /// <returns name="success">is successful</returns>
        public static void UpdateClassesForYear(
          ReportCounterManager<YearCounter> classTotals,
          List<GroupsType> groups,
          string year)
        {
            int yearInteger = 0;

            // Convert year string to an integer
            if (!int.TryParse(year, out yearInteger))
            {
                Logger.Instance.WriteLog("ReportBuilder: Can't convert year " + year);
                return;
            }

            for (int month = 1; month <= 12; ++month)
            {
                List<IJourneyDetailsType> journeysList =
                  DailyInputFactory.LoadMonth(
                    yearInteger,
                    month);

                foreach (IJourneyDetailsType currentJourneyDetails in journeysList)
                {
                    foreach (string unit in currentJourneyDetails.Units)
                    {
                        ClassReportFactory.UpdateClassTotals(
                          unit,
                          groups,
                          classTotals,
                          month);
                    }
                }
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>updateClassesForYear</name>
        /// <date>15/09/13</date>
        /// <summary>
        ///   Loops through all month files in a year directory and analyses
        ///     each journey. It works out the class of each number in the 
        ///     journey and updates the relevant arrays.
        ///   If singleCls is set then it is measuring all journeys for the
        ///     specified cls argument. If not set then it is measuring each
        ///     cls and counting examples per month.
        /// </summary>
        /// <param name="controllers">IO Controllers</param>
        /// <param name="year">year to update</param>
        /// <param name="locations">collection of locations</param>
        /// <param name="cls">cls name</param>
        /// <returns name="success">is successful</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public static void UpdateLocationsForYear(
          IIoControllers controllers,
          string year,
          ReportCounterManager<LocationCounter> locations,
          string cls)
        {
            // Load the groups and set up the report class with an entry for each group.
            List<GroupsType> groups =
              controllers.Gac.LoadFile();

            int yearInteger = 0;

            // Convert year string to an integer
            if (!int.TryParse(year, out yearInteger))
            {
                Logger.Instance.WriteLog("ReportBuilder: Can't convert year " + year);
                return;
            }

            // loop through months
            for (int month = 1; month <= 12; ++month)
            {
                List<IJourneyDetailsType> journeysList =
                  DailyInputFactory.LoadMonth(
                    yearInteger,
                    month);

                // loop through everything in the month
                //foreach (string currentJourney in journeysList) 
                foreach (IJourneyDetailsType currentJourneyDetails in journeysList)
                {
                    foreach (string unit in currentJourneyDetails.Units)
                    {
                        UpdateArraysBasedOnCls(
                          unit,
                          groups,
                          locations,
                          currentJourneyDetails,
                          cls);
                    }
                }
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>updateArraysBasedOnCls</name>
        /// <date>15/09/13</date>
        /// <summary>
        ///   If singleCls is set then use the currentJourney to update all
        ///    location arrays. If not set then update the cls array for the
        ///    current month.
        /// </summary>
        /// <param name="classId">class id</param>
        /// <param name="cls">cls name</param>
        /// <param name="currentJourneyDetails">current journey details</param>
        /// <param name="month">month to update</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private static void UpdateArraysBasedOnCls(
          string vehicleNumber,
          List<GroupsType> groups,
          ReportCounterManager<LocationCounter> locations,
          IJourneyDetailsType currentJourneyDetails,
          string cls)
        {
            if (vehicleNumber == string.Empty)
            {
                return;
            }

            List<string> classIds =
              ClassReportFactory.GetClassAndFamilies(
                groups,
                vehicleNumber);

            foreach (string classId in classIds)
            {
                if (classId == cls)
                {
                    locations.AddOne(
                      currentJourneyDetails.To,
                      currentJourneyDetails.From);
                }
            }
        }

       /// <summary>
        /// Determine the groups for the <paramref name="vehicleNumber"/> then increase
        /// the totals.
        /// </summary>
        /// <param name="vehicleNumber">current vehicle number</param>
        /// <param name="groups">group details</param>
        /// <param name="classTotals">class total array</param>
        private static void UpdateClassTotals(
          string vehicleNumber,
          List<GroupsType> groups,
          ReportCounterManager<ClassCounter> classTotals)
        {
            if (vehicleNumber == string.Empty)
            {
                return;
            }

            List<string> classIds =
              ClassReportFactory.GetClassAndFamilies(
                groups,
                vehicleNumber);

            if (classIds == null || classIds.Count == 0)
            {
                return;
            }

            foreach (string classId in classIds)
            {
                classTotals.AddOne(classId);
            }
        }

        /// <summary>
        /// Determine the groups for the <paramref name="vehicleNumber"/> then increase
        /// the totals.
        /// </summary>
        /// <param name="vehicleNumber">current vehicle number</param>
        /// <param name="groups">group details</param>
        /// <param name="classTotals">class total array</param>
        private static void UpdateClassTotals(
          string vehicleNumber,
          List<GroupsType> groups,
          ReportCounterManager<YearCounter> classTotals,
          int month)
        {
            if (vehicleNumber == string.Empty)
            {
                return;
            }

            List<string> classIds =
              ClassReportFactory.GetClassAndFamilies(
                groups,
                vehicleNumber);

            if (classIds == null || classIds.Count == 0)
            {
                return;
            }

            foreach (string classId in classIds)
            {
                classTotals.AddOne(classId, month);
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>getClass</name>
        /// <date>15/09/13</date>
        /// <summary>
        ///   Works out the cls based on the input string. If purely a 
        ///     number it tries to work out what cls the number refers to,
        ///     otherwise it works out which cls the first letter
        ///     corresponds to.
        /// </summary>
        /// <param name="unitNumber">input string</param>
        /// <returns>class name</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private static List<string> GetClassAndFamilies(
          List<GroupsType> groups,
          string unitNumber)
        {
            List<string> myclass = new List<string>();
            int number = 0;
            string alpha = string.Empty;

            if (unitNumber == string.Empty)
            {
                return myclass;
            }
            else
            {
                if (int.TryParse(unitNumber, out number))
                {
                    return ClassReportFactory.GetGetClassesWithNumber(number, groups);
                }
                else
                {
                    return ClassReportFactory.GetClassesWithAlpha(unitNumber, groups);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        private static List<string> GetGetClassesWithNumber(
          int number,
          List<GroupsType> groups)
        {
            List<string> returnClasses = new List<string>();

            // The string was a valid integer => use result here
            foreach (GroupsType group in groups)
            {
                for (int boundIndex = 0; boundIndex < group.RangeArraySize(); ++boundIndex)
                {
                    if (number >= group.GetLowerBound(boundIndex) &&
                        number <= group.GetUpperBound(boundIndex))
                    {
                        returnClasses.Add(group.Name);
                    }
                }
            }

            return returnClasses;
        }

        /// <summary>
        /// Get all classes with alpha Id
        /// </summary>
        /// <param name="unitNumber">The unit number</param>
        /// <param name="groups">The list of all groups</param>
        /// <returns>collection of classes with an alpha Id</returns>
        private static List<string> GetClassesWithAlpha(
          string unitNumber,
          List<GroupsType> groups)
        {
            List<string> returnClasses = new List<string>();

            int index = unitNumber.IndexOfAny("0123456789".ToCharArray());
            string alpha = unitNumber.Substring(0, index);

            foreach (GroupsType group in groups)
            {
                for (int alphaIndex = 0; alphaIndex < group.AlphaIDArraySize(); ++alphaIndex)
                {
                    if (alpha == group.GetAlphaID(alphaIndex))
                    {
                        returnClasses.Add(group.Name);
                    }
                }
            }

            return returnClasses;
        }
    }
}