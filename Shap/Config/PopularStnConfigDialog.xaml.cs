namespace Shap.Config
{
  using System;
  using System.Windows;

  using NynaeveLib.DialogService.Interfaces;

  /// <summary>
  /// Interaction logic for PopularStnConfigDialog.xaml
  /// </summary>
  public partial class PopularStnConfigDialog : Window, ICloseable
  {
    public PopularStnConfigDialog()
    {
      InitializeComponent();
    }

    public void CloseObject()
    {
      this.Close();
    }
  }
}
