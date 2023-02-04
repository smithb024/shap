namespace Shap
{
    using System;
    using System.Windows;

    using NynaeveLib.Logger;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
  {
        /// <summary>
        /// Initialises a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            Console.Write("create Log");
            Logger.SetInitialInstance("ShapLog");

            IocFactory.Setup();
        }
    }
}
