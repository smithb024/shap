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
using System.Windows.Shapes;

// TODO Enter a value in the field and ok button is not automatically enabled. Why???
namespace Shap.Units.Dialog
{
  using NynaeveLib.DialogService.Interfaces;

  /// <summary>
  /// Interaction logic for NewSubClassDialog.xaml
  /// </summary>
  public partial class NewSubClassDialog : Window, ICloseable
  {
    public NewSubClassDialog()
    {
      InitializeComponent();
    }

    public void CloseObject()
    {
      this.Close();
    }
  }
}
