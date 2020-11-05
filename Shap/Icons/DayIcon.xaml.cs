using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shap.Icons
{
  using System.Windows.Media;
  using System.Windows.Controls;
  using System.Windows;

  /// <summary>
  /// Interaction logic for DayIcon.xaml
  /// </summary>
  public partial class DayIcon : UserControl
  {
    /// <summary>
    /// Used to set the day number
    /// </summary>
    public static readonly DependencyProperty DayNumberProperty =
        DependencyProperty.Register(
          "DayNumber", 
          typeof(string),
          typeof(DayIcon),
          new PropertyMetadata("0"));

    /// <summary>
    /// Used to set the day detail
    /// </summary>
    public static readonly DependencyProperty DayDetailProperty =
        DependencyProperty.Register(
          "DayDetail",
          typeof(string),
          typeof(DayIcon),
          new PropertyMetadata("0"));

    /// <summary>
    /// Used to highlight this icon
    /// </summary>
    public static readonly DependencyProperty DaySelectedBrushProperty =
        DependencyProperty.Register(
          "DaySelectedBrush",
          typeof(Brush),
          typeof(DayIcon),
          new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

    /// <summary>
    /// Used to set the background of this icon.
    /// </summary>
    public static readonly DependencyProperty BackgroundBrushProperty =
        DependencyProperty.Register(
          "BackgroundBrush",
          typeof(Brush),
          typeof(DayIcon),
          new PropertyMetadata(Brushes.Blue));

    /// <summary>
    /// Initialises a new instance of the <see cref="DayIcon"/> class.
    /// </summary>
    public DayIcon()
    {
      this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the day number value
    /// </summary>
    public string DayNumber
    {
      get
      {
        return (string)GetValue(DayNumberProperty);
      }

      set
      {
        SetValue(DayNumberProperty, value);
      }
    }

    /// <summary>
    /// Gets or sets the day detail value
    /// </summary>
    public string DayDetail
    {
      get
      {
        return (string)GetValue(DayDetailProperty);
      }

      set
      {
        SetValue(DayDetailProperty, value);
      }
    }

    /// <summary>
    /// Gets or sets the brush used to indicate if this <see cref="DayIcon"/> is currently selected.
    /// </summary>
    public Brush DaySelectedBrush
    {
      get
      {
        return (Brush)GetValue(DaySelectedBrushProperty);
      }

      set
      {
        SetValue(DaySelectedBrushProperty, value);
      }
    }

    /// <summary>
    /// Gets or sets the colour used for the background of this <see cref="DayIcon"/>.
    /// </summary>
    public Brush BackgroundBrush
    {
      get
      {
        return (Brush)GetValue(BackgroundBrushProperty);
      }

      set
      {
        SetValue(BackgroundBrushProperty, value);
      }
    }

  }
}