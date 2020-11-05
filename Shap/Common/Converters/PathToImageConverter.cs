namespace Shap.Common.Converters
{
  using System;
  using System.Globalization;
  using System.Windows.Data;
  using System.Windows.Media.Imaging;

  public class PathToImageConverter : IValueConverter
  {
    public PathToImageConverter()
    {
    }

    public object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value == null)
      {
        return null;
      }

      if (value.GetType() != typeof(string))
      {
        return null;
      }

      return new BitmapImage(new Uri((string)value, UriKind.Relative));
    }

    public object[] ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}