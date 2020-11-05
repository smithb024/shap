namespace Shap.Types
{
  /// <summary>
  /// Service category
  /// </summary>
  public enum VehicleServiceType
  {
    /// <summary>
    /// Vehicle is in service - default.
    /// </summary>
    InService,

    /// <summary>
    /// Vehicle has been withdrawn from service.
    /// </summary>
    Withdrawn,

    /// <summary>
    /// Vehicle is privately owned.
    /// </summary>
    Preserved,

    /// <summary>
    /// Vehicle has been reclassified to a different class.
    /// </summary>
    Reclassified
  }
}
