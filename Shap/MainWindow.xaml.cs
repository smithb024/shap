namespace Shap
{
    using System;
    using System.Windows;

    using Shap.Io;
    using Shap.Interfaces.Io;
    using Shap.Units.IO;
    using Shap.Stats;

    using NynaeveLib.Logger;

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
            InitializeComponent();

            Console.Write("create Log");
            Logger.SetInitialInstance("ShapLog");

            IIoControllers ioControllers = new IoControllers();

            FirstExampleManager firstExamples =
              new FirstExampleManager();

            this.DataContext =
              new MainWindowViewModel(
                ioControllers,
                firstExamples);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}