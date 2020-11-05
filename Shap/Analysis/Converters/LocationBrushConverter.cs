namespace Shap.Analysis.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class LocationBrushConverter : IMultiValueConverter
    {
        static Color Lots = Colors.Blue;
        static Color Many = Colors.RoyalBlue;
        static Color Some = Colors.DodgerBlue;
        static Color Few = Colors.MediumSlateBlue;
        static Color One = Colors.LightSlateGray;

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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color GetSingleYearColours(int count)
        {
            if (count >= 50)
            {
                return Lots;
            }

            if (count >= 10)
            {
                return Many;
            }

            if (count >= 5)
            {
                return Some;
            }

            if (count >= 2)
            {
                return Few;
            }

            if (count >= 1)
            {
                return One;
            }

            return Colors.Transparent;
        }

        private Color GetMultiYearColours(int count)
        {
            if (count >= 500)
            {
                return Lots;
            }

            if (count >= 100)
            {
                return Many;
            }

            if (count >= 50)
            {
                return Some;
            }

            if (count >= 10)
            {
                return Few;
            }

            if (count >= 1)
            {
                return One;
            }

            return Colors.Transparent;
        }
    }
}
