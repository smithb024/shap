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

namespace Shap.Units.Dialog
{
  using NynaeveLib.DialogService.Interfaces;
  /// <summary>
  /// Interaction logic for NewNumberDialog.xaml
  /// </summary>
  public partial class NewNumberDialog : Window, ICloseable
  {
    public NewNumberDialog()
    {
      InitializeComponent();
    }

    public void CloseObject()
    {
      this.Close();
    }
  }
}
