namespace Shap.Stats
{
  /// <summary>
  /// Local list type, describes the purpose of a first example list.
  /// </summary>
  public enum LocalListType
  {
    /// <summary>
    /// Complete list of first examples.
    /// </summary>
    complete,

    /// <summary>
    /// First examples for the current year only.
    /// </summary>
    annual,

    /// <summary>
    /// Don't know, will have to check.
    /// </summary>
    allPurpose
  }
}
