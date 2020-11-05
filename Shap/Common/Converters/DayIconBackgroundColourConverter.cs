﻿namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  /// <summary>
  /// Used to convert a boolean value to a colour to be used as the day icon background.
  /// </summary>
  [ValueConversion(typeof(bool), typeof(Brush))]
  public class DayIconBackgroundColourConverter : IValueConverter
  {
    public object Convert(
      object value, 
      Type targetType,
      object parameter, 
      System.Globalization.CultureInfo culture)
    {
      if (value.GetType() != typeof(bool))
      {
        return new SolidColorBrush(Colors.HotPink);
      }

      bool testValue = (bool)value;

      return testValue ?
        new SolidColorBrush(Colors.DarkGoldenrod) :
        new SolidColorBrush(Colors.DarkOliveGreen);
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
