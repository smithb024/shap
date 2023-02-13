namespace Shap.Input
{
    using System;
    using System.Windows;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Interfaces.Input;

    /// <summary>
    /// Interaction logic for InputForm.xaml
    /// </summary>
    public partial class InputForm : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InputForm"/> class.
        /// </summary>
        public InputForm()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<IInputFormViewModel>();

            ((IInputFormViewModel)this.DataContext).ClosingRequest += this.CloseInputWindow;
        }

        /// <summary>
        /// Close the window.
        /// </summary>
        /// <param name="sender">
        /// The object which sent the event.
        /// </param>
        /// <param name="e">Event arguments</param>
        private void CloseInputWindow(object sender, EventArgs e)
        {
            ((IInputFormViewModel)this.DataContext).ClosingRequest -= this.CloseInputWindow;
            this.Close();
        }
    }
}