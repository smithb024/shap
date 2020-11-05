namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  using Types;

  [ValueConversion(typeof(VehicleServiceType), typeof(Brush))]
  public class ServiceTypeBrushConverter : IValueConverter
  {
    public object Convert(
      object value, 
      Type targetType,
      object parameter, 
      System.Globalization.CultureInfo culture)
    {
      if (value.GetType() != typeof(VehicleServiceType))
      {
        return new SolidColorBrush(Colors.HotPink);
      }

      return new SolidColorBrush(
        ServiceTypeToBrushHelper.GetColour(
          (VehicleServiceType)value,
          true));
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
