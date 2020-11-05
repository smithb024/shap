namespace Shap.Analysis
{
  using System;

  using Interfaces;

  /// <summary>
  /// Counter object for use with classes
  /// </summary>
  public class ClassCounter : ICsvOut
  {
    public ClassCounter(
      string classId,
      int total = 0)
    {
      this.Id = classId;
      this.Total = total;
    }

    public int Total { get; private set; }

    public string Id { get; private set; }

    public void AddOne()
    {
      ++this.Total;
    }

    public string CsvOut =>
        this.Id +
        ReportFactoryCommon.ColumnSeparator +
        this.Total.ToString();

    public void AddOne(int month)
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
  }
}