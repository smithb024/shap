namespace Shap.Config
{
    using System.Windows;

    using NynaeveLib.DialogService.Interfaces;

    /// <summary>
    /// Interaction logic for GroupsAndClassesDialog.xaml
    /// </summary>
    public partial class GroupsAndClassesDialog : Window, ICloseable
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="GroupsAndClassesDialog"/> class.
        /// </summary>
        public GroupsAndClassesDialog()
        {
            this.InitializeComponent();
        }

        public void CloseObject()
        {
            this.Close();
        }
    }
}