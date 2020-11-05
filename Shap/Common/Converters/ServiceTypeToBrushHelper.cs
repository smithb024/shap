namespace Shap.Common.Converters
{
  using System.Windows.Media;

  using Types;

  public static class ServiceTypeToBrushHelper
  {
    public static Color GetColour(
      VehicleServiceType serviceType,
      bool isBackground)
    {
      switch (serviceType)
      {
        case VehicleServiceType.InService:
          return isBackground ? Color.FromRgb(00, 50, 00) : Colors.DarkOliveGreen;
        case VehicleServiceType.Preserved:
          return Colors.Navy;
        case VehicleServiceType.Reclassified:
          return Colors.DarkSlateGray;
        case VehicleServiceType.Withdrawn:
          return Colors.Maroon;

        default:
          return Colors.HotPink;
      }
    }
  }
}