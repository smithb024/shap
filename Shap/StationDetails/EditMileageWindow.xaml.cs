namespace Shap.StationDetails
{
    using System;
    using System.Windows;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Interfaces.StationDetails;

    /// <summary>
    /// Interaction logic for EditMileageWindow.xaml
    /// </summary>
    public partial class EditMileageWindow : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EditMileageViewModel"/> class.
        /// </summary>
        public EditMileageWindow()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<IEditMileageViewModel>();

            ((IEditMileageViewModel)this.DataContext).ClosingRequest += this.CloseConfigurationWindow;
        }

        /// <summary>
        /// Close the window.
        /// </summary>
        /// <param name="sender">
        /// The object which sent the event.
        /// </param>
        /// <param name="e">Event arguments</param>
        private void CloseConfigurationWindow(object sender, EventArgs e)
        {
            ((IEditMileageViewModel)this.DataContext).ClosingRequest -= this.CloseConfigurationWindow;
            this.Close();
        }
    }
}