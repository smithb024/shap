namespace Shap.Config
{
  using System.Windows;

  using NynaeveLib.DialogService.Interfaces;

  /// <summary>
  /// Interaction logic for GroupsAndClassesDialog.xaml
  /// </summary>
  public partial class GroupsAndClassesDialog : Window, ICloseable
  {
    public GroupsAndClassesDialog()
    {
      InitializeComponent();
    }

    public void CloseObject()
    {
      this.Close();
    }
  }
}