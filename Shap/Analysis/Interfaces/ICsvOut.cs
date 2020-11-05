namespace Shap.Analysis.Interfaces
{
  /// <summary>
  /// Interface for a single object within an analysis report. It supports the ability to be output
  /// as a line within a CSV file.
  /// </summary>
  public interface ICsvOut
  {
    /// <summary>
    /// Gets the Id of the 
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the comma separated output
    /// </summary>
    string CsvOut { get; }

    /// <summary>
    /// Gets the total count.
    /// </summary>
    int Total { get; }

    /// <summary>
    /// Adds one to the month count.
    /// </summary>
    /// <param name="month">month to add one to.</param>
    void AddOne(
      int month);

    /// <summary>
    /// Adds one to the count.
    /// </summary>
    void AddOne();

    /// <summary>
    /// Adds one to the to count.
    /// </summary>
    /// <param name="name">name to add against.</param>
    void AddTo(string name);

    /// <summary>
    /// Adds one to the from count
    /// </summary>
    /// <param name="name">name to add to</param>
    void AddFrom(string name);
  }
}