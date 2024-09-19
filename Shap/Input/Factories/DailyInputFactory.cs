namespace Shap.Input.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    using Common;
    using Interfaces.Types;

    using NynaeveLib.Logger;
    using NynaeveLib.Types;

    using Shap.Types;

    /// <summary>
    /// Handles the daily data. It provides a way to read and write to the data files via the IO
    /// controller. When reading it returns the data in format which the calling class can 
    /// process rather than relying on it to do it itself.
    /// </summary>
    public static class DailyInputFactory
    {
        /// <summary>
        /// Format for months, ensures that they are all the same width.
        /// </summary>
        private const string monthFormat = "00";

        /// <summary>
        /// The prefix for all month files. A file with this prefix is always a raw journey file.
        /// </summary>
        private const string filePrefix = "ba";

        //DailyIOController dayController;

        ///// <summary>
        ///// Initialise a new instance of the <see cref="DailyInputFactory"/> class.
        ///// </summary>
        //public DailyInputFactory()
        //{
        //  //this.dayController = ioController;
        //}

        /// <summary>
        /// Get a list of all distances in a month
        /// </summary>
        /// <param name="year">current year</param>
        /// <param name="month">current year</param>
        /// <returns>list of all distances in a month</returns>
        public static ObservableCollection<DayViewModel> GetAllDaysInMonth(
          int year,
          int month)
        {
            List<MilesChains> daysList =
              DailyInputFactory.SetupDays(
                year,
                month);

            List<IJourneyDetailsType> jnyDetails =
              DailyInputFactory.LoadMonth(
                year,
                month);

            foreach (IJourneyDetailsType jny in jnyDetails)
            {
                daysList[jny.Date.Day - 1] += jny.Distance;
            }

            return DailyInputFactory.SetupDayViewModel(
              year,
              month,
              daysList);
        }

        /// <summary>
        /// Get all the journeys for the given <paramref name="date"/>
        /// </summary>
        /// <param name="date">day to check</param>
        /// <returns>observable collection of journeys</returns>
        public static List<IJourneyDetailsType> LoadDay(DateTime date)
        {
            string filePath =
              DailyInputFactory.BuildFilePath(
                date.Year,
                date.Month);

            if (string.IsNullOrEmpty(filePath))
            {
                Logger.Instance.WriteLog(
                  $"ERROR: DailyInputFactory: Load File: Input is invalid (Date-{date})");
                return new List<IJourneyDetailsType>();
            }

            return
              DailyInputFactory.LoadFile(
                filePath,
                date.Year,
                date.Month,
                date.Day);
        }

        /// <summary>
        /// Load an input file for a given month.
        /// </summary>
        /// <param name="year">year to load</param>
        /// <param name="month">month to load</param>
        /// <returns>file contents</returns>
        public static List<IJourneyDetailsType> LoadMonth(
          int year,
          int month)
        {
            List<IJourneyDetailsType> fileJourneys = new List<IJourneyDetailsType>();

            string filePath =
              DailyInputFactory.BuildFilePath(
                year,
                month);

            if (string.IsNullOrEmpty(filePath))
            {
                Logger.Instance.WriteLog(
                  $"ERROR: DailyInputFactory: Load File: Input is invalid (Year-{year}, Month-{month})");
                return new List<IJourneyDetailsType>();
            }

            return DailyInputFactory.LoadFile(
              filePath,
              year,
              month);
        }

        /// <summary>
        /// Load an input file for a given month.
        /// </summary>
        /// <param name="path">directory containing the month files</param>
        /// <param name="month">month to load</param>
        /// <returns>file contents</returns>
        public static List<IJourneyDetailsType> LoadMonth(
          string path,
          int month)
        {
            string filePath =
              DailyInputFactory.BuildFilePath(
                path,
                month);

            if (string.IsNullOrEmpty(filePath))
            {
                Logger.Instance.WriteLog(
                  $"ERROR: DailyInputFactory: Load Month: Input is invalid (Path-{path}, Month-{month})");
                return new List<IJourneyDetailsType>();
            }

            int year;

            try
            {
                year = DailyInputFactory.GetYear(path);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                  $"ERROR: DailyInputFactory: Load Month: Input is invalid (Path-{path}, Month-{month}): {ex}");
                return new List<IJourneyDetailsType>();
            }

            return DailyInputFactory.LoadFile(
              filePath,
              year,
              month);
        }

        /// <summary>
        /// Load an input file.
        /// </summary>
        /// <param name="filePath">name and path of file to load</param>
        /// <returns>file contents</returns>
        private static List<IJourneyDetailsType> LoadFile(
          string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<IJourneyDetailsType>();
            }

            int month;
            int year;

            try
            {
                month = DailyInputFactory.GetMonth(filePath);
                year = DailyInputFactory.GetYear(filePath);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                  $"DailyInputFactory File name is invalid {ex}");
                return new List<IJourneyDetailsType>();
            }

            List<JourneyDetailsType> fileList = new List<JourneyDetailsType>();

            return DailyInputFactory.LoadFile(
              filePath,
              year,
              month);
        }

        /// <summary>
        /// Save a day.
        /// </summary>
        /// <remarks>
        /// Need to revisit this to look at the correct division of labour.
        /// </remarks>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="dayJourneysList"></param>
        /// <returns></returns>
        public static void SaveDay(
          int year,
          int month,
          int day,
          List<IJourneyDetailsType> dayJourneysList)
        {
            StreamWriter writer;
            List<IJourneyDetailsType> newJourneys = new List<IJourneyDetailsType>();
            foreach (IJourneyDetailsType journey in dayJourneysList)
            {
                newJourneys.Add((IJourneyDetailsType)journey);
            }

            string saveFilePath =
              DailyInputFactory.BuildFilePath(
                year,
                month);

            List<string> baFileLines = new List<string>();

            DailyInputFactory.EnsureDirectoryExists(year);

            DailyInputFactory.BackupFile(
              saveFilePath,
              year,
              month);
            List<IJourneyDetailsType> existingJourneys =
              DailyInputFactory.LoadFile(saveFilePath);

            List<IJourneyDetailsType> beforeDay =
              DailyInputFactory.GetJnysBeforeDay(
                existingJourneys,
                day);
            List<IJourneyDetailsType> afterDay =
              DailyInputFactory.GetJnysAfterDay(
                existingJourneys,
                day);

            // create new file
            // output buffer line by line.
            // if its a earlier day then output the line straight back out
            // if current date output todays journeys
            // if its a later day then output the line straight back out
            using (writer = new StreamWriter(saveFilePath))
            {
                DailyInputFactory.OutputJourneys(
                  writer,
                  beforeDay);
                DailyInputFactory.OutputJourneys(
                  writer,
                  newJourneys);
                DailyInputFactory.OutputJourneys(
                  writer,
                  afterDay);
            }
        }


        /// <summary>
        /// Loop through dayJourneysList and output all lines.
        /// </summary>
        /// <param name="writer">stream writer</param>
        /// <param name="journeyDetails">day journey list</param>
        private static void OutputJourneys(
          StreamWriter writer,
          List<IJourneyDetailsType> journeyDetails)
        {
            foreach (IJourneyDetailsType journey in journeyDetails)
            {
                string newLine = journey.ToString();

                Logger.Instance.WriteLog($"TRACE: DailyIOController: OutputDayJourneyList Write: {newLine}");
                writer.WriteLine(newLine);
            }
        }

        /// <summary>
        /// Convert a collection of <see cref="MilesChains"/> to
        /// <see cref="DayViewModel"/>. Assumes <paramref name="distanceList"/> is an appropriate
        /// length.
        /// </summary>
        /// <param name="year">current year</param>
        /// <param name="month">current month</param>
        /// <param name="distanceList">collection of distances</param>
        /// <returns></returns>
        private static ObservableCollection<DayViewModel> SetupDayViewModel(
      int year,
      int month,
      List<MilesChains> distanceList)
        {
            int day = 1;
            ObservableCollection<DayViewModel> days = new ObservableCollection<DayViewModel>();

            foreach (MilesChains dayDistance in distanceList)
            {
                DateTime date =
                  new DateTime(
                    year,
                    month,
                    day);

                days.Add(
                  new DayViewModel(
                    date,
                    dayDistance.Miles.ToString()));

                ++day;
            }

            return days;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private static List<MilesChains> SetupDays(int year, int month)
        {
            List<MilesChains> days = new List<MilesChains>();

            for (int dayIndex = 1; dayIndex <= DateTime.DaysInMonth(year, month); dayIndex++)
            {
                MilesChains day =
                  new MilesChains();

                days.Add(day);
            }

            return days;
        }

        /// <summary>
        /// Build the file path. 
        /// </summary>
        /// <param name="year">file year</param>
        /// <param name="month">file month</param>
        /// <param name="check">check to see if this file exists, return empty string if it doesn't</param>
        /// <returns>path to file</returns>
        private static string BuildFilePath(
          int year,
          int month,
          bool check = true)
        {
            string directory =
              $"{BasePathReader.GetBasePath()}{StaticResources.baPath}{year}";
            string filePath =
              $"{directory}\\{filePrefix}{month.ToString(monthFormat)}.txt";

            if (check)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                }
            }

            return filePath;
        }

        /// <summary>
        /// Build the file path. 
        /// </summary>
        /// <param name="year">file year</param>
        /// <param name="month">file month</param>
        /// <param name="check">check to see if this file exists, return empty string if it doesn't</param>
        /// <returns>path to file</returns>
        private static string BuildFilePath(
          string path,
          int month,
          bool check = true)
        {
            string filePath =
              $"{path}\\{filePrefix}{month.ToString(monthFormat)}.txt";

            if (check)
            {
                if (!File.Exists(filePath))
                {
                    return string.Empty;
                }
            }

            return filePath;
        }

        /// <summary>
        /// Build the filename
        /// </summary>
        /// <param name="year">file year</param>
        /// <param name="month">file month</param>
        /// <returns>standard filename</returns>
        public static string BuildBackupFilePath(
          int year,
          int month)
        {
            return $"{BasePathReader.GetBasePath()}{StaticResources.baPath}{year}\\{filePrefix}{month.ToString(monthFormat)}bac.txt";
        }

        /// <summary>
        /// Load a specific raw journey details file. Filter to a specific day if requried.
        /// </summary>
        /// <param name="year">year to load</param>
        /// <param name="month">month to load</param>
        /// <param name="day">day to load</param>
        /// <returns>success flag</returns>
        private static List<IJourneyDetailsType> LoadFile(
          string filePath,
          int year,
          int month,
          int day = -1)
        {
            List<IJourneyDetailsType> dayContents = new List<IJourneyDetailsType>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string currentLine = string.Empty;
                    currentLine = sr.ReadLine();

                    while (currentLine != null)
                    {
                        IJourneyDetailsType currentDay =
                          new JourneyDetailsType(
                            currentLine,
                            year,
                            month);

                        if (
                          day == -1 ||
                          currentDay.Date.Day == day)
                        {
                            dayContents.Add(currentDay);
                        }

                        currentLine = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                  string.Format(
                  $"ERROR: DailyInputFactory: Failed to load file {filePath}. Error is {ex}"));
            }

            return dayContents;
        }

        /// <summary>
        /// Determines the year in question based on the path to the file being analysed.
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <returns>year</returns>
        /// <exception cref="Exception">the path does not contain a valid year</exception>
        private static int GetYear(string filePath)
        {
            int year = 0;
            string lastFolderName =
              Path.GetFileName(
                Path.GetDirectoryName(
                  filePath));

            if (int.TryParse(lastFolderName, out year))
            {
                return year;
            }

            throw new Exception("Year not valid");
        }

        /// <summary>
        /// Get the month from the file name string.
        /// </summary>
        /// <param name="filePath">path to the file</param>
        /// <exception cref="Exception">the file name can't be turned into an integer</exception>
        /// <returns>month as an integer</returns>
        private static int GetMonth(string filePath)
        {
            int month = 0;
            string absoluteFileName =
              Path.GetFileNameWithoutExtension(
                filePath);

            string monthString =
              absoluteFileName.Substring(
                filePrefix.Length);

            if (int.TryParse(monthString, out month))
            {
                return month;
            }

            throw new Exception("Month not valid");
        }

        /// <summary>
        /// Backup the existing file.
        /// </summary>
        /// <param name="fileName">name of the file to backup</param>
        /// <param name="year">year of the file</param>
        /// <param name="month">month of the file</param>
        private static void BackupFile(
          string fileName,
          int year,
          int month)
        {
            string backupFile =
              DailyInputFactory.BuildBackupFilePath(
                year,
                month);
            Logger.Instance.WriteLog($"TRACE: DailyIOController - backupfile is: {backupFile}");

            // does the directory exist. Create if needed.
            // if it currently exists, rename existing file to make a copy - overwrite if needed.
            // read copy into buffer. If available
            try
            {
                if (File.Exists(backupFile) &&
                  File.Exists(fileName))
                {
                    File.Delete(backupFile);
                    File.Copy(fileName, backupFile);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                  $"ERROR: DailyIOController: Days  - Failed to rearrange files. Error is {ex}");
            }
        }

        /// <summary>
        /// Create the directory for the given year, if it doesn't exist.
        /// </summary>
        /// <param name="year">directory year</param>
        private static void EnsureDirectoryExists(
          int year)
        {
            string path =
              $"{BasePathReader.GetBasePath()}{StaticResources.baPath}{year}";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Return a list of all journeys from before the <paramref name="day"/>.
        /// </summary>
        /// <param name="jnys">original list</param>
        /// <param name="day">day to search for</param>
        /// <returns>journeys collection from before day</returns>
        private static List<IJourneyDetailsType> GetJnysBeforeDay(
          List<IJourneyDetailsType> jnys,
          int day)
        {
            for (int index = 0; index < jnys.Count; ++index)
            {
                if (jnys[index].Date.Day >= day)
                {
                    return jnys.GetRange(0, index);
                }
            }

            return new List<IJourneyDetailsType>(jnys);
        }

        /// <summary>
        /// Return a list of all journeys from after the <paramref name="day"/>.
        /// </summary>
        /// <param name="jnys">original list</param>
        /// <param name="day">day to search for</param>
        /// <returns>journeys collection from after day</returns>
        private static List<IJourneyDetailsType> GetJnysAfterDay(
          List<IJourneyDetailsType> jnys,
          int day)
        {
            for (int index = 0; index < jnys.Count; ++index)
            {
                if (jnys[index].Date.Day > day)
                {
                    return jnys.GetRange(index, jnys.Count - index);
                }
            }

            return new List<IJourneyDetailsType>();
        }
    }
}