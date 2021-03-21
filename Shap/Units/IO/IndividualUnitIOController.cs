namespace Shap.Units.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO; // stream reader/writer copy
    using System.Linq;

    using Common;
    using NynaeveLib.Logger;
    using NynaeveLib.Types;
    using Shap.Interfaces.Types;
    using Types;

    /// <summary>
    /// Static IO controller class, this used to read and write to the individual unit files.
    /// These files are currently stored in uts/idvl/{classId}.
    /// </summary>
    public static class IndividualUnitIOController
    {
        /// <summary>
        /// The number of components in a string for a full journey record. This is a minimum
        /// figure and includes at least one unit. In practice there may be up to 4 units.
        /// </summary>
        private const int FullJourneyStringSize = 10;

        /// <summary>
        /// String used to help identify the version of the file being read. It is used in 
        /// conjunction with <see cref="CurrentDataVersion"/> at the head of the file.
        /// </summary>
        private const string VersionString = "Ver";

        /// <summary>
        /// The version of the file which is gets created by the current build of this application.
        /// It is used in conjuction with <see cref="VersionString"/> to help identify file type
        /// when a file is being read.
        /// </summary>
        private const int CurrentDataVersion = 2;

        /// <date>10/03/19</date>
        /// <summary>
        /// Reads contents of an individual unit file and returns them to the caller.
        /// </summary>
        /// <param name="className">name of the class the unit file is in</param>
        /// <param name="fileName">name of the file/unit identifier</param>
        /// <returns>Contents of the file which has been read</returns>
        public static IndividualUnitFileContents ReadIndividualUnitFile(
          string className,
          string fileName)
        {
            string fullFilePath =
                IndividualUnitIOController.GetFilePath(
                    className,
                    fileName);

            if (!File.Exists(fullFilePath))
            {
                string loggerError =
                    $"ERROR: File {fullFilePath} does not exist";

                Logger.Instance.WriteLog(
                    loggerError);
                return null;
            }

            try
            {
                using (StreamReader sr = new StreamReader(GetFilePath(className, fileName)))
                {
                    string currentLine = string.Empty;

                    // {Number}
                    // Version 1 does not have a version number, so in version 1 this would be the unit id.
                    currentLine = sr.ReadLine();

                    if (currentLine.Substring(0, VersionString.Length) == VersionString)
                    {
                        int version = 0;
                        if (int.TryParse(currentLine.Substring(VersionString.Length), out version))
                        {
                            return ReadIndividualUnitFileLaterVersion(
                              sr,
                              version);
                        }
                        else
                        {
                            Logger.Instance.WriteLog($"ERROR: Error reading {fullFilePath}. Invalid Version: {currentLine}");
                            return null;
                        }
                    }
                    else
                    {
                        // This is a version 1 file.
                        return IndividualUnitIOController.ReadIndividualUnitFileVersion1(
                            sr,
                            currentLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog($"ERROR: Error reading {fullFilePath}. Error is {ex}");

                return null;
            }
        }

        /// <date>10/03/19</date>
        /// <summary>
        /// Saves the contents of a <see cref="IVehicleDetailsType"/> object to the current
        /// <see cref="VersionString"/> of a unit details file.
        /// </summary>
        /// <param name="unitDetails">unit details to be saved</param>
        /// <param name="className">name of the class which the unit is part of</param>
        /// <returns>save success flag</returns>
        public static bool WriteIndividualUnitFile(
          IVehicleDetailsType unitDetails,
          string className)
        {
            bool success = true;
            string directoryPath = string.Empty;

            try
            {
                directoryPath =
                    IndividualUnitIOController.GetDirectoryPath(
                        className);

                // Create the directory if it does not exist.
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(GetFilePath(className, unitDetails.UnitNumber)))
                {
                    // {Version}
                    writer.WriteLine(VersionString + CurrentDataVersion.ToString());

                    // {Number}
                    writer.WriteLine(unitDetails.UnitNumber);

                    // {mileage}
                    writer.WriteLine(unitDetails.UnitDistance.ToString());

                    // {number_entries}
                    writer.WriteLine(unitDetails.NumberOfJnys.ToString());

                    // {former_number1}^t{former_number2}^t{former_number3}
                    writer.WriteLine(unitDetails.FormerNumbers.ToString());

                    // {last_entry_day}^t{last_entry_month}^t{last_entry_year}
                    DateTime lastEntry = new DateTime(1, 1, 1);
                    writer.WriteLine(lastEntry.Day + "\t" + lastEntry.Month + "\t" + lastEntry.Year);

                    // {last_check_day}^t{last_check_month}^t{last_check_year}
                    DateTime lastCheck = unitDetails.UnitLastCheck;
                    writer.WriteLine(lastCheck.Day + "\t" + lastCheck.Month + "\t" + lastCheck.Year);

                    // {Status}
                    writer.WriteLine(unitDetails.Status);

                    // {Notes}
                    writer.WriteLine(unitDetails.Notes);

                    if (unitDetails.NumberOfJnys > 0)
                    {
                        // {day}^t{month}^t{year}^t{from}^t{to}^t{via}^t{miles}^t{chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
                        for (int index = unitDetails.NumberOfJnys - 1; index >= 0; --index)
                        {
                            // Bit of a mess really. There is a model class which knows how to write to files and a view model
                            // class which know how to display to a view. This converts from view model to model. May need to be
                            // reworked. (There are other places as well).
                            List<string> units = new List<string>();
                            IndividualUnitIOController.Add(units, unitDetails.JourneysList[index].UnitOne);
                            IndividualUnitIOController.Add(units, unitDetails.JourneysList[index].UnitTwo);
                            IndividualUnitIOController.Add(units, unitDetails.JourneysList[index].UnitThree);
                            IndividualUnitIOController.Add(units, unitDetails.JourneysList[index].UnitFour);

                            IJourneyDetailsType journey =
                              new JourneyDetailsType(
                                unitDetails.JourneysList[index].JnyId.Date,
                                unitDetails.JourneysList[index].JnyId.JnyNumber,
                                unitDetails.JourneysList[index].From,
                                unitDetails.JourneysList[index].To,
                                unitDetails.JourneysList[index].Route,
                                unitDetails.JourneysList[index].Distance,
                                units);
                            writer.WriteLine(journey.ToFullString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                    $"ERROR: Error writing {directoryPath}, {unitDetails.UnitNumber}. Error is {ex}");

                success = false;
            }

            return success;
        }

        /// <name>writeIndividualUnitFile</name>
        /// <date>10/03/19</date>
        /// <summary>
        /// Saves the contents of a <see cref="IndividualUnitFileContents"/> object to the current
        /// <see cref="VersionString"/> of a unit details file.
        /// </summary>
        /// <param name="unitDetails">unit details</param>
        /// <param name="className">name of the class which the unit is part of</param>
        /// <returns>success flag</returns>
        public static bool WriteIndividualUnitFile(
          IndividualUnitFileContents unitDetails,
          string className)
        {
            bool success = true;
            string directoryPath = string.Empty;

            try
            {
                directoryPath =
                    IndividualUnitIOController.GetDirectoryPath(
                        className);

                // Create the directory if it does not exist.
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(GetFilePath(className, unitDetails.UnitNumber)))
                {
                    // {Version}
                    writer.WriteLine(VersionString + CurrentDataVersion.ToString());

                    // {Number}
                    writer.WriteLine(unitDetails.UnitNumber);

                    // {mileage}
                    writer.WriteLine(unitDetails.Distance.ToString());

                    // {number_entries}
                    writer.WriteLine(unitDetails.EntriesCount.ToString());

                    // {former_number1}^t{former_number2}^t{former_number3}
                    writer.WriteLine(unitDetails.FormerNumbers.ToString());

                    // {last_entry_day}^t{last_entry_month}^t{last_entry_year}
                    DateTime lastEntry = new DateTime(1, 1, 1);
                    writer.WriteLine(lastEntry.Day + "\t" + lastEntry.Month + "\t" + lastEntry.Year);

                    // {last_check_day}^t{last_check_month}^t{last_check_year}
                    DateTime lastCheck = unitDetails.LastCheckDate;
                    writer.WriteLine(lastCheck.Day + "\t" + lastCheck.Month + "\t" + lastCheck.Year);

                    // {Status}
                    writer.WriteLine(unitDetails.InService);

                    // {Notes}
                    writer.WriteLine(unitDetails.Notes);

                    if (unitDetails.Journeys.Count > 0)
                    {
                        // {day}^t{month}^t{year}^t{from}^t{to}^t{via}^t{miles}^t{chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
                        //Not this is the other way around to the IVehicleDataType
                        for (int index = 0; index < unitDetails.Journeys.Count; ++index)
                        {
                            writer.WriteLine(unitDetails.Journeys[index].ToFullString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                    $"ERROR: Error writing {directoryPath}, {unitDetails.UnitNumber}. Error is {ex}");

                success = false;
            }

            return success;
        }

        /// <date>10/03/19</date>
        /// <summary>
        /// Rename a unit file from one number to another, update the contents to include the
        /// new id and story the old id in the former number collection.
        /// </summary>
        /// <param name="newNumber">new unit number</param>
        /// <param name="className">name of the class which the unit is part of</param>
        /// <param name="formerNumber">former unit number</param>
        /// <returns>success flag</returns>
        public static bool RenameIndividualUnitFile(
            int newNumber,
            string className,
            int formerNumber)
        {
            string originalFullPath = string.Empty;
            string newFullPath = string.Empty;

            try
            {
                originalFullPath =
                    IndividualUnitIOController.GetFilePath(
                        className,
                        newNumber.ToString());
                newFullPath =
                    IndividualUnitIOController.GetFilePath(
                        className,
                        formerNumber.ToString());

                if (!File.Exists(originalFullPath))
                {
                    Logger.Instance.WriteLog(
                        $"Rename Unit File failed, can't find the original file: {originalFullPath}");
                    return false;
                }

                // read the file
                IndividualUnitFileContents unit =
                    IndividualUnitIOController.ReadIndividualUnitFile(
                        className,
                        formerNumber.ToString());

                if (unit == null)
                {
                    Logger.Instance.WriteLog(
                        "ERROR: IndividualUnitIOController.renameIndividualUnitFile: Read File failed");
                    return false;
                }

                // change the number in the file.
                unit.UnitNumber = newNumber.ToString();

                // update the former numbers list
                unit.AddFormerNumber(formerNumber);

                // save the file.
                if (!WriteIndividualUnitFile(unit, className))
                {
                    Logger.Instance.WriteLog("ERROR: IndividualUnitIOController.renameIndividualUnitFile: Write File failed");
                    return false;
                }

                // renumber the file.
                File.Move(
                    originalFullPath,
                    newFullPath);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog($"ERROR: Error renumbering unit file. Error is {ex}");
                return false;
            }

            return true;
        }

        /// <date>10/03/19</date>
        /// <summary>
        /// Determine the full file path
        /// </summary>
        /// <param name="className">name of the class the unit is part of</param>
        /// <param name="fileName">unit file name/unit identifier</param>
        /// <returns>full file path</returns>
        private static string GetFilePath(
            string className,
            string fileName)
        {
            string basePath = BasePathReader.GetBasePath();
            return $"{basePath}{StaticResources.idvlDetailsPath}{className}{Path.DirectorySeparatorChar}{fileName}.txt";
        }

        /// <date>10/03/19</date>
        /// <summary>
        /// Determine the path to save a unit file to.
        /// </summary>
        /// <param name="className">name of the class the unit is part of</param>
        /// <returns>directory path</returns>
        private static string GetDirectoryPath(string className)
        {
            string basePath = BasePathReader.GetBasePath();
            return $"{basePath}{StaticResources.idvlDetailsPath}{className}";
        }

        /// <date>10/003/19</date>
        /// <summary>
        /// Read the contents of a version 1 file. At the point this has been called, the
        /// first line has been read and this is the unit id.
        /// </summary>
        /// <param name="reader">the open file reader</param>
        /// <param name="firstLine">contents of the first line/unit id</param>
        /// <returns>contents of the file being read.</returns>
        private static IndividualUnitFileContents ReadIndividualUnitFileVersion1(
          StreamReader reader,
          string firstLine)
        {
            string unitNumber = firstLine;
            string currentLine = string.Empty;

            // {mileage}
            currentLine = reader.ReadLine();
            MilesChains unitDistance = new MilesChains(currentLine);

            // {number_entries}
            int entriesCount =
              IndividualUnitIOController.ConvertInteger(
                reader.ReadLine());

            IVehicleNumberType formerNumbers =
              IndividualUnitIOController.GetFormerNumbers(
                reader.ReadLine());

            DateTime lastCheckDate =
              IndividualUnitIOController.ConvertDate(
                reader.ReadLine());

            List<IJourneyDetailsType> journeys = new List<IJourneyDetailsType>();
            currentLine = reader.ReadLine();
            while (currentLine != null)
            {
                IJourneyDetailsType journey = IndividualUnitIOController.ConvertJourney(currentLine);

                if (journey != null)
                {
                    journeys.Add(journey);
                }

                currentLine = reader.ReadLine();
            }

            DateTime lastJourneyDate;
            {
                if (journeys.Count > 0)
                {
                    lastJourneyDate = journeys[journeys.Count - 1].Date;
                }
                else
                {
                    lastJourneyDate = new DateTime(1970, 1, 1);
                }

                return new IndividualUnitFileContents(
                  unitNumber,
                  unitDistance,
                  entriesCount,
                  formerNumbers,
                  VehicleServiceType.InService,
                  lastJourneyDate,
                  lastCheckDate,
                  journeys,
                  string.Empty);
            }
        }

        /// <date>10/03/19</date>
        /// <summary>
        /// Reads the contents a individiual unit file for a file version higher than 1.
        /// </summary>
        /// <param name="reader">stream reader</param>
        /// <param name="version">version number</param>
        /// <returns>contents of the file being read</returns>
        private static IndividualUnitFileContents ReadIndividualUnitFileLaterVersion(
          StreamReader reader,
          int version)
        {
            if (version == 2)
            {
                return IndividualUnitIOController.ReadIndividualUnitFileVersion2(
                    reader);
            }

            return null;
        }

        /// <date>10/003/19</date>
        /// <summary>
        /// Read the contents of a version 2 file. At the point this has been called, the
        /// first line has been read and is no longer relevant to the decoding process. 
        /// The next line to be read will be the unit id.
        /// </summary>
        /// <param name="reader">the open file reader</param>
        /// <returns>contents of the file being read.</returns>
        private static IndividualUnitFileContents ReadIndividualUnitFileVersion2(
            StreamReader reader)
        {
            string currentLine = string.Empty;

            try
            {
                // {number}
                string unitNumber = reader.ReadLine();

                // {mileage}
                //currentLine = reader.ReadLine();
                MilesChains unitDistance = new MilesChains(reader.ReadLine());

                // {number_entries}
                int entriesCount =
                  IndividualUnitIOController.ConvertInteger(
                    reader.ReadLine());

                IVehicleNumberType formerNumbers =
                  IndividualUnitIOController.GetFormerNumbers(
                    reader.ReadLine());

                DateTime lastEntryDate =
                  IndividualUnitIOController.ConvertDate(
                    reader.ReadLine());
                DateTime lastCheckDate =
                  IndividualUnitIOController.ConvertDate(
                    reader.ReadLine());

                // {Status}
                VehicleServiceType inService = VehicleServiceType.InService;
                inService = IndividualUnitIOController.GetServiceStatus(reader.ReadLine());

                // {Note}
                string note = string.Empty;
                note = reader.ReadLine();

                List<IJourneyDetailsType> journeys = new List<IJourneyDetailsType>();
                currentLine = reader.ReadLine();
                while (currentLine != null)
                {
                    IJourneyDetailsType journey =
                        IndividualUnitIOController.ConvertJourney(
                            currentLine);

                    if (journey != null)
                    {
                        journeys.Add(journey);
                    }

                    currentLine = reader.ReadLine();
                }

                return new IndividualUnitFileContents(
                  unitNumber,
                  unitDistance,
                  entriesCount,
                  formerNumbers,
                  inService,
                  lastEntryDate,
                  lastCheckDate,
                  journeys,
                  note);
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteLog(
                    $"ERROR: Error reading individual unit file (ver2). Error is {ex}");

                return null;
            }
        }

        /// <summary>
        /// Converts string contains a list of former numbers and adds them
        /// via the action.
        /// </summary>
        /// <param name="numberList">list of former numbers as a string</param>
        /// <returns>
        /// All former numbers
        /// </returns>
        private static IVehicleNumberType GetFormerNumbers(
          string numberList)
        {
            IVehicleNumberType formerNumbers = new VehicleNumberTypeViewModel();

            string[] formerNumbersArray = numberList.Split('\t');
            foreach (string formerNumber in formerNumbersArray)
            {
                int number = IndividualUnitIOController.ConvertInteger(formerNumber);

                if (number != 0)
                {
                    formerNumbers.AddFormerNumber(number);
                }
            }

            return formerNumbers;
        }

        /// <summary>
        /// Decode and set the date using the action.
        /// </summary>
        /// <param name="date">date raw data.</param>
        /// <returns>
        /// Converted date.
        /// </returns>
        private static DateTime ConvertDate(
          string date)
        {
            const int NumberOfSectionsInDate = 3;

            int entryDay;
            int entryMonth;
            int entryYear;
            string[] entryDateArray = date.Split('\t');

            if (entryDateArray.Length != NumberOfSectionsInDate)
            {
                Logger.Instance.WriteLog($"ERROR: IndividualUnitIOController.readIndividualUnitFile: Invalid Entry Date in Unit file - {date}");
                return new DateTime(1970, 1, 1);
            }

            if (int.TryParse(entryDateArray[0], out entryDay))
            {
                if (int.TryParse(entryDateArray[1], out entryMonth))
                {
                    if (int.TryParse(entryDateArray[2], out entryYear))
                    {
                        return new DateTime(entryYear, entryMonth, entryDay);
                    }
                }
            }

            return new DateTime(1970, 1, 1);
        }

        /// <summary>
        /// Translate string to <see cref="IJourneyDetailsType"/>
        /// </summary>
        /// <remarks>
        /// Currently total_vehicles isn't being used. 
        /// </remarks>
        /// <param name="rawLine">raw data</param>
        /// <returns>
        /// Converted <see cref="IJourneyDetailsType"/>.
        /// </returns>
        private static IJourneyDetailsType ConvertJourney(
           string rawLine)
        {
            // {day}^t{month}^t{year}^t{journeyNumber}^t{from}^t{to}^t{via}^t{miles-chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
            int currentJourneyDay = 0;
            int currentJourneyMonth = 0;
            int currentJourneyYear = 0;
            DateTime currentJourneyDate = new DateTime();
            MilesChains currentJourneyDistance = new MilesChains();

            string[] currentJourneyArray = rawLine.Split('\t');
            if (currentJourneyArray.Count() < FullJourneyStringSize)
            {
                Logger.Instance.WriteLog($"TRACE: IndividualUnitIOController - Invalid journey line: {rawLine}");
                return null;
            }

            if (int.TryParse(currentJourneyArray[0], out currentJourneyDay))
            {
                if (int.TryParse(currentJourneyArray[1], out currentJourneyMonth))
                {
                    if (int.TryParse(currentJourneyArray[2], out currentJourneyYear))
                    {
                        currentJourneyDate =
                          new DateTime(
                            currentJourneyYear,
                            currentJourneyMonth,
                            currentJourneyDay);
                    }
                }
            }

            currentJourneyDistance.Update(currentJourneyArray[7]);

            List<string> units =
              new List<string>();

            for (int index = 9; index < currentJourneyArray.Length; ++index)
            {
                // Use add to ensure that whitespace is not added. The historical implementation
                // was to always have 4 tabs and leave whitespace where none existed. Now it only
                // uses one tab per unit.
                IndividualUnitIOController.Add(units, currentJourneyArray[index]);
            }


            return
              new JourneyDetailsType(
                currentJourneyDate,
              currentJourneyArray[3],
              currentJourneyArray[4],
              currentJourneyArray[5],
              currentJourneyArray[6],
              currentJourneyDistance,
              units);
        }

        /// <summary>
        /// Convert a string to an integer and return it
        /// </summary>
        /// <param name="input">origin string</param>
        /// <returns>
        /// Converted integer
        /// </returns>
        private static int ConvertInteger(string input)
        {
            int convertedInteger;
            if (int.TryParse(input, out convertedInteger))
            {
                return convertedInteger;
            }

            return 0;
        }

        /// <summary>
        /// Check the input string to determine which <see cref="VehicleServiceType"/>
        /// </summary>
        /// <param name="serviceString">original string</param>
        /// <returns>integer for <see cref="VehicleServiceType"/></returns>
        private static VehicleServiceType GetServiceStatus(
          string serviceString)
        {
            if (serviceString == VehicleServiceType.Preserved.ToString())
            {
                return VehicleServiceType.Preserved;
            }
            else if (serviceString == VehicleServiceType.Withdrawn.ToString())
            {
                return VehicleServiceType.Withdrawn;
            }
            else if (serviceString == VehicleServiceType.Reclassified.ToString())
            {
                return VehicleServiceType.Reclassified;
            }
            else
            {
                return VehicleServiceType.InService;
            }
        }

        /// <summary>
        /// Add <paramref name="newValue"/> to <paramref name="collection"/> if not null or whitespace.
        /// </summary>
        /// <param name="collection">collection of strings</param>
        /// <param name="newValue">value to add to the collection</param>
        private static void Add(
          List<string> collection,
          string newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                collection.Add(newValue);
            }
        }
    }
}