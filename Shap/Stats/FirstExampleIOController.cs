namespace Shap.Stats
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Runtime.CompilerServices; // synchronised.
  using System.Text;
  using Shap.Common;
  using Shap.Types;
  using NynaeveLib.Logger;

  /// <summary>
  /// Deals with reading and writing to first example files.
  /// </summary>
  public class FirstExampleIOController
  {
    private const string c_fileNameNumber    = "fen.txt";
    private const string c_fileNameLocation  = "fel.txt";

    private static FirstExampleIOController m_instance    = null;

    private string basePath;

    /// <summary>
    ///   Creates a new example of this class.
    /// </summary>
    private FirstExampleIOController()
    {
      basePath = BasePathReader.GetBasePath();
    }

    /// <summary>
    ///   gets this instance of this class
    /// </summary>
    /// <returns>this class</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static FirstExampleIOController GetInstance()
    {
      if (m_instance == null)
      {
        m_instance = new FirstExampleIOController();
      }

      return m_instance;
    }

    /// <summary>
    ///   Gets list of a complete picture of first numbers.
    /// </summary>
    /// <returns>list of first examples</returns>
    public List<FirstExampleType> GetFirstExampleListNumber()
    {
      return ReadFile(basePath + 
                      StaticResources.baPath + 
                      c_fileNameNumber);
    }

    /// <summary>
    ///   Gets list of a complete picture of first locations.
    /// </summary>
    /// <returns>list of first examples</returns>
    public List<FirstExampleType> GetFirstExampleListLocation()
    {
      return ReadFile(basePath + 
                      StaticResources.baPath + 
                      c_fileNameLocation);
    }

    /// <summary>
    /// Gets list of first numbers for the year indicated by the
    ///   argument.
    /// </summary>
    /// <param name="year">the year</param>
    /// <returns>list of first examples</returns>
    public List<FirstExampleType> GetFirstExampleListNumber(string year)
    {
      return ReadFile(basePath + 
                      StaticResources.baPath + 
                      year + 
                      "\\" +
                      c_fileNameNumber);
    }

    /// <summary>
    /// Gets list of first locations for the year indicated by the
    ///   argument.
    /// </summary>
    /// <param name="year">the year</param>
    /// <returns>list of first examples</returns>
    public List<FirstExampleType> GetFirstExampleListLocation(string year)
    {
      return ReadFile(basePath + 
                      StaticResources.baPath + 
                      year + 
                      "\\" +
                      c_fileNameLocation);
    }

    /// <summary>
    /// Sets up path then calls append file. Used for complete number 
    ///   data.
    /// </summary>
    /// <param name="firstExamples">First Example Type</param>
    /// <returns>success flag</returns>
    public bool AppendFileNumber(FirstExampleType firstExamples)
    {
      return AppendFile(basePath + 
                        StaticResources.baPath +
                        c_fileNameNumber,
                        firstExamples);
    }

    /// <summary>
    /// Sets up path then calls append file. Used for annual number 
    ///   data, as specified by the year argument.
    /// </summary>
    /// <param name="firstExamples">First Example Type</param>
    /// <param name="year">the year</param>
    /// <returns>success flag</returns>
    public bool AppendFileNumber(FirstExampleType firstExamples,
                                 string           year)
    {
      return AppendFile(basePath + 
                        StaticResources.baPath +
                        year +
                        "\\" +
                        c_fileNameNumber,
                        firstExamples);
    }

    /// <summary>
    /// Sets up path then calls append file. Used for complete location 
    ///   data.
    /// </summary>
    /// <param name="firstExamples">First Example Type</param>
    /// <returns>success flag</returns>
    public bool AppendFileLocation(FirstExampleType firstExamples)
    {
      return AppendFile(basePath + 
                        StaticResources.baPath +
                        c_fileNameLocation,
                        firstExamples);
    }

    /// <summary>
    /// Sets up path then calls append file. Used for annual location 
    ///   data, as specified by the year argument.
    /// </summary>
    /// <param name="firstExamples">First Example Type</param>
    /// <param name="year">the year</param>
    /// <returns>success flag</returns>
    public bool AppendFileLocation(FirstExampleType firstExamples,
                                   string           year)
    {
      return AppendFile(basePath + 
                        StaticResources.baPath +
                        year +
                        "\\" +
                        c_fileNameLocation,
                        firstExamples);
    }

    /// <summary>
    /// Sets up path then calls write file. Used for complete number 
    ///   data.
    /// </summary>
    /// <param name="firstExamples">List of First Examples</param>
    /// <returns>success flag</returns>
    public bool WriteFileNumber(List<FirstExampleType> firstExamples)
    {
      return WriteFile(basePath + 
                       StaticResources.baPath +
                       c_fileNameNumber,
                       firstExamples);
    }

    /// <summary>
    /// Sets up path then calls write file. Used for annual number 
    ///   data, as specified by the year argument.
    /// </summary>
    /// <param name="firstExamples">List of First Examples</param>
    /// <param name="year">the year</param>
    /// <returns>success flag</returns>
    public bool WriteFileNumber(List<FirstExampleType> firstExamples,
                                string year)
    {
      return WriteFile(basePath + 
                       StaticResources.baPath +
                       year +
                       "\\" +
                       c_fileNameNumber,
                       firstExamples);
    }

    /// <summary>
    /// Sets up path then calls write file. Used for complete location 
    ///   data.
    /// </summary>
    /// <param name="firstExamples">List of First Examples</param>
    /// <returns>success flag</returns>
    public bool WriteFileLocation(List<FirstExampleType> firstExamples)
    {
      return WriteFile(basePath + 
                       StaticResources.baPath +
                       c_fileNameLocation,
                       firstExamples);
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>writeFileLocation</name>
    /// <date>13/01/13</date>
    /// <summary>
    /// Sets up path then calls write file. Used for annual number 
    ///   data, as specified by the year argument.
    /// </summary>
    /// <param name="firstExamples">List of First Examples</param>
    /// <param name="year">the year</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool WriteFileLocation(List<FirstExampleType> firstExamples,
                                  string                 year)
    {
      return WriteFile(basePath + 
                       StaticResources.baPath +
                       year +
                       "\\" +
                       c_fileNameLocation,
                       firstExamples);
    }

    /// <summary>
    ///   Read file
    /// </summary>
    /// <param name="path">file path</param>
    /// <returns>List of first examples</returns>
    private List<FirstExampleType> ReadFile(string path)
    {
      List<FirstExampleType> fileContents = new List<FirstExampleType>();
      if (File.Exists(path))
      {
        using (StreamReader reader = new StreamReader(path))
        {
          string currentLine = string.Empty;
          currentLine = reader.ReadLine();

          while (currentLine != null)
          {
            FirstExampleType firstExample = new FirstExampleType();
            if (firstExample.Set(currentLine))
            {
              fileContents.Add(firstExample);
            }
            else
            {
              Logger.Instance.WriteLog("ERROR: FirstExampleIOController: error in with line - " + currentLine);
            }

            currentLine  = reader.ReadLine();
            firstExample = null;
          }
        }
      }

      return fileContents;
    }

    /// <summary>
    ///   read the file
    /// </summary>
    /// <param name="path">file path</param>
    /// <param name="firstExamples">First Example Type</param>
    /// <returns>success flag</returns>
    private bool AppendFile(string           path,
                            FirstExampleType firstExamples)
    {
      if (!File.Exists(path))
      {
        File.Create(path).Dispose();
      }

      try
      {
        using (StreamWriter writer = new StreamWriter(path, true))
        {
          writer.WriteLine(firstExamples.ToString());
          return true;
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.WriteLog("ERROR: FirstExampleIOController: Failed to write (append) " 
                + path 
                + ": " 
                + ex.ToString());
        return false;
      }
    }

    /// <summary>
    ///   write to file
    /// </summary>
    /// <param name="path">path string</param>
    /// <param name="firstExamples">List of First Examples</param>
    /// <returns>success flag</returns>
    private bool WriteFile(string                 path,
                           List<FirstExampleType> firstExamples)
    {
      try
      {
        using (StreamWriter writer = new StreamWriter(path, false))
        {
          foreach (FirstExampleType firstExample in firstExamples)
          {
            writer.WriteLine(firstExample.ToString());
          }
        }

        return true;
      }
      catch (Exception ex)
      {
        Logger.Instance.WriteLog("ERROR: FirstExampleIOController: Failed to write " 
                + path 
                + ": " 
                + ex.ToString());
        return false;
      }
    }
  }
}
