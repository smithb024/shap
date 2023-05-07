namespace Shap.Common.Converters.Counters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class TotalsBrushConverter : IValueConverter
    {
        static Color Lots = Colors.Blue;
        static Color Many = Colors.RoyalBlue;
        static Color Some = Colors.DodgerBlue;
        static Color Few = Colors.MediumSlateBlue;
        static Color One = Colors.LightSlateGray;

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

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color GetColour(int count)
        {
            if (count >= 1000)
            {
                return Lots;
            }

            if (count >= 500)
            {
                return Many;
            }

            if (count >= 100)
            {
                return Some;
            }

            if (count >= 50)
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
