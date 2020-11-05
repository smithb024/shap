namespace Shap
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Media;
  using System.Linq;
  using System.Runtime.CompilerServices; // synchronised.
  using System.Text;

  public class ColourResourcesClass
  {
    private static ColourResourcesClass m_instance = null;

    // Standard Colours
    private Color c_stdForeColour            = Colors.Silver;
    private Color c_stdBackColour            = Color.FromRgb(0,   0,  28);
    private Color c_errorColour              = Colors.Red;
    private Color c_copColour                = Colors.OrangeRed;
    private Color c_firstOfYearColour        = Colors.Gold;
    private Color c_altBackColour            = Color.FromRgb(0,  28,   0);
    private Color c_footerForeColour         = Colors.Black;
    private Color c_noteForeColour           = Colors.Aquamarine;

    // Form Update Colours
    private Color c_itemChangedColour        = Colors.IndianRed;
    private Color c_fieldRequiredColour      = Colors.PaleGreen;
    private Color c_fieldEditColour          = Color.FromRgb(200,   0,   0);

    // Journey Colours
    private Color c_journeyAlternativeColour = Colors.BurlyWood;
    private Color c_journeyLowLightColour    = Colors.DimGray;

    // Vehicle Colours
    private Color c_localVcleColour          = Colors.GreenYellow;

    // Mileage Identification ForeColours
    private Color c_0_100Colour              = Color.FromRgb(0,   228, 255);
    private Color c_100_200Colour            = Color.FromRgb(20,  228, 185);
    private Color c_200_300Colour            = Color.FromRgb(41,  228, 165);
    private Color c_300_400Colour            = Color.FromRgb(62,  228, 143);
    private Color c_400_500Colour            = Color.FromRgb(82,  228, 123);
    private Color c_500_600Colour            = Color.FromRgb(102, 228, 102);
    private Color c_600_700Colour            = Color.FromRgb(123, 228,  82);
    private Color c_700_800Colour            = Color.FromRgb(143, 228,  62);
    private Color c_800_900Colour            = Color.FromRgb(165, 228,  41);
    private Color c_900_1000Colour           = Color.FromRgb(185, 228,  20);
    private Color c_1000Colour               = Color.FromRgb(255, 228,   0);

    // misc
    private Color c_disabledColour           = Colors.PaleGoldenrod;

    // Corporate Colours
    private Color c_brBlack                  = Colors.Black;
    private Color c_brWhite                  = Colors.White;
    private Color c_brYellow                 = Colors.Yellow;
    private Color c_brBlue                   = Color.FromRgb(43,  78, 108);

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ColorResourcesClass</name>
    /// <date>23/02/13</date>
    /// <summary>
    ///   constructor, creates a new log file based on the current time
    ///    and date.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    private ColourResourcesClass()
    {
    }

    // Standard Colours

    /// <summary>
    /// Gets standard fore colour
    /// </summary>
    public Color StdForeColour
    {
      get { return c_stdForeColour; }
    }

    /// <summary>
    /// Gets standard back colour
    /// </summary>
    public Color StdBackColour
    {
      get { return c_stdBackColour; }
    }

    /// <summary>
    /// Gets error colour
    /// </summary>
    public Color ErrorColour
    {
      get { return c_errorColour; }
    }

    /// <summary>
    /// Gets cop colour
    /// </summary>
    public Color CopColour
    {
      get { return c_copColour; }
    }

    /// <summary>
    /// Gets first of year colour
    /// </summary>
    public Color FirstOfYearColour
    {
      get { return c_firstOfYearColour; }
    }

    /// <summary>
    /// Gets alternative back colour
    /// </summary>
    public Color AltBackColour
    {
      get { return c_altBackColour; }
    }

    /// <summary>
    /// Gets footer fore colour
    /// </summary>
    public Color FooterForeColour
    {
      get { return c_footerForeColour; }
    }

    /// <summary>
    /// Gets note fore colour
    /// </summary>
    public Color NoteForeColour
    {
      get { return c_noteForeColour; }
    }

    // Form Update Colours

    /// <summary>
    /// Gets item changed colour
    /// </summary>
    public Color ItemChangedColour
    {
      get { return c_itemChangedColour; }
    }

    /// <summary>
    /// Gets field required colour
    /// </summary>
    public Color FieldRequiredColour
    {
      get { return c_fieldRequiredColour; }
    }

    /// <summary>
    /// Gets field edit colour
    /// </summary>
    public Color FieldEditColour
    {
      get { return c_fieldEditColour; }
    }

    // Journey Colours

    /// <summary>
    /// Gets alternative colour
    /// </summary>
    public Color JourneyAlternativeColour
    {
      get { return c_journeyAlternativeColour; }
    }

    /// <summary>
    /// Gets low light
    /// </summary>
    public Color JourneyLowLightColour
    {
      get { return Colors.HotPink; }
    }

    // Vehicle Colours

    /// <summary>
    /// Gets local colour
    /// </summary>
    public Color LocalVcleColour
    {
      get { return c_localVcleColour; }
    }

    // Mileage Identification ForeColours

    /// <summary>
    /// Gets under 100 colour
    /// </summary>
    public Color _0_100Colour
    {
      get { return c_0_100Colour; }
    }

    /// <summary>
    /// Gets 100 to 200 colour
    /// </summary>
    public Color _100_200Colour
    {
      get { return c_100_200Colour; }
    }

    /// <summary>
    /// Gets 200 to 300 colour
    /// </summary>
    public Color _200_300Colour
    {
      get { return c_200_300Colour; }
    }

    /// <summary>
    /// Gets 300 to 400 colour
    /// </summary>
    public Color _300_400Colour
    {
      get { return c_300_400Colour; }
    }

    /// <summary>
    /// Gets 400 to 500 colour
    /// </summary>
    public Color _400_500Colour
    {
      get { return c_400_500Colour; }
    }

    /// <summary>
    /// Gets 500 to 600 colour
    /// </summary>
    public Color _500_600Colour
    {
      get { return c_500_600Colour; }
    }

    /// <summary>
    /// Gets 600 to 700 colour
    /// </summary>
    public Color _600_700Colour
    {
      get { return c_600_700Colour; }
    }

    /// <summary>
    /// Gets 700 to 800 colour
    /// </summary>
    public Color _700_800Colour
    {
      get { return c_700_800Colour; }
    }

    /// <summary>
    /// Gets 800 to 900 colour
    /// </summary>
    public Color _800_900Colour
    {
      get { return c_800_900Colour; }
    }

    /// <summary>
    /// Gets 900 to 1000 colour
    /// </summary>
    public Color _900_1000Colour
    {
      get { return c_900_1000Colour; }
    }

    /// <summary>
    /// Gets clear colour
    /// </summary>
    public Color _1000Colour
    {
      get { return c_1000Colour; }
    }

    // misc

    /// <summary>
    /// Gets disabled colour
    /// </summary>
    public Color DisabledColour
    {
      get { return c_disabledColour; }
    }

    // Corporate Colours

    /// <summary>
    /// Gets black
    /// </summary>
    public Color BRBlack
    {
      get { return c_brBlack; }
    }

    /// <summary>
    /// Gets white
    /// </summary>
    public Color BRWhite
    {
      get { return c_brWhite; }
    }

    /// <summary>
    /// Gets yellow
    /// </summary>
    public Color BRYellow
    {
      get { return c_brYellow; }
    }

    /// <summary>
    /// Gets blue
    /// </summary>
    public Color BRBlue
    {
      get { return c_brBlue; }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>getInstance</name>
    /// <date>23/02/13</date>
    /// <summary>/
    ///   get instance of this class
    /// </summary>
    /// <returns>instance of this class</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static ColourResourcesClass GetInstance()
    {
      if (m_instance == null)
      {
        m_instance = new ColourResourcesClass();
      }

      return m_instance;
    }
  }
}
