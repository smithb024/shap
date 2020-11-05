namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  /// <summary>
  /// Used to convert a best (personal or season) to a highlighted backcolour.
  /// </summary>
  [ValueConversion(typeof(bool), typeof(Color))]
  public class HighlightBackgroundColourConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      bool testValue = (bool)value;

      return testValue ? new SolidColorBrush(Colors.Maroon) : new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
