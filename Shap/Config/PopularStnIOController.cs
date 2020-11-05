namespace Shap.Config
{
  using System;
  using System.Collections.Generic;
  using System.IO; // stream reader/writer copy
  using System.Runtime.CompilerServices; // synchronised.
  using NynaeveLib.Logger;

  using Shap.Common;

  public class PopularStnIOController
  {
    private const string c_popularStns = "PS.txt";
    private static PopularStnIOController m_instance = null;
    private string m_filePath = string.Empty;

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>PopularStnIOController</name>
    /// <date>28/07/13</date>
    /// <summary>
    /// Creates a new instance of this class
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    private PopularStnIOController()
    {
      m_filePath = BasePathReader.GetBasePath() +
                   StaticResources.miscellaneousPath +
                   c_popularStns;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>getInstance</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Returns an instance of this class
    /// </summary>
    /// <returns>instance of this class</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static PopularStnIOController GetInstance()
    {
      if (m_instance == null)
      {
        m_instance = new PopularStnIOController();
      }

      return m_instance;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadFile</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Load the Stn file, and create a list based on its contents.
    /// Return the list.
    /// </summary>
    /// <returns>list of locations</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public List<string> LoadFile()
    {
      List<string> locationList = new List<string>();

      if (File.Exists(m_filePath))
      {
        using (StreamReader reader = new StreamReader(m_filePath))
        {
          string currentLine = string.Empty;
          currentLine = reader.ReadLine();
          while (currentLine != null)
          {
            locationList.Add(currentLine);
            currentLine = reader.ReadLine();
          }
        }
      }

      return locationList;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveFile</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Receive a list of Stns and save it to the Stn File.
    /// </summary>
    /// <param name="locationList">location list</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public void SaveFile(List<string> locationList)
    {
      try
      {
        using (StreamWriter writer = new StreamWriter(m_filePath, false))
        {
          foreach (string location in locationList)
          {
            writer.WriteLine(location);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.WriteLog(
          $"ERROR: PopularStationsDialog: Failed to write {m_filePath}: {ex.ToString()}");
      }
    }
  }
}