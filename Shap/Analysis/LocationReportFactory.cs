namespace Shap.Analysis
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Common;
  using NynaeveLib.Logger;

  using Shap.Input.Factories;
  using Shap.Interfaces.Types;
  using Types;

  public static class LocationReportFactory
  {
    /// <summary>
    ///   Run a stn report. It counts the number of occurrences of each
    ///     stn across all records.
    /// </summary>
    /// <returns>is successful</returns>
    public static ReportCounterManager<LocationCounter> RunStnGeneralReport()
    {
      string[] dirNamesArray =
        System.IO.Directory.GetDirectories(
          $"{BasePathReader.GetBasePath()}{StaticResources.baPath}");

      ReportCounterManager<LocationCounter> locationTotals =
        LocationReportFactory.CreateLocations();

      for (int index = 0; index < dirNamesArray.Count(); ++index)
      {
        // get directory name from the path and convert it into it's integer value.
        string dirName =
          dirNamesArray[index].Substring(
            dirNamesArray[index].LastIndexOf('\\') + 1);
        LocationReportFactory.UpdateStnsForYear(
          locationTotals,
          dirName);
      }

      return locationTotals;
      //locationTotals.WriteCSVFile(
      //  $"StnReport_Gen_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv",
      //  "ReportBuilder: Failed to write General Stn Report.");
    }

    /// <summary>
    ///   Run a stn report. It counts the number of occurrences of each
    ///     stn in the given year.
    /// </summary>
    /// <param name="year">chosen year</param>
    /// <param name="fullList">fullList</param>
    /// <returns>success flag</returns>
    public static ReportCounterManager<LocationCounter> RunStnAnnualReport(
      string year,
      bool fullList)
    {
      ReportCounterManager<LocationCounter> locationTotals =
        LocationReportFactory.CreateLocations();

      LocationReportFactory.UpdateStnsForYear(
        locationTotals,
        year);

      if (!fullList)
      {
        locationTotals.RemoveEmptyClasses();
      }

      return locationTotals;

      //string writeName = $"StnReport_{year}_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv";
      //string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write Annual Stn Report for {year}.";

      //locationTotals.WriteCSVFile(
      //  writeName,
      //  faultMessage);
    }

    /// <summary>
    ///   Run a report based on a stn. It counts the number of other
    ///     stns visited from the given stn across all records.
    /// </summary>
    /// <param name="stn">stn name</param>
    /// <param name="fullList">full list of locations</param>
    /// <returns>is successful</returns>
    public static ReportCounterManager<LocationCounter> RunSingleStnGeneralReport(
      string stn,
      bool fullList)
    {
      string[] dirNamesArray =
        System.IO.Directory.GetDirectories(
          BasePathReader.GetBasePath() + StaticResources.baPath);

      ReportCounterManager<LocationCounter> locationTotals =
        LocationReportFactory.CreateLocations();

      for (int i = 0; i < dirNamesArray.Count(); ++i)
      {
        // get directory name from the path and convert it into it's integer value.
        string dirName = dirNamesArray[i].Substring(dirNamesArray[i].LastIndexOf('\\') + 1);
        LocationReportFactory.UpdateStnsForYear(
          locationTotals,
          dirName,
          stn);
      }

      if (!fullList)
      {
        locationTotals.RemoveEmptyClasses();
      }

      return locationTotals;

      //string writeName = $"{stn}_Report_Gen_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv";
      //string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write General Stn Report for {stn}.";

      //locationTotals.WriteCSVFile(
      //  writeName,
      //  faultMessage);
    }

    /// <summary>
    ///   Run a report based on a stn and the year. It counts the number
    ///     of other stns visited from the given stn in the given year.
    /// </summary>
    /// <param name="year">current year</param>
    /// <param name="stn">stn name</param>
    /// <param name="fullList">full list of locations</param>
    /// <returns name="success">is successful</returns>
    public static ReportCounterManager<LocationCounter> RunSingleStnAnnualReport(
      string year,
      string stn,
      bool fullList)
    {
      ReportCounterManager<LocationCounter> locationTotals =
        LocationReportFactory.CreateLocations();

      LocationReportFactory.UpdateStnsForYear(
        locationTotals,
        year,
        stn);

      if (!fullList)
      {
        locationTotals.RemoveEmptyClasses();
      }

      return locationTotals;

      //string writeName = $"{stn}_Report_{year}_{DateTime.Now.ToString(ReportFactoryCommon.DatePattern)}.csv";
      //string faultMessage = $"ReportBuilder: ReportBuilder: Failed to write Annual Stn Report for {stn} in {year}.";

      //locationTotals.WriteCSVFile(
      //  writeName,
      //  faultMessage);
    }

    /// <summary>
    ///   Loops through all month files in a year directory and analyses
    ///     each journey. It updates the relevant arrays.
    ///   If singleStn is set then it is measuring all journeys for the
    ///     specified stn argument. If not set then it is counting all
    ///     stns.
    /// </summary>
    /// <param name="year">year to update</param>
    /// <param name="singleStn">single stn flag</param>
    /// <param name="stn">stn name</param>
    /// <returns name="success">success flag</returns>
    private static void UpdateStnsForYear(
      ReportCounterManager<LocationCounter> locations,
      string year,
      string stn = "")
    {
      int yearInteger = 0;
      string monthNumber = string.Empty;

      // Convert year into integer
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
          if (string.IsNullOrEmpty(stn))
          {
            locations.AddOne(
              currentJourneyDetails.To,
              currentJourneyDetails.From);
          }
          else
          {
            LocationReportFactory.UpdateArraysForSingleStn(
              locations,
              stn,
              currentJourneyDetails);
          }
        }
      }
    }

    /// <summary>
    ///   Takes a stn and a journey, if the stn and the journey to stn
    ///     match then increase the toLocation arrays. Similar for the 
    ///     from stn.
    /// </summary>
    /// <param name="stn">stn name</param>
    /// <param name="currentJourneyDetails">current jny details</param>
    private static void UpdateArraysForSingleStn(
      ReportCounterManager<LocationCounter> locations,
      string stn,
      IJourneyDetailsType currentJourneyDetails)
    {
      if (currentJourneyDetails.To == stn)
      {
        locations.AddFrom(currentJourneyDetails.From);
      }
      else if (currentJourneyDetails.From == stn)
      {
        locations.AddTo(currentJourneyDetails.To);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private static ReportCounterManager<LocationCounter> CreateLocations()
    {
      ReportCounterManager<LocationCounter> locationTotals =
        new ReportCounterManager<LocationCounter>();

      List<FirstExampleType> firstExampleList =
        Stats.FirstExampleIOController.GetInstance().GetFirstExampleListLocation();
      firstExampleList = firstExampleList.OrderBy(loc => loc.Item).ToList();

      foreach (FirstExampleType location in firstExampleList)
      {
        locationTotals.AddNewCounter(
          new LocationCounter(
            location.Item));
      }

      return locationTotals;
    }
  }
}