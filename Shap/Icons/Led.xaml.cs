using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shap.Icons
{
  /// <summary>
  /// Interaction logic for Led.xaml
  /// </summary>
  public partial class Led : UserControl
  {
    /// <summary>
    /// Used to set the day number
    /// </summary>
    public static readonly DependencyProperty LedColourProperty =
        DependencyProperty.Register(
          "LedColour", 
          typeof(Color),
          typeof(Led),
          new PropertyMetadata(Colors.HotPink));

    public Led()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the LED Colour value
    /// </summary>
    public Color LedColour
    {
      get
      {
        return (Color)GetValue(LedColourProperty);
      }

      set
      {
        SetValue(LedColourProperty, value);
      }
    }
  }
}