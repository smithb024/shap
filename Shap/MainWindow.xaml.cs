namespace Shap
{
  using System;
  using System.Windows;

  using Shap.Units.IO;
  using Shap.Stats;

  using NynaeveLib.Logger;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Console.Write("create Log");
      Logger.SetInitialInstance("ShapLog");

      //DailyIOController dailyIoController = new DailyIOController();

      //IDailyInputFactory dailyInputFactory =
      //  new DailyInputFactory
      //  (dailyIoController);

      UnitsIOController unitsIoController = new UnitsIOController();
      UnitsXmlIOController unitsXmlIoController =
        new UnitsXmlIOController(
          unitsIoController);
      //IndividualUnitIOController individualUnitIoController =
      //  new IndividualUnitIOController();
      FirstExampleManager firstExamples =
        new FirstExampleManager();

      this.DataContext =
        new MainWindowViewModel(
          unitsIoController,
          unitsXmlIoController,
          firstExamples);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      Application.Current.Shutdown();
    }
  }
}
