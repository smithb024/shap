﻿namespace Shap.Common.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    using Types;

    /// <summary>
    /// Converter used to convert a <see cref="VehicleServiceType"/> to a brush.
    /// </summary>
    [ValueConversion(typeof(VehicleServiceType), typeof(Brush))]
    public class ServiceTypeBrushConverter : IValueConverter
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
            if (value.GetType() != typeof(VehicleServiceType))
            {
                return new SolidColorBrush(Colors.HotPink);
            }

            return new SolidColorBrush(
              ServiceTypeToBrushHelper.GetColour(
                (VehicleServiceType)value,
                true));
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