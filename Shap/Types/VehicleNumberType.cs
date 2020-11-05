namespace Shap.Types
{
  using System.Collections.Generic;
  using System.Linq;
  using NynaeveLib.ViewModel;

  using Shap.Interfaces.Types;

  /// <summary>
  /// vehicle number type
  /// </summary>
  public class VehicleNumberType : ViewModelBase, IVehicleNumberType
  {
    private List<int> m_formerNumber  = new List<int>();

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>VehicleNumberType</name>
    /// <date>12/08/12</date>
    /// <summary>
    ///   Create a new instance of this class
    /// </summary>
    /// <param name="number">vcle number</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public VehicleNumberType(int number)
    {
      this.VehicleNumber = number;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>VehicleNumberType</name>
    /// <date>15/10/12</date>
    /// <summary>
    ///   Empty constructor
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public VehicleNumberType()
      : this (0)
    {
    }

    /// <summary>
    /// Gets the vehicle number
    /// </summary>
    public int VehicleNumber { get; set; }

    public List<int> FormerNumbers
    {
      get
      {
        return this.m_formerNumber;
      }

      private set
      {
        this.m_formerNumber = value;
        this.RaisePropertyChangedEvent("FormerNumbers");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>AddFormerNumber</name>
    /// <date>12/08/12</date>
    /// <summary>
    /// addOldNumber, append an old number.
    /// </summary>
    /// <param name="number">old number</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public void AddFormerNumber(int number)
    {
      this.FormerNumbers.Add(number);
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>GetFormerNumberList</name>
    ///// <date>07/09/12</date>
    ///// <summary>
    /////   get list of Old Numbers
    ///// </summary>
    ///// <returns>list of old numbers</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public List<int> GetFormerNumberList()
    //{
    //  return m_formerNumber;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>GetFormerNumber</name>
    ///// <date>19/08/12</date>
    ///// <summary>
    /////   get the oldNumber at the index specified by the
    /////   index argument.
    ///// </summary>
    ///// <param name="index">index in old number list</param>
    ///// <returns>old number at requested index</returns>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public int GetFormerNumber(int index)
    //{
    //  return m_formerNumber[index];
    //}

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>GetNumberOfFormerNumbers</name>
    /// <date>19/08/12</date>
    /// <summary>
    ///   returns the count of old numbers
    /// </summary>
    /// <returns>count of old numbers</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public int GetNumberOfFormerNumbers() => m_formerNumber.Count();

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ToString</name>
    /// <date>02/11/12</date>
    /// <summary>
    ///   return string of format "no1 tab no2 tab no3"
    /// </summary>
    /// <returns>output string</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public override string ToString()
    {
      string outputString = string.Empty;
      for (int index = 0; index < m_formerNumber.Count(); ++index)
      {
        if (index == 0)
        {
          outputString = m_formerNumber[index].ToString();
        }
        else
        {
          outputString += "\t" + m_formerNumber[index].ToString();
        }
      }

      return outputString;
    }
  }
}
