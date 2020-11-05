namespace Shap.Types.Enum
{
  /// <summary>
  /// Describes the state of a component.
  /// </summary>
  public enum ComponentState
  {
    /// <summary>
    /// The state is normal.
    /// </summary>
    None,

    /// <summary>
    /// The state is not known.
    /// </summary>
    Unknown,

    /// <summary>
    /// This is the unit described in the parent view
    /// </summary>
    CurrentUnit,

    /// <summary>
    /// This is a cop for the year.
    /// </summary>
    CopYear,

    /// <summary>
    /// This is a cop.
    /// </summary>
    Cop
  }
}
