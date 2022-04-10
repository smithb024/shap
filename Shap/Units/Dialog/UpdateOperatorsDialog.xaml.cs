namespace Shap.Units.Dialog
{
    using System.Windows;
    using NynaeveLib.DialogService.Interfaces;

    /// <summary>
    /// Interaction logic for UpdateOperatorsDialog.xaml
    /// </summary>
    public partial class UpdateOperatorsDialog : Window, ICloseable
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateOperatorsDialog"/> class.
        /// </summary>
        public UpdateOperatorsDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Close this dialog.
        /// </summary>
        public void CloseObject()
        {
            this.Close();
        }
    }
}