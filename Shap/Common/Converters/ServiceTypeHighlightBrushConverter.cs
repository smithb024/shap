namespace Shap.Common.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    using Types;

    /// <summary>
    /// Converter used to convert a <see cref="VehicleServiceType"/> to a brush.
    /// </summary>
    [ValueConversion(typeof(VehicleServiceType), typeof(Brush))]
    public class ServiceTypeHighlightBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convert <see cref="VehicleServiceType"/> to a <see cref="Brush"/>.
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="targetType">target type is not used.</param>
        /// <param name="parameter">parameter is not used</param>
        /// <param name="culture">culture is not used</param>
        /// <returns>a <see cref="Brush"/></returns>
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          System.Globalization.CultureInfo culture)
        {
            Color brushColour;

            if (value.GetType() != typeof(VehicleServiceType))
            {
                brushColour = Colors.HotPink;
            }
            else
            {
                switch ((VehicleServiceType)value)
                {
                    case VehicleServiceType.InService:
                        brushColour = Colors.MediumSeaGreen;
                        break;
                    case VehicleServiceType.Preserved:
                        brushColour = Colors.Goldenrod;
                        break;
                    case VehicleServiceType.Reclassified:
                    case VehicleServiceType.Withdrawn:
                    default:
                        brushColour = Colors.Gray;
                        break;
                }
            }

            SolidColorBrush returnBrush =
                new SolidColorBrush(
                    brushColour);
            return returnBrush;
        }

        /// <summary>
        /// Convert back is not used.
        /// </summary>
        /// <param name="value">value is not used.</param>
        /// <param name="targetType">target type is not used.</param>
        /// <param name="parameter">parameter is not used</param>
        /// <param name="culture">culture is not used</param>
        /// <returns>Not applicable</returns>
        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}