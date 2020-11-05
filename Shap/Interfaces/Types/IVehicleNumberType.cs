namespace Shap.Interfaces.Types
{
  using System.Collections.Generic;
  using NynaeveLib.ViewModel;

  /// <summary>
  /// vehicle number type
  /// </summary>
  public interface IVehicleNumberType : IViewModelBase
  {
    /// <summary>
    /// Gets the vehicle number
    /// </summary>
    int VehicleNumber { get; set; }

    /// <summary>
    /// Gets the list of former numbers
    /// </summary>
    List<int> FormerNumbers { get; }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>AddFormerNumber</name>
    /// <date>12/08/12</date>
    /// <summary>
    /// addOldNumber, append an old number.
    /// </summary>
    /// <param name="number">old number</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    void AddFormerNumber(int number);

    /// <date>19/08/12</date>
    /// <summary>
    ///   returns the count of old numbers
    /// </summary>
    /// <returns>count of old numbers</returns>
    int GetNumberOfFormerNumbers();

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ToString</name>
    /// <date>02/11/12</date>
    /// <summary>
    ///   return string of format "no1 tab no2 tab no3"
    /// </summary>
    /// <returns>output string</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    string ToString();
  }
}
