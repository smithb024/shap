namespace Shap.Types
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  using NynaeveLib.Logger;
  using NynaeveLib.Types;
  using NynaeveLib.ViewModel;

  using Shap.Interfaces.Types;

  // TODO - Added unit number to this class fo use with the VehicleDataWindow/VehicleDetailsType
  // even though it's not stricty part of a journey. It's done to make the binding less obscure
  // which I believe is a valid reason. Think this type needs refactoring - thing hierarchy
  // depending on how/where it is being used.
  public class JourneyDetailsType : IJourneyDetailsType
  {
    // TODO - to Refactor - load of methods which should be properties.
    private DateTime m_journeyDate;
    private string m_journeyNumber = string.Empty;
    private string m_journeyFrom = string.Empty;
    private string m_journeyTo = string.Empty;
    private string m_journeyVia = string.Empty;
    private MilesChains m_journeyDistance = new MilesChains();
    //private int          m_totalVehicles   = 0;
    //private string       m_firstNumber     = string.Empty;
    //private string       m_secondNumber    = string.Empty;
    //private string       m_thirdNumber     = string.Empty;
    //private string       m_fourthNumber    = string.Empty;

    private List<string> units;
    private string parentUnitNumber = string.Empty;

    /// <summary>
    /// Creates an instance of the JourneyDetailsType class.
    /// </summary>
    /// <param name="date">journey date</param>
    /// <param name="journeyNumber">journey number</param>
    /// <param name="from">journey from</param>
    /// <param name="to">journey to</param>
    /// <param name="via">journey via</param>
    /// <param name="distance">journey distance</param>
    /// <param name="totalVehicles">total vehicles</param>
    /// <param name="firstNumber">first identifier</param>
    /// <param name="secondNumber">second identifier</param>
    /// <param name="thirdNumber">third identifier</param>
    /// <param name="fourthNumber">fourth identifier</param>
    public JourneyDetailsType(DateTime date,
                              string journeyNumber,
                              string from,
                              string to,
                              string via,
                              MilesChains distance,
                              List<string> units)
    {
      m_journeyDate = date;
      m_journeyNumber = journeyNumber;
      m_journeyFrom = from;
      m_journeyTo = to;
      m_journeyVia = via;
      m_journeyDistance = distance;
      this.units = units;
    }

    ///// <summary>
    ///// Creates an instance of the JourneyDetailsType class.
    ///// </summary>
    ///// <param name="day">journey day</param>
    ///// <param name="month">journey month</param>
    ///// <param name="year">journey year</param>
    ///// <param name="journeyNumber">journey number</param>
    ///// <param name="from">journey from</param>
    ///// <param name="to">journey to</param>
    ///// <param name="via">journey via</param>
    ///// <param name="distance">journey distance</param>
    ///// <param name="totalVehicles">total vehicles</param>
    ///// <param name="firstNumber">first identifier</param>
    ///// <param name="secondNumber">second identifier</param>
    ///// <param name="thirdNumber">third identifier</param>
    ///// <param name="fourthNumber">fourth identifier</param>
    //public JourneyDetailsType(int day,
    //                          int month,
    //                          int year,
    //                          string      journeyNumber,
    //                          string      from,
    //                          string      to,
    //                          string      via,
    //                          MilesChains distance,
    //                          List<string> units)
    //{
    //  m_journeyDate     = new DateTime(year, month, day);
    //  m_journeyNumber   = journeyNumber;
    //  m_journeyFrom     = from;
    //  m_journeyTo       = to;
    //  m_journeyVia      = via;
    //  m_journeyDistance = distance;
    //  this.units = units;
    //}

    /// <summary>
    /// Creates an instance of the JourneyDetailsType class.
    /// </summary>
    /// <param name="journeyDetails">journey details as string</param>
    /// <param name="year">journey year</param>
    /// <param name="month">journey month</param>
    public JourneyDetailsType(string journeyDetails,
                              int year,
                              int month)
    {
      // {day}^t{journeyNumber}^t{from}^t{to}^t{via}^t{miles-chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
      bool inputValid = true;
      string[] cells = journeyDetails.Split('\t');
      int day = 0;
      int firstNumber = 7;

      this.units = new List<string>();

      if (cells.Count() >= 8 && cells.Count() <= 11)
      {
        if (inputValid)
        {
          if (int.TryParse(cells[0], out day))
          {
            m_journeyDate = new DateTime(year, month, day);
          }
          else
          {
            inputValid = false;
          }
        }

        m_journeyNumber = cells[1];
        m_journeyFrom = cells[2];
        m_journeyTo = cells[3];
        m_journeyVia = cells[4];
        m_journeyDistance.Update(cells[5]);

        //if (inputValid)
        //{
        //  if (!int.TryParse(cells[6], out m_totalVehicles))
        //  {
        //    inputValid = false;
        //  }
        //}

        if (inputValid)
        {
          for (int index = firstNumber; index < cells.Length; ++index)
          {
            this.units.Add(cells[index]);
          }
          //m_firstNumber = cells[7];
          //if (cells.Count() > 8)
          //{
          //  m_secondNumber = cells[8];
          //  if (cells.Count() > 9)
          //  {
          //    m_thirdNumber = cells[9];
          //    if (cells.Count() > 10)
          //    {
          //      m_fourthNumber = cells[10];
          //    }
          //  }
          //}
        }
      }
      else
      {
        inputValid = false;
      }

      if (!inputValid)
      {
        m_journeyDate = new DateTime(1970, 1, 1);
        m_journeyFrom = string.Empty;
        m_journeyTo = string.Empty;
        m_journeyVia = string.Empty;
        m_journeyDistance.Update(0, 0);
        this.units = new List<string>();

        Logger.Instance.WriteLog("ERROR: JourneyDetailsType - failed constructor. Set everything to zero.");
      }
    }

    /// <summary>
    /// Gets the Jny Id.
    /// </summary>
    public IJnyId JnyId => new JnyId(this.Date, this.m_journeyNumber);

    /// <summary>
    /// Gets the journey from details.
    /// </summary>
    public string From => this.m_journeyFrom;

    /// <summary>
    /// Gets the journey to details.
    /// </summary>
    public string To => this.m_journeyTo;

    /// <summary>
    /// Gets the journey via details.
    /// </summary>
    public string Via => this.m_journeyVia;

    /// <summary>
    /// Gets the journey distance details.
    /// </summary>
    public MilesChains Distance => this.m_journeyDistance;

    /// <summary>
    /// Gets all unit identifications in the journey.
    /// </summary>
    public List<string> Units => this.units;

    /// <summary>
    /// Gets the journey date.
    /// </summary>
    public DateTime Date => m_journeyDate;

    /// <summary>
    /// Gets the journey date.
    /// </summary>
    public string JourneyDateString => m_journeyDate.ToString("dd/MM/yyyy");

    /// <summary>
    /// Gets the number of the cuurent unit, this is a purely HCI property and is used to identify
    /// the unit number when the <see cref="IJourneyDetailsType"/> is being used for a specific 
    /// unit.
    /// </summary>
    public string ParentUnitNumber
    {
      get
      {
        return parentUnitNumber;
      }
      set
      {
        parentUnitNumber = value;
        //this.RaisePropertyChangedEvent(nameof(this.ParentUnitNumber));
      }
    }

    /// <summary>
    ///   return string
    /// </summary>
    /// <returns>string object</returns>
    public override string ToString()
    {
      string unitsList = this.UnitsToString();

      // {day}^t{journeyNumber}^t{from}^t{to}^t{via}^t{miles-chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
      return string.Format(
        "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}{7}",
        m_journeyDate.Day,
        m_journeyNumber,
        m_journeyFrom,
        m_journeyTo,
        m_journeyVia,
        m_journeyDistance.ToString(),
        this.Units.Count.ToString(),
        unitsList);
    }

    /// <summary>
    ///   return string
    /// </summary>
    /// <returns>string object</returns>
    public string ToFullString()
    {
      string unitsList = this.UnitsToString();

      // {day}^t{month}^t{year}^t{journeyNumber}^t{from}^t{to}^t{via}^t{miles-chains}^t{total_vehicles}^t{number1}^t{number2}^t{number3}^t{number4}
      return string.Format(
        "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}{9}",
        m_journeyDate.Day,
        m_journeyDate.Month,
        m_journeyDate.Year,
        m_journeyNumber,
        m_journeyFrom,
        m_journeyTo,
        m_journeyVia,
        m_journeyDistance.ToString(),
        this.Units.Count.ToString(),
        unitsList);
    }

    //// TODO, why is this a method, not a property.
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get journey date
    ///// </summary>
    ///// <returns>journey details</returns>
    //public DateTime GetJourneyDate()
    //{
    //  return m_journeyDate;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>GetJourneyIndex</name>
    ///// <date>07/01/13</date>
    ///// <summary>
    /////   get journey index
    ///// </summary>
    ///// <returns>journey index</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyIndex()
    //{
    //  return m_journeyNumber;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyFrom</name>
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get journey from
    ///// </summary>
    ///// <returns>journey from</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyFrom()
    //{
    //  return m_journeyFrom;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyTo</name>
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get journey to
    ///// </summary>
    ///// <returns>journey to</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyTo()
    //{
    //  return m_journeyTo;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyVia</name>
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get journey via
    ///// </summary>
    ///// <returns>journey via</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyVia()
    //{
    //  return m_journeyVia;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyDistance</name>
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get journey distance
    ///// </summary>
    ///// <returns>journey distance</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public MilesChains GetJouneyDistance()
    //{
    //  return m_journeyDistance;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyNumberOfVehicles</name>
    ///// <date>04/11/12</date>
    ///// <summary>
    /////   get total vehicles
    ///// </summary>
    ///// <returns>total vehicles</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public int GetJourneyNumberOfVehicles()
    //{
    //  return m_totalVehicles;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyFirstNumber</name>
    ///// <date>18/11/12</date>
    ///// <summary>
    /////   get first number
    ///// </summary>
    ///// <returns>first number</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyFirstNumber()
    //{
    //  return m_firstNumber;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneySecondNumber</name>
    ///// <date>18/11/12</date>
    ///// <summary>
    /////   get second number
    ///// </summary>
    ///// <returns>second number</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneySecondNumber()
    //{
    //  return m_secondNumber;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyThirdNumber</name>
    ///// <date>18/11/12</date>
    ///// <summary>
    /////   get third number
    ///// </summary>
    ///// <returns>third number</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyThirdNumber()
    //{
    //  return m_thirdNumber;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>getJourneyFourthNumber</name>
    ///// <date>18/11/12</date>
    ///// <summary>
    /////   get fourth number
    ///// </summary>
    ///// <returns>fourth number</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public string GetJourneyFourthNumber()
    //{
    //  return m_fourthNumber;
    //}

    ///// <summary>
    ///// Raise the property changed events on all properties.
    ///// </summary>
    //public void RaisePropertyChangedEvents()
    //{
    //  this.RaisePropertyChangedEvent(nameof(this.From));
    //  this.RaisePropertyChangedEvent(nameof(this.To));
    //  //this.RaisePropertyChangedEvent(nameof(this.FirstVcl));
    //  //this.RaisePropertyChangedEvent(nameof(this.SecondVcl));
    //  //this.RaisePropertyChangedEvent(nameof(this.ThirdVcl));
    //  //this.RaisePropertyChangedEvent(nameof(this.FourthVcl));
    //}

    /// <summary>
    /// Convert all the units to a standard string
    /// </summary>
    /// <returns>units string</returns>
    private string UnitsToString()
    {
      string returnString = string.Empty;

      foreach (string unit in this.Units)
      {
        returnString += $"\t{unit}";
      }

      return returnString;
    }
  }
}