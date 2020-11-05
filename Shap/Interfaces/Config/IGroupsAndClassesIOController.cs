namespace Shap.Interfaces.Config
{
  using System.Collections.Generic;
  using Shap.Types;

  /// <summary>
  /// Interface describing the class used to read and write the groups and classes file.
  /// </summary>
  public interface IGroupsAndClassesIOController
  {
    /// <date>17/11/18</date>
    /// <summary>
    ///   Load the Groups and Classes file, and create a list based on
    /// its contents. Return the list.
    /// </summary>
    /// <returns>list of groups</returns>
    List<GroupsType> LoadFile();

    /// <date>17/11/18</date>
    /// <summary>
    ///   Receive a list of GroupsType, convert each GroupType into a 
    /// string and save it to the Groups and Classes File.
    /// </summary>
    /// <param name="groupsList">groups list</param>

    void SaveFile(List<GroupsType> groupsList);
  }
}