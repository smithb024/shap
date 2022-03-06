namespace Shap.Common.Converters
{
    using System.Windows.Media;

    using Types;

    /// <summary>
    /// Convert <see cref="VehicleServiceType"/> to a <see cref="Color"/>.
    /// </summary>
    public static class ServiceTypeToBrushHelper
    {
        /// <summary>
        /// Return a colour
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="isBackground"></param>
        /// <returns></returns>
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