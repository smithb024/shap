namespace Shap.Analysis
{
  using System;
  using System.Collections.Generic;
  using System.IO;

  using Common;
  using Interfaces;

  using NynaeveLib.Logger;

  /// <summary>
  /// Report builder, manages a collection of counters.
  /// </summary>
  public class ReportCounterManager<T> where T : ICsvOut
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="ReportCounter"/> class.
    /// </summary>
    public ReportCounterManager()
    {
      this.CounterCollection = new List<T>();
    }

    /// <summary>
    /// Gets a collection of counter objects
    /// </summary>
    public List<T> CounterCollection { get; private set; }

    /// <summary>
    /// Add a new year counter to the collection.
    /// </summary>
    /// <param name="clsId">class of the new counter object</param>
    public void AddNewCounter(
      T newCounter)
    {
      this.CounterCollection.Add(newCounter);
    }

    /// <summary>
    ///  Add one to the count for <paramref name="classId"/> in the month <paramref name="month"/>
    /// </summary>
    /// <param name="classId">id of the interested class</param>
    /// <param name="month">month to add the count for</param>
    public void AddOne(
      string classId,
      int month)
    {
      foreach (T cls in this.CounterCollection)
      {
        if (cls.Id == classId)
        {
          cls.AddOne(month);
          return;
        }
      }
    }

    /// <summary>
    ///  Add one to the count for <paramref name="classId"/> in the month <paramref name="month"/>
    /// </summary>
    /// <param name="classId">id of the interested class</param>
    public void AddOne(
      string classId)
    {
      foreach (T cls in this.CounterCollection)
      {
        if (cls.Id == classId)
        {
          cls.AddOne();
          return;
        }
      }
    }

    /// <summary>
    /// Add one to the count for to and from.
    /// </summary>
    /// <param name="to">to id</param>
    /// <param name="from">from id</param>
    public void AddOne(
      string to,
      string from)
    {
      foreach (T cls in this.CounterCollection)
      {
        cls.AddTo(to);
        cls.AddFrom(from);
      }
    }

    /// <summary>
    ///  Add one to the count for <paramref name="from"/>/>
    /// </summary>
    /// <param name="from">id of the interested class</param>
    public void AddFrom(
      string from)
    {
      foreach (T cls in this.CounterCollection)
      {
        if (cls.Id == from)
        {
          cls.AddFrom(from);
          return;
        }
      }
    }

    /// <summary>
    ///  Add one to the count for <paramref name="to"/>/>
    /// </summary>
    /// <param name="to">id of the interested class</param>
    public void AddTo(
      string to)
    {
      foreach (T cls in this.CounterCollection)
      {
        if (cls.Id == to)
        {
          cls.AddTo(to);
          return;
        }
      }
    }

    /// <summary>
    /// Removes any cls from the arrays which contain no data.
    /// </summary>
    public void RemoveEmptyClasses()
    {
      for (int index = this.CounterCollection.Count - 1; index >= 0; --index)
      {
        if (this.CounterCollection[index].Total == 0)
        {
          this.CounterCollection.RemoveAt(index);
        }
      }
    }

    /// <summary>
    ///   Creates a file in the location indicated by the path and 
    ///     filename. It creates the dir as well if necessary.
    ///   The column lists should be the same size, the method writes 
    ///     comma separated lines based on the columns.
    /// </summary>
    /// <param name="path">file path</param>
    /// <param name="fileName">file name</param>
    /// <param name="lines">contents to write to the file</param>
    /// <returns name="success">success flag</returns>
    public void WriteCSVFile(
      string       fileName,
      string faultMessage)
    {
      string path = BasePathReader.GetBasePath() + StaticResources.reportPath;

      // create directory if it doesn't exist
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      try
      {
        using (StreamWriter sw = new StreamWriter(path + fileName))
        {
          foreach (ICsvOut line in this.CounterCollection)
          {
            sw.WriteLine(line.CsvOut);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.WriteLog("ReportFactoryCommon: Report: " + ex.ToString());
        Logger.Instance.WriteLog(faultMessage);
      }
    }
  }
}