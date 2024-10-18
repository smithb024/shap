namespace Shap.Common.Converters
{
    using Shap.Types.Enum;
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converter used to convert a <see cref="FeedbackType"/> to a brush.
    /// </summary>
    [ValueConversion(typeof(FeedbackType), typeof(Brush))]
    public class FeedbackTypeBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convert <see cref="FeedbackType"/> to a <see cref="Brush"/>.
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
            Color feedbackColour = Colors.HotPink;

            if (value.GetType() == typeof(FeedbackType))
            {
                return new SolidColorBrush(Colors.HotPink);
            }

            switch ((FeedbackType)value)
            {
                case FeedbackType.Info:
                    feedbackColour = Colors.DarkGoldenrod;
                    break;

                case FeedbackType.Navigation:
                    feedbackColour = Colors.DarkGreen;
                    break;

                case FeedbackType.Fault:
                    feedbackColour = Colors.DarkRed;
                    break;

                case FeedbackType.Command:
                    feedbackColour = Colors.DarkBlue;
                    break;
            }

            SolidColorBrush returnBrush = 
                new SolidColorBrush(
                    feedbackColour);

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