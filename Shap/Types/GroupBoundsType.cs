namespace Shap.Types
{
  public class GroupBoundsType
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="GroupBoundsType"/> class.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    public GroupBoundsType(
      int lowerBound,
      int upperBound)
    {
      this.LowerBound = lowerBound;
      this.UpperBound = upperBound;
    }

    /// <summary>
    /// Gets or set ths lower bound.
    /// </summary>
    public int LowerBound { get; set; }

    /// <summary>
    /// Gets or sets the upper bound.
    /// </summary>
    public int UpperBound { get; set; }

    /// <summary>
    /// Returns a string.
    /// </summary>
    /// <returns>the bounds as a string</returns>
    public override string ToString()
    {
      return $"{this.LowerBound}-{this.UpperBound}";
    }
  }
}