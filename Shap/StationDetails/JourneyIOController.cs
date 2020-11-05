namespace Shap.StationDetails
{
  using System;
  using System.Collections; // ArrayList
  using System.Collections.Generic; // List<T>
  using System.Collections.ObjectModel;
  using System.IO; // stream reader/writer copy
  using System.Linq;
  using System.Runtime.CompilerServices; // synchronised.
  using System.Text;
  using NynaeveLib.Types;
  using Shap.Common;
  using Shap.Types;

  public class JourneyIOController
  {
    private static JourneyIOController instance    = null;
    private List<RouteDetailsType> routes = new List<RouteDetailsType>();

    /// <summary>
    /// Initialises a new instance of the <see cref="JourneyIOController"/> class.
    /// </summary>
    private JourneyIOController()
    {
      InitialiseMileageDetails();
    }

    public static JourneyIOController Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new JourneyIOController();
        }

        return instance;
      }
    }

    /// <summary>
    /// Returns an instance of this class
    /// </summary>
    /// <returns>an instance of this class</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static JourneyIOController GetInstance()
    {
      if (instance == null)
      {
        instance = new JourneyIOController();
      }

      return instance;
    }

    /// <summary>
    /// saves the current set of mileage details.
    /// </summary>
    public void SaveMileageDetails()
    {
      StreamWriter writer   = null;
      string       basePath = BasePathReader.GetBasePath();
      List<string> mileageLines = new List<string>();

      // Make a back up of the Milage file.
      System.IO.File.Copy(basePath + StaticResources.mileagePath + "Milage.txt",
                          basePath + StaticResources.mileagePath + "Milage_backup.txt",
                          true);

      foreach (RouteDetailsType route in this.routes)
      {
        mileageLines.Add(route.ToString());
      }

      mileageLines.Sort();

      using (writer = new StreamWriter(basePath + StaticResources.mileagePath + "Milage.txt"))
      {
        foreach (string line in mileageLines)
        {
          writer.WriteLine(line);
        }
      }

      // TODO - why initialise
      //this.InitialiseMileageDetails();
    }

    /// <summary>
    /// Return all to values which are associated with a from value
    /// </summary>
    /// <param name="from">the from destination</param>
    /// <returns>all to destinations in alphabetical order</returns>
    public ObservableCollection<string> GetToCollection(
      string from)
    {
      List<string> toList = new List<string>();
      ObservableCollection<string> returnCollection = new ObservableCollection<string>();
      returnCollection.Add(string.Empty);
      string previousValue = string.Empty;

      for (int index = 0; index < this.GetMileageDetailsLength(); ++index)
      {
        if (string.Equals(from, this.GetFromStation(index)))
        {
          toList.Add(this.GetToStation(index));
        }
      }

      if (toList.Count > 0)
      {
        toList.Sort();

        foreach (string nextString in toList)
        {
          if (nextString != previousValue)
          {
            returnCollection.Add(nextString);
          }

          previousValue = nextString;
        }
      }

      return returnCollection;
    }

    /// <summary>
    /// Return all via values which are associated with a from and a to value.
    /// </summary>
    /// <param name="from">the from destination</param>
    /// <param name="to">the to destination</param>
    /// <returns>all via destinations in alphabetical order</returns>
    public ObservableCollection<string> GetViaCollection(
      string from,
      string to)
    {
      List<string> viaList = new List<string>();
      ObservableCollection<string> returnCollection =
        new ObservableCollection<string>();
      returnCollection.Add(string.Empty);
      string previousValue = string.Empty;

      for (int index = 0; index < this.GetMileageDetailsLength(); ++index)
      {
          if (string.Equals(from, this.GetFromStation(index)) &&
            string.Equals(to, this.GetToStation(index)))
        {
          viaList.Add(this.GetViaStation(index));
        }
      }

      if (viaList.Count > 0)
      {
        viaList.Sort();

        foreach (string nextString in viaList)
        {
          if (nextString != previousValue)
          {
            returnCollection.Add(nextString);
          }

          previousValue = nextString;
        }
      }

      return returnCollection;
    }

    /// <summary>
    /// Get the distance for the requested journey
    /// </summary>
    /// <param name="from">from location</param>
    /// <param name="to">to location</param>
    /// <param name="via">via location</param>
    /// <returns>distance, zero if not found</returns>
    public MilesChains GetDistance(
      string from,
      string to,
      string via)
    {
      for (int i = 0; i < this.GetMileageDetailsLength(); ++i)
      {
        if (string.Equals(from, this.GetFromStation(i))
          && string.Equals(to, this.GetToStation(i))
          && string.Equals(via, this.GetViaRoute(i)))
        {
          return new MilesChains(
            this.GetMiles(i),
            this.GetChains(i));
        }
      }

      return new MilesChains();
    }

    /// <summary>
    /// Get size of mileage details list
    /// </summary>
    /// <returns>number of routes</returns>
    public int GetMileageDetailsLength()
    {
      return this.routes.Count();
    }

    /// <summary>
    ///   returns the number of the arguments from the locations list.
    /// </summary>
    /// <param name="fromLocation">from location</param>
    /// <returns>mileage details list size</returns>
    public int GetMileageDetailsLength(string fromLocation)
    {
      return this.routes.Where(x => x.Equals(fromLocation)).Count();
    }

    /// <summary>
    /// return instance of location
    /// </summary>
    /// <param name="index">location index</param>
    /// <returns>chosen location</returns>
    public RouteDetailsType GetRoute(int index)
    {
      return this.routes[index];
    }

    /// <summary>
    /// return instance of location
    /// </summary>
    /// <param name="index">location index</param>
    /// <returns>chosen location</returns>
    public string GetFromStation(int index)
    {
      return this.routes[index].From;
    }

    /// <summary>
    /// return instance of location
    /// </summary>
    /// <param name="index">location index</param>
    /// <returns>chosen location</returns>
    public string GetToStation(int index)
    {
      return this.routes[index].To;
    }

    /// <summary>
    /// return instance of location
    /// </summary>
    /// <param name="index">location index</param>
    /// <returns>chosen location</returns>
    public string GetViaStation(int index)
    {
      return this.routes[index].Via;
    }

    ///// <summary>
    ///// returns the distance
    ///// </summary>
    ///// <param name="index">distance index</param>
    ///// <returns>chosen distance</returns>
    //public string GetMiles(int index)
    //{
    //  return this.routes[index].Distance.Miles.ToString();
    //}

    /// <summary>
    /// returns the distance
    /// </summary>
    /// <param name="index">distance index</param>
    /// <returns>chosen distance</returns>
    public int GetMiles(int index)
    {
      return this.routes[index].Distance.Miles;
    }

    ///// <summary>
    ///// return distance instance
    ///// </summary>
    ///// <param name="index">distance index</param>
    ///// <returns>chosen distance</returns>
    //public string GetChains(int index)
    //{
    //  return this.routes[index].Distance.Chains.ToString();
    //}

    /// <summary>
    /// return distance instance
    /// </summary>
    /// <param name="index">distance index</param>
    /// <returns>chosen distance</returns>
    public int GetChains(int index)
    {
      return this.routes[index].Distance.Chains;
    }

    /// <summary>
    /// return chosen route
    /// </summary>
    /// <param name="index">route index</param>
    /// <returns>chosen distance</returns>
    public string GetViaRoute(int index)
    {
      return this.routes[index].Via;
    }

    /// <summary>
    /// return chosen key
    /// </summary>
    /// <param name="index">key index</param>
    /// <returns>chosen key</returns>
    public string GetMileageKey(int index)
    {
      return this.routes[index].Key;
    }

    /// <summary>
    /// set new location
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="location">new location</param>
    public void PutFromStation(int index, string location)
    {
      this.routes[index].From = location;
    }

    /// <summary>
    /// add new route
    /// </summary>
    /// <param name="route">new route</param>
    public void PutRoute(RouteDetailsType route)
    {
      this.routes.Add(route);
    }

    /// <summary>
    /// set new location
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="location">new location</param>
    public void PutToStation(int index, string location)
    {
      this.routes[index].To = location;
    }

    /// <summary>
    /// set new distance
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="distance">new distance</param>
    public void PutMiles(int index, int distance)
    {
      this.routes[index].Distance.Miles = distance;
    }

    /// <summary>
    /// set new distance
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="distance">new distance</param>
    public void PutChains(int index, int distance)
    {
      this.routes[index].Distance.Chains = distance;
    }

    /// <summary>
    /// set new distance
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="route">new route</param>
    public void PutViaRoute(int index, string route)
    {
      this.routes[index].Via = route;
    }

    /// <summary>
    /// set new key
    /// </summary>
    /// <param name="index">location index</param>
    /// <param name="key">new key</param>
    public void PutMileageKey(int index, string key)
    {
      this.routes[index].Key = key;
    }

    ///// ---------- ---------- ---------- ---------- ---------- ----------
    ///// <name>refreshMileageDetails</name>
    ///// <date>09/12/12</date>
    ///// <summary>
    /////   reset everything
    ///// </summary>
    ///// ---------- ---------- ---------- ---------- ---------- ----------
    //private void RefreshMileageDetails()
    //{
    //  fromStation.Clear();
    //  toStation.Clear();
    //  miles.Clear();
    //  chains.Clear();
    //  viaRoute.Clear();
    //  mileageKey.Clear();

    //  InitialiseMileageDetails();
    //}

    /// <summary>
    ///   Initialise the mileages details.
    /// </summary>
    private void InitialiseMileageDetails()
    {
      routes = new List<RouteDetailsType>();

      using (StreamReader reader = new StreamReader(BasePathReader.GetBasePath() +
                                                    StaticResources.mileagePath +
                                                    "Milage.txt"))
      {
        string currentLine = string.Empty;
        currentLine = reader.ReadLine();
        while (currentLine != null)
        {
          RouteDetailsType newRoute = new RouteDetailsType(currentLine);
          if (newRoute.From != null)
          {
            this.routes.Add(newRoute);
          }

          //string[] cells = currentLine.Split('\t');

          //fromStation.Add(cells[0]);
          //toStation.Add(cells[1]);
          //miles.Add(cells[2]);
          //chains.Add(cells[3]);
          //viaRoute.Add(cells[4]);
          //mileageKey.Add(cells[5]);

          currentLine = reader.ReadLine();
        }
      }
    }
  }

}
