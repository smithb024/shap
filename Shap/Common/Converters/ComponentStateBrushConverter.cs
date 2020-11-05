namespace Shap.Common.Converters
{
  using System;
  using System.Windows.Data;
  using System.Windows.Media;

  using Shap.Types.Enum;

  /// <summary>
  /// Used to convert a <see cref="ComponentState"/> value to a <see cref="Brush"/> to be used
  /// as the day icon background.
  /// </summary>
  [ValueConversion(typeof(ComponentState), typeof(Brush))]
  public class ComponentStateBrushConverter : IValueConverter
  {
    public object Convert(
      object value, 
      Type targetType,
      object parameter, 
      System.Globalization.CultureInfo culture)
    {
      Color convertedColour;

      if (
        value == null ||
        value.GetType() != typeof(ComponentState))
      {
        convertedColour = ColourResourcesClass.GetInstance().ErrorColour;
      }
      else
      {
        ComponentState convertedState = (ComponentState)value;

        switch (convertedState)
        {
          case ComponentState.Cop:
            convertedColour = ColourResourcesClass.GetInstance().CopColour;
            break;

          case ComponentState.CopYear:
            convertedColour = ColourResourcesClass.GetInstance().FirstOfYearColour;
            break;

          case ComponentState.CurrentUnit:
            convertedColour = ColourResourcesClass.GetInstance().LocalVcleColour;
            break;

          case ComponentState.None:
            convertedColour = ColourResourcesClass.GetInstance().JourneyAlternativeColour;
            break;

          case ComponentState.Unknown:
            convertedColour = ColourResourcesClass.GetInstance().JourneyLowLightColour;
            break;

          default:
            convertedColour = ColourResourcesClass.GetInstance().ErrorColour;
            break;
        }
      }

      return new SolidColorBrush(convertedColour);
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
