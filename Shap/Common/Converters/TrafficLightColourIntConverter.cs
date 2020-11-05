namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  [ValueConversion(typeof(int), typeof(Brush))]
  public class TrafficLightColourIntConverter : IValueConverter
  {
    public TrafficLightColourIntConverter()
    {
      this.UpperNumber = 10;
      this.LowerNumber = 5;
    }

    public int UpperNumber { get; set; }

    public int LowerNumber { get; set; }

    public object Convert(
      object value, 
      Type targetType,
      object parameter, 
      System.Globalization.CultureInfo culture)
    {
      if (value == null || value.GetType() != typeof(int))
      {
        return new SolidColorBrush(Colors.Gray);
      }

      int input = (int)value;

      if (input >= this.UpperNumber)
      {
        return new SolidColorBrush(Colors.Green);
      }

      if (input >= this.LowerNumber)
      {
        return new SolidColorBrush(Colors.Goldenrod);
      }

      return new SolidColorBrush(Colors.Red);
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
