namespace Shap.Common.Converters.Counters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converter which is used to paint the background of a counter.
    /// It is used to highlight a total count. 
    /// </summary>
    public class TotalsBrushConverter : IValueConverter
    {
        /// <summary>
        /// Convert from a <see cref="int"/> to a <see cref="SolidColorBrush"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Color brushColour;

            if (value == null)
            {
                brushColour = Colors.Transparent;
            }
            else if (!(value is int))
            {
                brushColour = Colors.Transparent;
            }
            else
            {
                int count = (int)value;

                brushColour = this.GetColour(count);
            }

            return new SolidColorBrush(brushColour);
        }

        /// <summary>
        /// Method not used.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(
            object value,
            Type targetTypes,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decide on colour based on total count.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private Color GetColour(int count)
        {
            if (count >= 1000)
            {
                return CounterColours.Lots;
            }

            if (count >= 500)
            {
                return CounterColours.Many;
            }

            if (count >= 100)
            {
                return CounterColours.Some;
            }

            if (count >= 50)
            {
                return CounterColours.Few;
            }

            if (count >= 1)
            {
                return CounterColours.One;
            }

            return Colors.Transparent;
        }
    }
}
