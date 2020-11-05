namespace Shap.Types
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;

  // TODO is this now obsolete.
  public class FirstExampleType
  {
    private string   m_item          = string.Empty;
    private DateTime m_date          = new DateTime();
    private string   m_journeyNumber = string.Empty;

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>FirstExampleType</name>
    /// <date>04/01/13</date>
    /// <summary>
    ///   Creates a new instance of the FirstExampleType class 
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public FirstExampleType()
    {
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Gets and sets the item.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public string Item
    {
      get { return m_item; }
      set { m_item = value; }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Gets and sets the date.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public DateTime Date
    {
      get { return m_date; }
      set { m_date = value; }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Gets and sets the index.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public string Index
    {
      get { return m_journeyNumber; }
      set { m_journeyNumber = value; }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Set</name>
    /// <date>13/01/13</date>
    /// <summary>
    ///   set type details from incoming string
    /// </summary>
    /// <param name="firstExampleDate">first example date</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool Set(string firstExampleDate)
    {
      bool     success = true;
      bool     parseOK = true;
      int      day     = 0;
      int      month   = 0;
      int      year    = 0;
      string[] cells   = firstExampleDate.Split('\t');

      if (cells.Count() == 5)
      {
        if (!int.TryParse(cells[1], out day))
        {
          parseOK = false;
        }

        if (!int.TryParse(cells[2], out month))
        {
          parseOK = false;
        }

        if (!int.TryParse(cells[3], out year))
        {
          parseOK = false;
        }

        if (parseOK)
        {
          Item               = cells[0];
          DateTime localDate = new DateTime(year, month, day);
          Date               = localDate;
          Index              = cells[4];
        }
        else
        {
          success = false;
        }
      }
      else
      {
        success = false;
      }

      return success;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Compare</name>
    /// <date>13/01/13</date>
    /// <summary>
    ///   compare incoming data with this one.
    /// </summary>
    /// <param name="firstExample">first example</param>
    /// <returns>is identical</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool Compare(FirstExampleType firstExample)
    {
      bool identical = false;
      if ((Item == firstExample.Item) & (Date == firstExample.Date) & (Index == firstExample.Index))
      {
        identical = true;
      }

      return identical;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ToString</name>
    /// <date>13/01/13</date>
    /// <summary>
    ///   overrides the to string method
    /// </summary>
    /// <returns>string output</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public override string ToString()
    {
      string output = Item + 
                      "\t" + 
                      Date.Day.ToString() +
                      "\t" + 
                      Date.Month.ToString() +
                      "\t" + 
                      Date.Year.ToString() +
                      "\t" + 
                      Index;
      return output;
    }
  }
}
