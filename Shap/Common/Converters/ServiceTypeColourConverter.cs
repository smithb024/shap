namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  using Types;

  /// <summary>
  /// Used to convert a boolean value to a colour to be used as the day icon background.
  /// </summary>
  [ValueConversion(typeof(VehicleServiceType), typeof(Color))]
  public class ServiceTypeColourConverter : IValueConverter
  {
    public object Convert(
      object value, 
      Type targetType,
      object parameter, 
      System.Globalization.CultureInfo culture)
    {
      if (value.GetType() != typeof(VehicleServiceType))
      {
        return Colors.HotPink;
      }

      return ServiceTypeToBrushHelper.GetColour(
        (VehicleServiceType)value,
        false);
    }

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
