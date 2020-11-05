namespace Shap.Common.Converters
{
  using System;
  using System.Globalization;
  using System.Collections.ObjectModel;
  using System.Windows.Data;

  public class CollectionToBoolConverter : IValueConverter
  {
    public CollectionToBoolConverter()
    {
      this.MinValue = 0;
    }

    public int MinValue
    {
      get;
      set;
    }

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
      return testValue.Count >= this.MinValue;
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