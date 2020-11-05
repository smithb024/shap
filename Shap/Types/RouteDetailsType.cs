namespace Shap.Types
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using NynaeveLib.Types;
  using NynaeveLib.ViewModel;

  public class RouteDetailsType : ViewModelBase
  {
    private string from;
    private string to;
    private string via;
    private string key;
    private MilesChains distance;

    /// <summary>
    /// Initialises a new instance of the <see cref="RouteDetailsType"/> class.
    /// </summary>
    /// <param name="from">from value</param>
    /// <param name="to">to value</param>
    /// <param name="via">via value</param>
    /// <param name="key">key value</param>
    /// <param name="distance">distance value</param>
    public RouteDetailsType(
      string from,
      string to,
      string via,
      string key,
      MilesChains distance)
    {
      this.from = from;
      this.to = to;
      this.via = via;
      this.key = key;
      this.distance = distance;
    }

    /// <summary>
    /// Initialise a new instance of the <see cref="RouteDetailsType"/> class.
    /// </summary>
    /// <param name="input">input string based on the to string output</param>
    public RouteDetailsType(string input)
    {
      string[] cells = input.Split('\t');
      int miles;
      int chains;

      if (cells.Count() != 6)
      {
        return;
      }

      if (!int.TryParse(cells[2], out miles))
      {
        return;
      }

      if (!int.TryParse(cells[3], out chains))
      {
        return;
      }

      this.from = cells[0];
      this.to = cells[1];
      this.distance = new MilesChains(miles, chains);
      this.via = cells[4];
      this.key = cells[5];
    }

    /// <summary>
    /// From property
    /// </summary>
    public string From
    {
      get
      {
        return this.from;
      }

      set
      {
        this.from = value;
        this.RaisePropertyChangedEvent("From");
      }
    }

    /// <summary>
    /// To property
    /// </summary>
    public string To
    {
      get
      {
        return this.to;
      }

      set
      {
        this.to = value;
        this.RaisePropertyChangedEvent("To");
      }
    }

    /// <summary>
    /// Via property
    /// </summary>
    public string Via
    {
      get
      {
        return this.via;
      }

      set
      {
        this.via = value;
        this.RaisePropertyChangedEvent("Via");
      }
    }

    /// <summary>
    /// Key property
    /// </summary>
    public string Key
    {
      get
      {
        return this.key;
      }

      set
      {
        this.key = value;
        this.RaisePropertyChangedEvent("Key");
      }
    }

    /// <summary>
    /// Distance propety
    /// </summary>
    public MilesChains Distance
    {
      get
      {
        return this.distance;
      }

      set
      {
        this.distance = value;
        this.RaisePropertyChangedEvent("Distance");
      }
    }

    /// <summary>
    /// Write object to a string.
    /// </summary>
    /// <returns>string value</returns>
    public override string ToString()
    {
      return string.Format(
        "{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
        this.From,
        this.To,
        this.Distance.Miles.ToString(),
        this.Distance.Chains.ToString(),
        this.Via,
        this.Key);
    }
  }
}