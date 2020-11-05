namespace Shap.Analysis
{
  using System;

  using Interfaces;

  /// <summary>
  /// Counter object for use with classes
  /// </summary>
  public class LocationCounter : ICsvOut
  {
    public LocationCounter(
      string name)
    {
      this.Id = name;
      this.To = 0;
      this.From = 0;
    }

    public int To { get; private set; }

    public int From { get; private set; }

    public int Total => this.To + this.From;

    public string CsvOut =>
        this.Id+
        ReportFactoryCommon.ColumnSeparator +
        this.From +
        ReportFactoryCommon.ColumnSeparator +
        this.To +
        ReportFactoryCommon.ColumnSeparator +
        this.Total;

    public string Id { get; private set; }

    public void AddOne(int month)
    {
      throw new NotImplementedException();
    }

    public void AddOne()
    {
      throw new NotImplementedException();
    }

    public void AddTo(string name)
    {
      if (string.Compare(this.Id, name) == 0)
      {
        ++this.To;
      }
    }

    public void AddFrom(string name)
    {
      if (string.Compare(this.Id, name) == 0)
      {
        ++this.From;
      }
    }

    /// <summary>
    ///   overrides the to string method
    /// </summary>
    /// <returns>string output</returns>
    public override string ToString()
    {
      return this.CsvOut;
    }
  }
}
