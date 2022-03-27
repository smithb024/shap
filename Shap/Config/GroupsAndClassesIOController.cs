namespace Shap.Config
{
  using System;
  using System.Collections.Generic;
  using System.IO; // stream reader/writer copy

  using NynaeveLib.Logger;

  using Shap.Common;
  using Shap.Interfaces.Io;
  using Shap.Types;

  /// <summary>
  /// Class used to read and write to the groups and classes file.
  /// </summary>
  public class GroupsAndClassesIOController : IGroupsAndClassesIOController
  {
    /// <summary>
    /// Name of the groups and classes file.
    /// </summary>
    private const string GroupsFileName = "CAG.txt";

    /// <summary>
    /// Path of the groups and classes file.
    /// </summary>
    private string filePath = string.Empty;

    /// <date>17/11/18</date>
    /// <summary>
    ///   Create a new instance of this class.
    /// </summary>
    public GroupsAndClassesIOController()
    {
      filePath = BasePathReader.GetBasePath() +
                   StaticResources.miscellaneousPath +
                   GroupsFileName;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadFile</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Load the Groups and Classes file, and create a list based on
    /// its contents. Return the list.
    /// </summary>
    /// <returns>list of groups</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public List<GroupsType> LoadFile()
    {
      List<GroupsType> groupsList = new List<GroupsType>();

      if (File.Exists(filePath))
      {
        using (StreamReader reader = new StreamReader(filePath, false))
        {
          string currentLine = string.Empty;
          currentLine = reader.ReadLine();
          while (currentLine != null)
          {
            // Create a GroupsType from each line in the file.
            GroupsType newGroup = new GroupsType();
            if (newGroup.DecodeAndAddGroup(currentLine))
            {
              groupsList.Add(newGroup);
            }
            else
            {
              // There was an error on a line in the file. Return nothing.
              groupsList.Clear();
              return groupsList;
            }

            currentLine = reader.ReadLine();
          }
        }
      }

      return groupsList;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveFile</name>
    /// <date>28/07/13</date>
    /// <summary>
    ///   Receive a list of GroupsType, convert each GroupType into a 
    /// string and save it to the Groups and Classes File.
    /// </summary>
    /// <param name="groupsList">groups list</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public void SaveFile(List<GroupsType> groupsList)
    {
      try
      {
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
          foreach (GroupsType group in groupsList)
          {
            writer.WriteLine(group.ToString());
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Instance.WriteLog(
          $"ERROR: GroupsAndClassesIOController: Failed to write {filePath}: {ex.ToString()}");
      }
    }
  }
}