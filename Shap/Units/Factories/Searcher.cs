namespace Shap.Units.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Shap.Common;
    using Shap.Input.Factories;
    using Shap.Interfaces.Types;
    using Shap.Types;

    /// <summary>
    /// Search through all journeys to find all those associated with a specific machine.
    /// </summary>
    public static class Searcher 
    {
        /// <summary>
        /// Length of the file name for each of the month data classes.
        /// </summary>
        private const int monthFileNameLength = 4;

        /// <summary>
        /// Run search over everything
        /// </summary>
        /// <param name="currentNumber">current number</param>
        /// <param name="previousNumbersList">collection of previous numbers</param>
        public static SearcherResults RunCompleteSearch(
          string currentNumber,
          List<int> previousNumbersList)
        {
            List<IJourneyDetailsType> foundJourneys = new List<IJourneyDetailsType>();
            DateTime lastCheckedDate = new DateTime();

            string[] yearDirsArray =
              System.IO.Directory.GetDirectories(
                BasePathReader.GetBasePath() + StaticResources.baPath);

            for (int yearIndex = 0; yearIndex < yearDirsArray.Count(); ++yearIndex)
            {
                int? currentYear = Searcher.ConvertYearDirectory(yearDirsArray[yearIndex]);

                if (currentYear == null)
                {
                    continue;
                }

                string[] fileNamesArray =
                  System.IO.Directory.GetFiles(
                    yearDirsArray[yearIndex]);

                for (int monthIndex = 0; monthIndex < fileNamesArray.Count(); ++monthIndex)
                {
                    int? currentMonth = Searcher.ConvertMonthFilename(fileNamesArray[monthIndex]);

                    if (currentMonth == null)
                    {
                        continue;
                    }

                    List<IJourneyDetailsType> found =
                      Searcher.AnalyseMonth(
                        (int)currentYear,
                        (int)currentMonth,
                        currentNumber,
                        previousNumbersList,
                        ref lastCheckedDate);

                    foundJourneys.AddRange(found);
                }
            }

            SearcherResults results =
                new SearcherResults(
                    foundJourneys,
                    lastCheckedDate);

            return results;
        }

        /// <summary>
        /// TODO Update to be implemented correctly
        /// </summary>
        /// <param name="currentNumber">number to check for</param>
        /// <param name="previousNumbersList">alternative list of numbers</param>
        /// <param name="year">search start year</param>
        /// <param name="month">search start month</param>
        /// <param name="day">search start day</param>
        public static void RunUpdateSearch(
          string currentNumber,
          List<int> previousNumbersList,
          int year,
          int month,
          int day)
        {
            // null;
        }

        /// <summary>
        /// Convert a directory name into a year integer
        /// </summary>
        /// <param name="yearDirectory">year directory</param>
        /// <returns>year as integer, null if invalid</returns>
        private static int? ConvertYearDirectory(
          string yearDirectory)
        {
            int year;

            string calculatedYear = yearDirectory.Substring(yearDirectory.LastIndexOf('\\') + 1);

            if (!int.TryParse(calculatedYear, out year))
            {
                return null;
            }

            return year;
        }

        /// <summary>
        /// Convert a filename into a month integer
        /// </summary>
        /// <param name="monthFilename">month filename</param>
        /// <returns>month as integer, null if invalid</returns>
        private static int? ConvertMonthFilename(
          string monthFilename)
        {
            int month;

            string calculatedMonth = monthFilename.Substring(monthFilename.LastIndexOf('\\') + 1);
            calculatedMonth = calculatedMonth.Substring(0, calculatedMonth.LastIndexOf('.'));
            calculatedMonth = calculatedMonth.Substring(2);

            if (!int.TryParse(calculatedMonth, out month))
            {
                return null;
            }

            return month;
        }

        /// <summary>
        /// Analyse the requested month
        /// </summary>
        /// <param name="year">year to check</param>
        /// <param name="month">month to check</param>
        /// <param name="currentNumber">current number</param>
        /// <param name="previousNumbersList">colelction of previous numbers</param>
        /// <returns>The last jny checked</returns>
        private static List<IJourneyDetailsType> AnalyseMonth(
          int year,
          int month,
          string currentNumber,
          List<int> previousNumbersList,
          ref DateTime lastChecked)
        {
            List<IJourneyDetailsType> foundJourneys = new List<IJourneyDetailsType>();

            // Searching all lines from this point forward.
            List<IJourneyDetailsType> jnysList =
              DailyInputFactory.LoadMonth(
                year,
                month);

            if (jnysList.Count == 0)
            {
                return foundJourneys;
            }

            // loop through everything in the month
            foreach (JourneyDetailsType currentJourneyDetails in jnysList)
            {
                currentJourneyDetails.ParentUnitNumber = currentNumber;

                if (
                    Searcher.NumbersMatch(
                  currentNumber,
                  previousNumbersList,
                  currentJourneyDetails.Units))
                {
                    foundJourneys.Add(currentJourneyDetails);
                }
            }

            lastChecked = jnysList[jnysList.Count - 1].Date;

            return foundJourneys;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>searchForMatch</name>
        /// <date>16/11/12</date>
        /// <summary>
        ///   Searches the two arguments to see if any two strings match.
        /// </summary>
        /// <param name="unitNumbersList">input comparison list</param>
        /// <param name="jnyNumberList">read number list</param>
        /// <returns>numbers match flag</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        private static bool NumbersMatch(
          string currentNumber,
          List<int> formerUnitNumbersList,
          List<string> jnyNumberList)
        {
            foreach (string number in jnyNumberList)
            {
                if (number == currentNumber)
                {
                    return true;
                }

                foreach (int input in formerUnitNumbersList)
                {
                    if (input.ToString() == number)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}