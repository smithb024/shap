namespace Shap
{
    using System;
    using System.Windows;

    using NynaeveLib.Logger;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Shap.Interfaces;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            Console.Write("create Log");
            Logger.SetInitialInstance("ShapLog");

            this.DataContext = Ioc.Default.GetService<IMainWindowViewModel>();
        }

        /// <summary>
        /// The Window has closed. Close the application.
        /// </summary>
        /// <param name="sender">The main window view</param>
        /// <param name="e">The event arguments</param>
        private void Window_Closed(
            object sender, 
            EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}