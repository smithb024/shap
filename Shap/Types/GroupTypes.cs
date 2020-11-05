namespace Shap.Types
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;
  using NynaeveLib.Logger;
  using NynaeveLib.ViewModel;

  // TODO Are the get methods obsolete?
  public class GroupsType : ViewModelBase
  {
    private ObservableCollection<GroupBoundsType> bounds = new ObservableCollection<GroupBoundsType>();
    //private List<int>    m_lowerBound = new List<int>();
    //private List<int>    m_upperBound = new List<int>();
    private ObservableCollection<string> alphaIDs    = new ObservableCollection<string>();
    private string       m_groupName  = string.Empty;

    private char c_sectionSeparator = ':';
    private char c_itemSeparator    = ',';
    private char c_rangeCharacter   = '-';

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>GroupsType</name>
    /// <date>28/07/13</date>
    /// <summary>
    /// Creates a new instance of the GroupsType class.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public GroupsType() 
    {
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>GroupsType</name>
    /// <date>28/07/13</date>
    /// <summary>
    /// Creates a new instance of the GroupsType class.
    /// </summary>
    /// <param name="name">group name</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public GroupsType(string name)
    {
      m_groupName = name;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Gets the group name.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public string Name
    {
      get { return m_groupName; }
    }

    public ObservableCollection<GroupBoundsType> Bounds
    {
      get
      {
        return this.bounds;
      }

      set
      {
        this.bounds = value;
        this.RaisePropertyChangedEvent("Bounds");
      }
    }

    public ObservableCollection<string> AlphaIds
    {
      get
      {
        return this.alphaIDs;
      }

      set
      {
        this.alphaIDs = value;
        this.RaisePropertyChangedEvent("AlphaIds");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ToString</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   convert class to single string in the format:
    /// {Class Name}:{Number Ranges, comma separated}:{alpha character
    /// prefixes, comma separated}
    /// </summary>
    /// <returns>output string</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public override string ToString()
    {
      string outputString = Name;
      outputString = outputString + c_sectionSeparator;
      for (int i = 0; i < bounds.Count(); ++i)
      {
        if (i > 0)
        {
          outputString = outputString + c_itemSeparator;
        }

        outputString = outputString + bounds[i].LowerBound + c_rangeCharacter + bounds[i].UpperBound;
      }

      outputString = outputString + c_sectionSeparator;
      for (int i = 0; i < alphaIDs.Count(); ++i)
      {
        if (i > 0)
        {
          outputString = outputString + c_itemSeparator;
        }

        outputString = outputString + alphaIDs[i];
      }

      return outputString;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>decodeAndAddGroup</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Takes the group data in its raw form:
    /// {Class Name}:{Number Ranges, comma separated}:{alpha character
    /// prefixes, comma separated}
    ///   it splits up the raw data, first by ':' then by ','.
    /// </summary>
    /// <param name="rawData">raw data</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool DecodeAndAddGroup(string rawData)
    {
      Logger logger = Logger.Instance;

      // Split coarsely.
      string[] itemsArray = rawData.Split(c_sectionSeparator);
      if (itemsArray.Count() != 3)
      {
        logger.WriteLog("ERROR: Groups Type invalid input: " + rawData);
        return false;
      }

      // TODO This method is really smelly.

      m_groupName = itemsArray[0];

      // split finely and set the range boundaries.
      if (itemsArray[1] != string.Empty)
      {
        string[] rangesArray = itemsArray[1].Split(c_itemSeparator);
        foreach (string range in rangesArray)
        {
          string[] singleRangeArray = range.Split(c_rangeCharacter);
          if (singleRangeArray.Count() == 2)
          {
            int tempLowerBound;
            int tempUpperBound;

            if (int.TryParse(singleRangeArray[0], out tempLowerBound))
            {
              if (int.TryParse(singleRangeArray[1], out tempUpperBound))
              {
                this.Bounds.Add(
                  new GroupBoundsType(
                    tempLowerBound,
                    tempUpperBound));
              }
              else
              {
                logger.WriteLog("ERROR: Groups Type invalid upper range input: " + rawData);
              }
            }
            else
            {
              logger.WriteLog("ERROR: Groups Type invalid lower range input: " + rawData);
            }
          }
          else
          {
            logger.WriteLog("ERROR: Groups Type invalid range input: " + rawData);
          }
        }
      }

      // split finely and set the Alpha prefixes.
      string[] alphaIDArray = itemsArray[2].Split(c_itemSeparator);
      foreach (string alphaID in alphaIDArray)
      {
        this.alphaIDs.Add(alphaID);
      }

      return true;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>addRange</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Add range bounds.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public void AddRange(int lowerBound, int upperBound)
    {
      bounds.Add(
        new GroupBoundsType(
          lowerBound,
          upperBound));
      //m_lowerBound.Add(lowerBound);
      //m_upperBound.Add(upperBound);
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>addAlphaID</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Add alpha prefixes.
    /// </summary>
    /// <param name="alphaID">alpha identifier</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public void AddAlphaID(string alphaID)
    {
      this.alphaIDs.Add(alphaID);
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>rangeArraySize</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Get range array size
    /// </summary>
    /// <returns>lower bound size</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public int RangeArraySize()
    {
      return this.bounds.Count();
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>alphaIDArraySize</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Get alpha identifier size
    /// </summary>
    /// <returns>alpha identifier size</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public int AlphaIDArraySize()
    {
      return alphaIDs.Count();
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>getLowerBound</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   get the lower bound corresponding to the index.
    /// </summary>
    /// <param name="index">bound index</param>
    /// <returns>lower bound</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public int GetLowerBound(int index)
    {
      if (index < this.Bounds.Count())
      {
        return this.Bounds[index].LowerBound;
      }
      else
      {
        return 0;
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>getUpperBound</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   get the upper bound corresponding to the index.
    /// </summary>
    /// <param name="index">bound index</param>
    /// <returns>upper bound</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public int GetUpperBound(int index)
    {
      if (index < this.Bounds.Count())
      {
        return this.Bounds[index].UpperBound;
      }
      else
      {
        return 0;
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>getAlphaID</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   get the alpha id corresponding to the index.
    /// </summary>
    /// <param name="index">alpha index</param>
    /// <returns>alpha identifier</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public string GetAlphaID(int index)
    {
      if (index < alphaIDs.Count())
      {
        return alphaIDs[index];
      }
      else
      {
        return string.Empty;
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>isValid</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Check to see if the range is valid and doesn't contain any 
    /// numbers which already exist.
    /// </summary>
    /// <param name="lowerBound">lower bound</param>
    /// <param name="upperBound">upper bound</param>
    /// <returns>valid flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool IsValid(int lowerBound,
                        int upperBound)
    {
      return lowerBound < upperBound;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>isValid</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Check to see if the alphaID doesn't already exist.
    /// </summary>
    /// <param name="alphaID">alpha identifier</param>
    /// <returns name="valid">valid flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool IsValid(string alphaID)
    {
      foreach (string id in this.alphaIDs)
      {
        if (alphaID == id)
        {
          return false;
        }
      }

      return true;
    }

    /// <summary>
    /// Remove the argument value.
    /// </summary>
    /// <param name="boundToRemove">value to remove</param>
    public void DeleteBound(GroupBoundsType boundToRemove)
    {
      this.Bounds.Remove(boundToRemove);
      this.RaisePropertyChangedEvent("Bounds");
    }

    /// <date>28/07/13</date>
    /// <summary>
    ///   Attempt to remove the alpha id data.
    /// </summary>
    /// <param name="alphaID">alpha identifier</param>
    /// <returns>success flag</returns>
    public void DeleteAlphaID(string alphaID)
    {
      this.alphaIDs.Remove(alphaID);
      this.RaisePropertyChangedEvent("AlphaIds");
    }

    /// <summary>
    /// Remove any alpha id or range which matches <paramref name="range"/>.
    /// </summary>
    /// <param name="range">value to remove.</param>
    public void Delete(string range)
    {
      this.AlphaIds.Remove(range);

      int index = this.Bounds.Count;
      while (--index > 0)
      {
        GroupBoundsType element = this.Bounds.ElementAt(index);
        if (string.Compare(element.ToString(), range) == 0)
        {
          this.DeleteBound(element);
        }
      }
    }
  }
}
