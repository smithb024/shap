namespace Shap.Common.Converters.Counters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converter which is used to paint the background of a counter.
    /// It is used to highlight the location count. 
    /// </summary>
    public class LocationBrushConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convert from a <see cref="int"/> to a <see cref="SolidColorBrush"/>
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(
            object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Color brushColour;

            if (values == null || values.Length != 2)
            {
                brushColour = Colors.Transparent;
            }
            else if (!(values[0] is int))
            {
                brushColour = Colors.Transparent;
            }
            else if (!(values[1] is bool))
            {
                brushColour = Colors.Transparent;
            }
            else
            {
                int count = (int)values[0];
                bool isYear = (bool)values[1];

                brushColour =
                    isYear
                    ? this.GetSingleYearColours(count)
                    : this.GetMultiYearColours(count);
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
        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decide on colour based on count over a single year.
        /// </summary>
        /// <param name="count">count to convert</param>
        /// <returns>background colour</returns>
        private Color GetSingleYearColours(int count)
        {
            if (count >= 50)
            {
                return CounterColours.Lots;
            }

            if (count >= 10)
            {
                return CounterColours.Many;
            }

            if (count >= 5)
            {
                return CounterColours.Some;
            }

            if (count >= 2)
            {
                return CounterColours.Few;
            }

            if (count >= 1)
            {
                return CounterColours.One;
            }

            return Colors.Transparent;
        }

        /// <summary>
        /// Decide on colour based on count over many years.
        /// </summary>
        /// <param name="count">count to convert</param>
        /// <returns>background colour</returns>
        private Color GetMultiYearColours(int count)
        {
            if (count >= 500)
            {
                return CounterColours.Lots;
            }

            if (count >= 100)
            {
                return CounterColours.Many;
            }

            if (count >= 50)
            {
                return CounterColours.Some;
            }

            if (count >= 10)
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
