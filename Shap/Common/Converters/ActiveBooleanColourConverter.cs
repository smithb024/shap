namespace Shap.Common.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Used to convert a boolean value to a colour to be used in the led icon.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Color))]
    public class ActiveBooleanColourConverter : IValueConverter
    {
        /// <summary>
        /// Convert from a <see cref="bool"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="value">the <see cref="bool"/> to convert</param>
        /// <param name="targetType">target type is not used</param>
        /// <param name="parameter">parameter is not used</param>
        /// <param name="culture">culture is not used</param>
        /// <returns>the converted <see cref="Color"/></returns>
        public object Convert(
          object value,
          Type targetType,
          object parameter,
          System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
            {
                return Colors.HotPink;
            }

            Color returnColour =
                (bool)value
                ? Color.FromRgb(00, 50, 00)
                : Colors.Maroon;

            return returnColour;
        }

        /// <summary>
        /// Convert back is not in use.
        /// </summary>
        /// <param name="value">value is not used</param>
        /// <param name="targetType">target type is not used</param>
        /// <param name="parameter">parameter is not used</param>
        /// <param name="culture">culture is not used</param>
        /// <returns>Not used</returns>
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