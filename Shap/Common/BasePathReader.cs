namespace Shap.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;
  using System.Text;
  using System.Threading.Tasks;
  using NynaeveLib.Logger;
  using System.Windows;

  /// <summary>
  /// Read the path file to get the base path for all data. 
  /// </summary>
  public class BasePathReader
  {
    private static string basePathFileName = ".\\path.txt";

    /// <summary>
    ///   Create a new instance of this class
    /// </summary>
    public BasePathReader()
    {
    }

    /// <summary>
    ///   Reads the path file and returns the contents. The contents
    ///   are expected to be the path to the top level of data.
    /// </summary>
    /// <returns>base path of data</returns>
    public static string GetBasePath()
    {
      if (File.Exists(basePathFileName))
      {
        try
        {
          using (StreamReader reader = new StreamReader(basePathFileName))
          {
            return reader.ReadLine();
          }
        }
        catch (Exception ex)
        {
          Logger.Instance.WriteLog("Error reading path.text, base path not set: " + ex.ToString());
          return string.Empty;
        }
      }

      //Logger.Instance.WriteLog("Error, can't find path.text, base path not set.");
      return string.Empty;
//      return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\";
    }

    /// <summary>
    ///   Reads the path file and returns the contents. The contents
    ///   are expected to be the path to the top level of data.
    /// </summary>
    /// <returns>base path of data</returns>
    public static string GetBasePathUri()
    {
      if (File.Exists(basePathFileName))
      {
        try
        {
          using (StreamReader reader = new StreamReader(basePathFileName))
          {
            return reader.ReadLine();
          }
        }
        catch (Exception ex)
        {
          Logger.Instance.WriteLog("Error reading path.text, base path not set: " + ex.ToString());
          return string.Empty;
        }
      }

      //Logger.Instance.WriteLog("Error, can't find path.text, base path not set.");
//      return string.Empty;
      return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\";
    }
  }
}
