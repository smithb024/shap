namespace Shap.Analysis
{
  using System;

  using Interfaces;

  /// <summary>
  /// Counter object for use with years
  /// </summary>
  public class YearCounter : ICsvOut
  {
    public YearCounter(string classId)
      : this(classId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
    {
    }

    public YearCounter(
      string classId,
      int jan,
      int feb,
      int mar,
      int apr,
      int may,
      int jun,
      int jul,
      int aug,
      int sept,
      int oct,
      int nov,
      int dec)
    {
      this.Id = classId;
      this.Jan = jan;
      this.Feb = feb;
      this.Mar = mar;
      this.Apr = apr;
      this.May = may;
      this.Jun = jun;
      this.Jul = jul;
      this.Aug = aug;
      this.Sept = sept;
      this.Oct = oct;
      this.Nov = nov;
      this.Dec = dec;
    }

    public int Jan { get; private set; }

    public int Feb { get; private set; }

    public int Mar { get; private set; }

    public int Apr { get; private set; }

    public int May { get; private set; }

    public int Jun { get; private set; }

    public int Jul { get; private set; }

    public int Aug { get; private set; }

    public int Sept { get; private set; }

    public int Oct { get; private set; }

    public int Nov { get; private set; }

    public int Dec { get; private set; }

    public int Total
    {
      get
      {
        return
          this.Jan +
          this.Feb +
          this.Mar +
          this.Apr +
          this.May +
          this.Jun +
          this.Jul +
          this.Aug +
          this.Sept +
          this.Oct +
          this.Nov +
          this.Dec;
      }
    }

    public string Id { get; private set; }

    public string CsvOut =>
      this.Id +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Jan) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Feb) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Mar) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Apr) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.May) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Jun) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Jul) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Aug) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Sept) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Oct) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Nov) +
      ReportFactoryCommon.ColumnSeparator +
      this.OutputString(this.Dec) +
      ReportFactoryCommon.ColumnSeparator +
      this.Total.ToString();

    public void AddOne(int month)
    {
      switch (month)
          {
            case 1:
              ++this.Jan;
              break;
            case 2:
              ++this.Feb;
              break;
            case 3:
              ++this.Mar;
              break;
            case 4:
              ++this.Apr;
              break;
            case 5:
              ++this.May;
              break;
            case 6:
              ++this.Jun;
              break;
            case 7:
              ++this.Jul;
              break;
            case 8:
              ++this.Aug;
              break;
            case 9:
              ++this.Sept;
              break;
            case 10:
              ++this.Oct;
              break;
            case 11:
              ++this.Nov;
              break;
            case 12:
              ++this.Dec;
              break;
            default:
              break;
          }
    }

    public void AddOne()
    {
      throw new NotImplementedException();
    }

    public void AddTo(string name)
    {
      throw new NotImplementedException();
    }

    public void AddFrom(string name)
    {
      throw new NotImplementedException();
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
      return this.CsvOut;
    }

    /// <summary>
    /// Returns a string to output as part of the results to the CSV file.
    /// </summary>
    /// <remarks>
    /// The method converts the input to a string, except in the case of a zero figure. In that 
    /// case an empty string is returned.
    /// </remarks>
    /// <returns>Output string</returns>
    private string OutputString(int input)
    {
      if (input == 0)
      {
        return string.Empty;
      }

      return input.ToString();
    }
  }
}