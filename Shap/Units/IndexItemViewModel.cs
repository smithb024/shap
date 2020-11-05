namespace Shap.Units
{
  using System;
  using System.Windows.Input;
  using NynaeveLib.ViewModel;
  using Shap.Common.Commands;
  using Shap.Units.IO;
  using Common;
  using Stats;

  public class IndexItemViewModel : ViewModelBase
  {
    /// <summary>
    /// Manager class holding collections of the first examples.
    /// </summary>
    private FirstExampleManager firstExamples;

    private string indexName;
    private bool inConfigurationMode;

    private UnitsIOController unitsIoController;
    private UnitsXmlIOController unitsXmlIoController;

    ClassConfigWindow classConfigWindow;
    ClassFrontPage classFrontPageWindow;

    /// <summary>
    /// Initialises a new instance of the <see cref="IndexItemViewModel"/> class.
    /// </summary>
    /// <param name="unitsIoController">units IO controller</param>
    /// <param name="unitsXmlIoController">units XML IO controller</param>
    /// <param name="individualUnitIoController">individual Unit IO controller</param>
      /// <param name="name">item name</param>
    public IndexItemViewModel(
      UnitsIOController unitsIoController,
      UnitsXmlIOController unitsXmlIoController,
      FirstExampleManager firstExamples,
      string name)
    {
      this.unitsIoController = unitsIoController;
      this.unitsXmlIoController = unitsXmlIoController;
      this.firstExamples = firstExamples;
      this.indexName = name;
      this.inConfigurationMode = false;
      this.OpenWindowCmd = new CommonCommand(this.ShowClassWindow);
    }

    /// <summary>
    /// Gets or sets the index name.
    /// </summary>
    public string IndexName
    {
      get
      {
        return this.indexName;
      }

      set
      {
        this.indexName = value;
        this.RaisePropertyChangedEvent(nameof(this.IndexName));
        this.RaisePropertyChangedEvent(nameof(this.IndexImagePath));
      }
    }

    /// <summary>
    /// Index for the list of sub class image lists.
    /// </summary>
    public string IndexImagePath
    {
      get
      {
//      return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        //return "C:\\_myDocs\\bert\\03_projects\\my_programing\\cSharpWPF\\ShapDevelopment\\Shap\\data\\uts\\img\\37.jpg";
       // string returnString = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\" +
       string returnString = BasePathReader.GetBasePathUri() +
          StaticResources.classIconPath +
          this.IndexName +
          ".jpg";

        return returnString;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the class is in configuration mode or not.
    /// </summary>
    public bool InConfigurationMode
    {
      get
      {
        return this.inConfigurationMode;
      }

      set
      {
        this.inConfigurationMode = value;
        this.RaisePropertyChangedEvent("InConfigurationMode");
      }
    }

    /// <summary>
    /// Close window command.
    /// </summary>
    public ICommand OpenWindowCmd
    {
      get;
      private set;
    }

    /// <summary>
    /// Show a new window, which one depends on the mode.
    /// </summary>
    public void ShowClassWindow()
    {
      if (this.InConfigurationMode)
      {
        this.ShowClassConfigWindow();
      }
      else
      {
        this.ShowClassFrontPage();
      }
    }

    /// <summary>
    /// Show the new class config window. Manage it so multiple examples are not shown. If the 
    /// front page window exists, show that instead.
    /// </summary>
    public void ShowClassConfigWindow()
    {
      if (this.classFrontPageWindow != null)
      {
        this.classFrontPageWindow.Focus();
        return;
      }

      if (this.classConfigWindow == null)
      {
        ClassConfigViewModel classConfig =
          new ClassConfigViewModel(
            this.unitsIoController,
            this.unitsXmlIoController,
            this.indexName);

        SetupWindow(
          this.classConfigWindow = new ClassConfigWindow(),
          classConfig,
          this.CloseClassConfigWindow,
          this.EditClassConfigWindowClosed);
      }

      this.classConfigWindow.Focus();
    }

    /// <summary>
    /// Form closed, set to null.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void CloseClassConfigWindow(object sender, EventArgs e)
    {
      this.classConfigWindow.Close();
    }

    /// <summary>
    /// Form closed, set to null.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void EditClassConfigWindowClosed(object sender, EventArgs e)
    {
      this.classConfigWindow = null;
    }

    /// <summary>
    /// Show the new class front page window. Manage it so multiple examples are not shown. If
    /// the config window exists, show that instead.
    /// </summary>
    public void ShowClassFrontPage()
    {
      if (this.classConfigWindow!= null)
      {
        this.classConfigWindow.Focus();
        return;
      }

      if (this.classFrontPageWindow == null)
      {
        this.classFrontPageWindow = new ClassFrontPage();
        ClassFunctionalViewModel classFunctionalViewModel =
          new ClassFunctionalViewModel(
            this.unitsIoController,
            this.unitsXmlIoController,
            this.firstExamples,
            this.indexName);

        SetupWindow(
          this.classFrontPageWindow,
          classFunctionalViewModel,
          this.CloseClassFrontPageWindow,
          this.EditClassFrontPageClosed);
      }

      this.classFrontPageWindow.Focus();
    }

    /// <summary>
    /// Form closed, set to null.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void CloseClassFrontPageWindow(object sender, EventArgs e)
    {
      this.classFrontPageWindow.Close();
    }

    /// <summary>
    /// Form closed, set to null.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void EditClassFrontPageClosed(object sender, EventArgs e)
    {
      this.classFrontPageWindow = null;
    }

    /// <summary>
    /// Setup and show a window.
    /// </summary>
    /// <param name="window">window to set up</param>
    /// <param name="viewModel">view model to assign to the view model</param>
    /// <param name="closedViewMethod">request from the view model to close the view</param>
    /// <param name="closedMethod">method to run when the window closes</param>
    public void SetupWindow(
      System.Windows.Window window,
      NynaeveLib.ViewModel.ViewModelBase viewModel,
      EventHandler closeViewMethod,
      EventHandler closedMethod)
    {
      window.DataContext = viewModel;

      viewModel.ClosingRequest += closeViewMethod;
      window.Closed += closedMethod;

      window.Show();
      window.Activate();
    }
  }
}
