namespace Shap.Common.Converters
{
  using System;
  using System.Globalization;
  using System.Collections.ObjectModel;
  using System.Windows.Data;

  /// <summary>
  /// Converter specifically for the route combo box. It should be disabled if empty, but also if
  /// it contains a single entry with an empty string. This is because that route will always be
  /// auto selected. 
  /// </summary>
  /// <remarks>
  /// The empty string in the collection is needs to be present in order to match routes.
  /// </remarks>
  public class RouteEnabledConverter : IValueConverter
  {
    /// <summary>
    /// Minimum number of entries before enabled (true) is returned.
    /// </summary>
    private const int MinValue = 1;

    /// <summary>
    /// Initialises a new instance of the <see cref="RouteEnabledConverter"/> class.
    /// </summary>
    public RouteEnabledConverter()
    {
    }

    /// <summary>
    /// Convert to enabled flag
    /// </summary>
    /// <param name="value">Input value, a collection of strings is expected</param>
    /// <param name="targetType">target type is not used</param>
    /// <param name="parameter">parameter is not used</param>
    /// <param name="culture">culture is not used</param>
    /// <returns>enabled flag</returns>
    public object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value == null)
      {
        return false;
      }

      if (value.GetType() != typeof(ObservableCollection<string>))
      {
        return false;
      }

      ObservableCollection<string> testValue = (ObservableCollection<string>)value;

      if (testValue.Count == 1 &&
        string.IsNullOrWhiteSpace(testValue[0]))
      {
        return false;
      }

      return testValue.Count >= RouteEnabledConverter.MinValue;
    }

    /// <summary>
    /// Not used
    /// </summary>
    /// <param name="value">value is not used</param>
    /// <param name="targetType">target type is not used</param>
    /// <param name="parameter">parameter is not used</param>
    /// <param name="culture">culture is not used</param>
    /// <returns>no returns</returns>
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