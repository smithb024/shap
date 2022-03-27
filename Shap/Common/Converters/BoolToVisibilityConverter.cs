namespace Shap.Common.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows;

    /// <summary>
    /// Convert, converts a <see cref="bool"/> to a <see cref="Visibility"/>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert a <see cref="bool"/> to a <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="targetType">target type is not used</param>
        /// <param name="parameter">parameter is not used</param>
        /// <param name="culture">culture is not used</param>
        /// <returns>A <see cref="visibility"/></returns>
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(bool))
            {
                return Visibility.Collapsed;
            }

            return (bool)value 
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(
          object value,
          Type targetType,
          object parameter,
          CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}