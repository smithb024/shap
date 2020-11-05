namespace Shap.StationDetails
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using NynaeveLib.ViewModel;
  using Shap.Types;

  public class MileageDetailsViewModel : ViewModelBase
  {

    //private const int    c_chainsOffset      = 375;
    //private const int    c_distanceWidth     = 30;
    //private const int    c_fromOffset        = 0;
    //private const int    c_journeyWidth      = 165;
    //private const int    c_labelHeight       = 17;
    //private const int    c_labelTopOffset    = 3;
    //private const int    c_milesOffset       = 335;
    //private const int    c_rowHeight         = 23;
    //private const int    c_separatorOffset   = 365;
    //private const int    c_separatorWidth    = 10;
    //private const int    c_toOffset          = 177;
    //private const int    c_viaOffset         = 405;
    //private const int    c_viaWidth          = 349;
    //private const string c_titleBarImg       = "titlebarwide.jpg";

    //private Main                m_parent                     = null;
    private JourneyIOController journeyController            ;
    //private ArrayList           fromStationLabel             = new ArrayList();
    //private int                 maxNumberOfJourneysDisplayed = 50;
    //private Point               m_mouseDownLocation          = new Point();
    //private Point               m_origLocation               = new Point();
    //private Font                c_journeyStdFont             = new Font("Monotype Corsiva", 10, FontStyle.Italic);
    private ObservableCollection<RouteDetailsType> routes;
    private ObservableCollection<string> stnList;
    private string stn;

    /// <summary>
    /// Initialise a new instance of the <see cref="MileageDetailsViewModel"/> class.
    /// </summary>
    public MileageDetailsViewModel()
    {
      this.journeyController = JourneyIOController.Instance;
      routes = new ObservableCollection<RouteDetailsType>();
      this.RefreshCmd = new StnDetailsCmd(this, CalculateRoutes);

      InitialiseComboBoxPrimary();
    }

    /// <summary>
    /// Gets or sets the stn list
    /// </summary>
    public ObservableCollection<string> StnList
    {
      get
      {
        return this.stnList;
      }
      set
      {
        this.stnList = value;
        this.RaisePropertyChangedEvent("StnList");
      }
    }

    /// <summary>
    /// Gets or sets the current stn.
    /// </summary>
    public string Stn
    {
      get
      {
        return this.stn;
      }
      set
      {
        this.stn = value;
        this.RaisePropertyChangedEvent("Stn");
        this.CalculateRoutes();
      }
    }

    /// <summary>
    /// Gets or sets the routes
    /// </summary>
    public ObservableCollection<RouteDetailsType> Routes
    {
      get
      {
        return this.routes;
      }
      set
      {
        this.routes = value;
        this.RaisePropertyChangedEvent("Routes");
      }
    }

    /// <summary>
    /// Refresh all.
    /// </summary>
    public ICommand RefreshCmd
    {
      get;
      private set;
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>MilageDetailsForm</name>
    ///// <date>27/10/12</date>
    ///// <summary>
    ///// Creates an instance of the MileageDetailsForm class
    ///// </summary>
    ///// <param name="mainForm">parent form</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //public MilageDetailsForm(Main mainForm)
    //{
    //  m_parent = mainForm;
    //  InitializeComponent();

    //  // location
    //  this.Location      = new Point(450, 0);
    //  this.StartPosition = FormStartPosition.Manual;
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>loadMilageDetailsForm</name>
    ///// <date>06/05/13</date>
    ///// <summary>
    /////   Add the value "All" to the primary combo box.
    /////   Calls all stations to be added to the combo box.
    /////   Sets the comboBoxPrimary index to 0.
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void LoadMilageDetailsForm(object sender, EventArgs e)
    //{
    //  // The form is loaded.
    //  comboBoxPrimary.Items.Add("All");
    //  InitialiseComboBoxPrimary();
    //  comboBoxPrimary.SelectedIndex = 0;
    //  try
    //  {
    //    panelTitleBar.BackgroundImage = Image.FromFile(BasePathReader.GetBasePath() + 
    //                                                   StaticResources.imgPath + 
    //                                                   c_titleBarImg);
    //  }
    //  catch (Exception ex)
    //  {
    //    Logger logger = Logger.Instance;
    //    logger.log("ERROR: MilageDetailsForm: " + ex.ToString());
    //  }
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ButtonRefresh_Click</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////    Clears the comboBox Primary, before refreshing it. As part of
    /////     the refresh is assumes that All journeys are to be displayed.
    /////   Add the value "All" to the primary combo box.
    /////   Calls for all journeys to be displayed.
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ButtonRefresh_Click(object sender, EventArgs e)
    //{
    //  comboBoxPrimary.Items.Clear();

    //  // reinitialise
    //  comboBoxPrimary.Items.Add("All");
    //  InitialiseComboBoxPrimary();
    //  comboBoxPrimary.SelectedIndex = 0;
    //}

    /// <summary>
    /// Refresh the stn list.
    /// </summary>
    private void RefreshStnList()
    {
      this.InitialiseComboBoxPrimary();
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ComboBoxPrimary_SelectedIndexChanged</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////    Call initialisePageNumberComboBox, this initialises the Page
    /////     controls. There page 1 is selected for whatever is selected
    /////     in comboBoxPrimary and all the journeys are displayed.
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ComboBoxPrimary_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  InitialisePageNumberComboBox();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ButtonFirstPage_Click</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    ///// Sets the comboBoxPageNumber the first one.
    /////   Sets First and Previous buttons to unavailable
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ButtonFirstPage_Click(object sender, EventArgs e)
    //{
    //  comboBoxPageNumber.SelectedIndex = 0;
    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ButtonPreviousPage_Click</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////  Sets the comboBoxPageNumber the previous one.
    /////   Checks the availability of the page navigation controls
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ButtonPreviousPage_Click(object sender, EventArgs e)
    //{
    //  comboBoxPageNumber.SelectedIndex = comboBoxPageNumber.SelectedIndex - 1;
    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ComboBoxPageNumber_SelectedIndexChanged</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////  New Page, firstly it clears everything from the journey panel.
    /////   Prints the new page to the journey panel.
    /////   Checks the availability of the page navigation controls.
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ComboBoxPageNumber_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //  // New page, deleted everything from the journey panel
    //  foreach (Label obj in fromStationLabel)
    //  {
    //    obj.Dispose();
    //  }

    //  fromStationLabel.Clear();

    //  if (string.Equals(comboBoxPrimary.SelectedItem.ToString(), "All"))
    //  {
    //    OutputToMilagePanel(maxNumberOfJourneysDisplayed * comboBoxPageNumber.SelectedIndex);
    //  }
    //  else
    //  {
    //    OutputToMilagePanel(comboBoxPrimary.SelectedItem.ToString(), maxNumberOfJourneysDisplayed * comboBoxPageNumber.SelectedIndex);
    //  }

    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ButtonNextPage_Click</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////    Sets the comboBoxPageNumber the next one.
    /////   Checks the availability of the page navigation controls
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ButtonNextPage_Click(object sender, EventArgs e)
    //{
    //  comboBoxPageNumber.SelectedIndex = comboBoxPageNumber.SelectedIndex + 1;
    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>ButtonLastPage_Click</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////    Sets the comboBoxPageNumber the last one.
    /////   Checks the availability of the page navigation controls
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Event Args</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void ButtonLastPage_Click(object sender, EventArgs e)
    //{
    //  comboBoxPageNumber.SelectedIndex = comboBoxPageNumber.Items.Count - 1;
    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>MilageDetailsForm_FormClosed</name>
    ///// <date>09/12/12</date>
    ///// <summary>
    /////   Tell the parent that the form has closed.
    ///// </summary>
    ///// <param name="sender">form object</param>
    ///// <param name="e">Form Closed Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void MilageDetailsForm_FormClosed(object sender, FormClosedEventArgs e)
    //{
    //  m_parent.CloseMileageDetailsForm();
    //}

    /// <summary>
    /// Close the window.
    /// </summary>
    private void CloseWindow()
    {
      this.OnClosingRequest();
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>labelClose_Click</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// label click
    ///// </summary>
    ///// <param name="sender">label object</param>
    ///// <param name="e">Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void LabelClose_Click(object sender, EventArgs e)
    //{
    //  this.Close();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>panelTitleBar_MouseDown</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// mouse click
    ///// </summary>
    ///// <param name="sender">mouse object</param>
    ///// <param name="e">Mouse Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
    //{
    //  MouseButtonDown(e);
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>PanelTitleBar_MouseUp</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// mouse click
    ///// </summary>
    ///// <param name="sender">mouse object</param>
    ///// <param name="e">Mouse Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void PanelTitleBar_MouseUp(object sender, MouseEventArgs e)
    //{
    //  MouseButtonUp(e);
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>LabelTitleBar_MouseDown</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// mouse click
    ///// </summary>
    ///// <param name="sender">mouse object</param>
    ///// <param name="e">Mouse Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void LabelTitleBar_MouseDown(object sender, MouseEventArgs e)
    //{
    //  MouseButtonDown(e);
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>LabelTitleBar_MouseUp</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// mouse click
    ///// </summary>
    ///// <param name="sender">mouse object</param>
    ///// <param name="e">Mouse Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void LabelTitleBar_MouseUp(object sender, MouseEventArgs e)
    //{
    //  MouseButtonUp(e);
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>panelMilage_MouseEnter</name>
    ///// <date>29/12/12</date>
    ///// <summary>
    ///// panel mouse enter
    ///// </summary>
    ///// <param name="sender">panel object</param>
    ///// <param name="e">Event argument</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void PanelMilage_MouseEnter(object sender, EventArgs e)
    //{
    //  if (this == Form.ActiveForm)
    //  {
    //    panelMilage.Focus();
    //  }
    //}

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>InitialiseComboBoxPrimary</name>
    /// <date>06/05/13</date>
    /// <summary>
    ///   Loops through all the from Stations and adds them to the 
    ///     comboBoxPrimary combo box. It only adds one of each station, 
    ///     since they are sorted alphabetically it does this by checking 
    ///     against the previous value.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    private void InitialiseComboBoxPrimary()
    {
      this.stnList = new ObservableCollection<string>();
      this.stnList.Add(string.Empty);

      string previousvalue = string.Empty;
      string location      = string.Empty;

      for (int i = 0; i < journeyController.GetMileageDetailsLength(); i++)
      {
        location = journeyController.GetFromStation(i);
        if (location != previousvalue)
        {
          this.stnList.Add(location);
        }

        previousvalue = location;
      }
    }

    /// <summary>
    /// Get all the routes equalling the from stn.
    /// </summary>
    private void CalculateRoutes()
    {
      this.Routes = new ObservableCollection<RouteDetailsType>();

      for (int index = 0; index < journeyController.GetMileageDetailsLength(); ++index)
      {
        if (NynaeveLib.Utils.StringCompare.SimpleCompare(
          journeyController.GetFromStation(index),
          this.Stn))
        {
          this.Routes.Add(journeyController.GetRoute(index));
        }
      }
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>OutputToMilagePanel</name>
    ///// <date>07/04/12</date>
    ///// <summary>
    /////   loops though all all journeys and outputs them to the Mileage 
    /////     panel. It stops if it runs out of journeys, or it fills the 
    /////     maximum number of allowed rows.
    /////     startIndex indicates the first journey to print.
    ///// </summary>
    ///// <param name="startIndex">start index</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void OutputToMilagePanel(int startIndex = 0)
    //{
    //  int row = 0;

    //  for (int i = startIndex; 
    //       i < journeyController.GetMileageDetailsLength() && row < maxNumberOfJourneysDisplayed;
    //       ++i)
    //  {
    //    OutputRow(row, i);
    //    ++row;
    //  }
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>OutputToMilagePanel</name>
    ///// <date>07/04/12</date>
    ///// <summary>
    /////   loops though all all journeys from the station indicated by 
    /////     fromLocation and outputs them to the Mileage panel. It stops if 
    /////     it runs out of journeys, or it fills the maximum number of 
    /////     allowed rows.
    /////   startIndex indicates the first example journey to print. Since
    /////     the index of the first one is not known, it counts up to 
    /////     startIndex before beginning to print.
    ///// </summary>
    ///// <param name="fromLocation">from location</param>
    ///// <param name="startIndex">start index</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void OutputToMilagePanel(string fromLocation, int startIndex = 0)
    //{
    //  int row         = 0;
    //  int numberFound = 0;

    //  for (int i = 0;
    //       i < journeyController.GetMileageDetailsLength() && row < maxNumberOfJourneysDisplayed;
    //       ++i)
    //  {
    //    if (string.Equals(fromLocation, journeyController.GetFromStation(i)))
    //    {
    //      if (numberFound >= startIndex)
    //      {
    //        OutputRow(row, i);
    //        ++row;
    //      }

    //      ++numberFound;
    //    }
    //  }
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>OutputRow</name>
    ///// <date>24/02/13</date>
    ///// <summary>
    /////   output a new row
    ///// </summary>
    ///// <param name="row">output row</param>
    ///// <param name="arrayIndex">array index</param>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void OutputRow(int row, int arrayIndex)
    //{
    //  ColourResourcesClass colourResources = ColourResourcesClass.GetInstance();

    //  Label fromLabel     = new Label();
    //  fromLabel.Text      = journeyController.GetFromStation(arrayIndex);
    //  fromLabel.Left      = c_fromOffset;
    //  fromLabel.Height    = c_labelHeight;
    //  fromLabel.Width     = c_journeyWidth;
    //  fromLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  fromLabel.ForeColor = colourResources.StdForeColour;
    //  fromLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(fromLabel);
    //  fromStationLabel.Add(fromLabel);

    //  Label toLabel     = new Label();
    //  toLabel.Text      = journeyController.GetToStation(arrayIndex);
    //  toLabel.Left      = c_toOffset;
    //  toLabel.Height    = c_labelHeight;
    //  toLabel.Width     = c_journeyWidth;
    //  toLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  toLabel.ForeColor = colourResources.StdForeColour;
    //  toLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(toLabel);
    //  fromStationLabel.Add(toLabel);

    //  Label milesLabel     = new Label();
    //  milesLabel.Text      = journeyController.GetMiles(arrayIndex);
    //  milesLabel.Left      = c_milesOffset;
    //  milesLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  milesLabel.Height    = c_labelHeight;
    //  milesLabel.Width     = c_distanceWidth;
    //  milesLabel.ForeColor = colourResources.StdForeColour;
    //  milesLabel.TextAlign = ContentAlignment.MiddleRight;
    //  milesLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(milesLabel);
    //  fromStationLabel.Add(milesLabel);

    //  Label separatorLabel     = new Label();
    //  separatorLabel.Text      = "-";
    //  separatorLabel.Left      = c_separatorOffset;
    //  separatorLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  separatorLabel.Height    = c_labelHeight;
    //  separatorLabel.Width     = c_separatorWidth;
    //  separatorLabel.ForeColor = colourResources.StdForeColour;
    //  separatorLabel.TextAlign = ContentAlignment.MiddleCenter;
    //  separatorLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(separatorLabel);
    //  fromStationLabel.Add(separatorLabel);

    //  Label chainsLabel     = new Label();
    //  chainsLabel.Text      = journeyController.GetChains(arrayIndex);
    //  chainsLabel.Left      = c_chainsOffset;
    //  chainsLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  chainsLabel.Height    = c_labelHeight;
    //  chainsLabel.Width     = c_distanceWidth;
    //  chainsLabel.ForeColor = colourResources.StdForeColour;
    //  chainsLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(chainsLabel);
    //  fromStationLabel.Add(chainsLabel);

    //  Label viaLabel     = new Label();
    //  viaLabel.Text      = journeyController.GetViaRoute(arrayIndex);
    //  viaLabel.Left      = c_viaOffset;
    //  viaLabel.Height    = c_labelHeight;
    //  viaLabel.Width     = c_viaWidth;
    //  viaLabel.Top       = (row * c_rowHeight) + c_labelTopOffset;
    //  viaLabel.ForeColor = colourResources.StdForeColour;
    //  viaLabel.Font      = c_journeyStdFont;
    //  this.panelMilage.Controls.Add(viaLabel);
    //  fromStationLabel.Add(viaLabel);
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>InitialisePageNumberComboBox</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////   Works out how many pages are needed to display all journeys.
    /////   Adds a page number to the comboBoxPageNumber for each page.
    /////   Selects index 0.
    /////   Reset Page navigation controls.
    ///// </summary>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void InitialisePageNumberComboBox()
    //{
    //  int numberOfPages = 0;

    //  comboBoxPageNumber.Items.Clear();

    //  if (string.Equals(comboBoxPrimary.SelectedItem.ToString(), "All"))
    //  {
    //    numberOfPages = journeyController.GetMileageDetailsLength() / maxNumberOfJourneysDisplayed;
    //    if (journeyController.GetMileageDetailsLength() % maxNumberOfJourneysDisplayed > 0)
    //    {
    //      ++numberOfPages;
    //    }
    //  }
    //  else
    //  {
    //    numberOfPages = journeyController.GetMileageDetailsLength(comboBoxPrimary.SelectedItem.ToString()) / maxNumberOfJourneysDisplayed;
    //    if (journeyController.GetMileageDetailsLength(comboBoxPrimary.SelectedItem.ToString()) % maxNumberOfJourneysDisplayed > 0)
    //    {
    //      ++numberOfPages;
    //    }
    //  }

    //  for (int i = 1; i <= numberOfPages; ++i)
    //  {
    //    comboBoxPageNumber.Items.Add("Page " + i.ToString());
    //  }

    //  comboBoxPageNumber.SelectedIndex = 0;

    //  UpdatePageNavigationAvailablily();
    //}

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>UpdatePageNavigationAvailablily</name>
    ///// <date>08/04/12</date>
    ///// <summary>
    /////   Works out which page navigation buttons should be displayed/
    /////   hidden and displays/hides them.
    ///// </summary>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void UpdatePageNavigationAvailablily()
    //{
    //  if (comboBoxPageNumber.Items.Count == 1)
    //  {
    //    buttonFirstPage.Enabled    = false;
    //    buttonPreviousPage.Enabled = false;
    //    comboBoxPageNumber.Enabled = false;
    //    buttonNextPage.Enabled     = false;
    //    buttonLastPage.Enabled     = false;
    //  }
    //  else
    //  {
    //    comboBoxPageNumber.Enabled = true;
    //    if (comboBoxPageNumber.SelectedIndex == 0)
    //    {
    //      buttonFirstPage.Enabled    = false;
    //      buttonPreviousPage.Enabled = false;
    //      buttonNextPage.Enabled     = true;
    //      buttonLastPage.Enabled     = true;
    //    }
    //    else if (comboBoxPageNumber.SelectedIndex == comboBoxPageNumber.Items.Count - 1)
    //    {
    //      buttonFirstPage.Enabled    = true;
    //      buttonPreviousPage.Enabled = true;
    //      buttonNextPage.Enabled     = false;
    //      buttonLastPage.Enabled     = false;
    //    }
    //    else
    //    {
    //      buttonFirstPage.Enabled    = true;
    //      buttonPreviousPage.Enabled = true;
    //      buttonNextPage.Enabled     = true;
    //      buttonLastPage.Enabled     = true;
    //    }
    //  }
    //}

  }
}