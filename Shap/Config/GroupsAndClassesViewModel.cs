namespace Shap.Config
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Input;

  using NynaeveLib.Commands;
  using NynaeveLib.DialogService.Interfaces;
  using NynaeveLib.ViewModel;

  using Shap.Common.Commands;
  using Shap.Interfaces.Config;
  using Shap.Types;

  /// <summary>
  /// View model class used to manage the groups and classes dialog.
  /// </summary>
  public class GroupsAndClassesViewModel : DialogViewModelBase
  {
    /// <summary>
    /// Contains a list of all groups being managed by this view model.
    /// </summary>
    private List<GroupsType> groupsCollection;

    /// <summary>
    /// Object used to read and write the groups and classes data.
    /// </summary>
    private IGroupsAndClassesIOController reader;

    /// <summary>
    /// Gets a collection of all groups names.
    /// </summary>
    private ObservableCollection<string> names;

    /// <summary>
    /// Gets a collection of all ranges.
    /// </summary>
    private ObservableCollection<string> ranges;

    /// <summary>
    /// The index of the currently selected group.
    /// </summary>
    private int groupIndex;

    /// <summary>
    /// The index of the currently selected range.
    /// </summary>
    private int rangeIndex;

    /// <summary>
    /// Proposed new alpha id.
    /// </summary>
    private string newAlphaId;

    /// <summary>
    /// Proposed new bottom end of a range.
    /// </summary>
    private string newRangeFrom;

    /// <summary>
    /// Proposed new top end of a range.
    /// </summary>
    private string newRangeTo;

    /// <summary>
    /// Indicates whether the next range to enter is an alpha id (as opposed to a new number
    /// range).
    /// </summary>
    private bool insertAlphaId;

    /// <summary>
    /// Proposed new group name.
    /// </summary>
    private string newGroup;

    /// <summary>
    /// Initialises a new instance of the <see cref="GroupsAndClassesViewModel"/> class.
    /// </summary>
    public GroupsAndClassesViewModel(IGroupsAndClassesIOController reader)
    {
      this.reader = reader;

      this.AddGroupCmd = new CommonCommand(this.AddGroup, this.CanAddGroup);
      this.DeleteGroupCmd = new CommonCommand(this.DeleteGroup, this.CanDeleteGroup);
      this.AddRangeCmd = new CommonCommand(this.AddRange, this.CanAddRange);
      this.DeleteRangeCmd = new CommonCommand(this.DeleteRange, this.CanDeleteRange);
      this.CompleteCmd = new CommonCommand<ICloseable>(this.SelectComplete, this.CanSelectComplete);

      this.groupsCollection = reader.LoadFile();
      this.SetupGroupsNamesCollection();

      this.rangeIndex = -1;
      this.groupIndex = -1;
    }

    /// <summary>
    /// Gets the collection containing all groups by name.
    /// </summary>
    public ObservableCollection<string> GroupNames => this.names;

    /// <summary>
    /// Gets or sets the indexs of the currently selected group.
    /// </summary>
    public int GroupIndex
    {
      get
      {
        return this.groupIndex;
      }

      set
      {
        if (value != this.groupIndex)
        {
          this.groupIndex = value;
          RaisePropertyChangedEvent(nameof(this.GroupIndex));
          this.ResetRange();
        }      }
    }

    /// <summary>
    /// Gets or sets the name of a new group to potentially be adde to the groups.
    /// </summary>
    public string NewGroup
    {
      get
      {
        return this.newGroup;
      }

      set
      {
        if (value != this.newGroup)
        {
          this.newGroup = value;
          RaisePropertyChangedEvent(nameof(this.NewGroup));
        }
      }
    }

    /// <summary>
    /// Gets the collection of all ranges for the currently selected group.
    /// </summary>
    public ObservableCollection<string> RangeCollection => this.ranges;

    /// <summary>
    /// Gets or sets the index of the currently selected range.
    /// </summary>
    public int RangeIndex
    {
      get
      {
        return this.rangeIndex;
      }

      set
      {
        this.rangeIndex = value;
        RaisePropertyChangedEvent("RangeIndex");
      }
    }

    /// <summary>
    /// Gets or sets the bottom end of a prospective range.
    /// </summary>
    public string RangeFrom
    {
      get
      {
        return this.newRangeFrom;
      }

      set
      {
        this.newRangeFrom = value;
        RaisePropertyChangedEvent(nameof(RangeFrom));
      }
    }

    /// <summary>
    /// Gets or sets the top end of a prospective range.
    /// </summary>
    public string RangeTo
    {
      get
      {
        return this.newRangeTo;
      }

      set
      {
        this.newRangeTo = value;
        RaisePropertyChangedEvent(nameof(RangeTo));
      }
    }

    /// <summary>
    /// Gets or sets a new prospective new alpha id.
    /// </summary>
    public string AlphaId
    {
      get
      {
        return this.newAlphaId;
      }

      set
      {
        this.newAlphaId= value;
        RaisePropertyChangedEvent(nameof(this.AlphaId));
      }
    }

    /// <summary>
    /// Gets a value indicating whether the next range to add is an alpha id or not.
    /// </summary>
    public bool RangeIsAlphaId
    {
      get
      {
        return this.insertAlphaId;
      }

      set
      {
        this.insertAlphaId = value;
        RaisePropertyChangedEvent(nameof(this.RangeIsAlphaId));
      }
    }

    /// <summary>
    /// Add a group item.
    /// </summary>
    public ICommand AddGroupCmd { get; private set; }

    /// <summary>
    /// Delete a group item.
    /// </summary>
    public ICommand DeleteGroupCmd { get; private set; }

    /// <summary>
    /// Add a range to a group.
    /// </summary>
    public ICommand AddRangeCmd { get; private set; }

    /// <summary>
    /// Delete a range from a group.
    /// </summary>
    public ICommand DeleteRangeCmd { get; private set; }

    /// <summary>
    /// Accept changes and save command. The command also dismisses the dialog.
    /// </summary>
    public ICommand CompleteCmd { get; private set; }

    /// <summary>
    /// Add a new group to the collection using <see cref="NewGroup"/> as the name.
    /// Auto select the new group.
    /// </summary>
    private void AddGroup()
    {
      this.groupsCollection.Add(
        new GroupsType(
          this.NewGroup));

      this.groupsCollection = this.groupsCollection.OrderBy(g => g.Name).ToList();

      for (int index = 0; index < this.groupsCollection.Count; ++index)
      {
        if (string.Equals(this.groupsCollection[index].Name, this.NewGroup))
        {
          this.GroupIndex = index;
          break;
        }
      }

      this.RefreshAll();
    }

    /// <summary>
    /// Indicates whether the <see cref="AddGroupCmd"/> command can be run. It requires a valid
    /// <see cref="NewGroup"/> value;
    /// </summary>
    /// <returns>validity flag</returns>
    private bool CanAddGroup()
    {
      return !string.IsNullOrEmpty(this.NewGroup);
    }

    /// <summary>
    /// Delete the selected group.
    /// </summary>
    private void DeleteGroup()
    {
      if (this.IsValidGroupSelected())
      {
        this.groupsCollection.RemoveAt(this.GroupIndex);
        this.ResetGroups();
      }
    }

    /// <summary>
    /// Indicates whether the <see cref="AddGroupCmd"/> command can be run. It requires a valid
    /// <see cref="NewGroup"/> value;
    /// </summary>
    /// <returns>validity flag</returns>
    private bool CanDeleteGroup()
    {
      return this.IsValidGroupSelected();
    }

    /// <summary>
    /// Add a new range to the currently selected group.
    /// </summary>
    private void AddRange()
    {
      if (this.RangeIsAlphaId)
      {
        this.groupsCollection[GroupIndex].AddAlphaID(this.AlphaId);
      }
      else
      {
        if (this.IsProposedRangeValid())
        {
          int from;
          int to;

          int.TryParse(this.newRangeFrom, out from);
          int.TryParse(this.newRangeTo, out to);

          this.groupsCollection[GroupIndex].AddRange(from, to);
        }
      }

      this.RefreshRanges();
    }

    /// <summary>
    /// Indicates whether the <see cref="AddRangeCmd"/> command can be run. It requires a valid
    /// <see cref="NewGroup"/> value or a valid range. A valid group should also be selected.
    /// </summary>
    /// <returns>validity flag</returns>
    private bool CanAddRange()
    {
      if (!this.IsValidGroupSelected())
      {
        return false;
      }

      if (this.RangeIsAlphaId)
      {
        return !string.IsNullOrEmpty(this.AlphaId);
      }
      else
      {
        return this.IsProposedRangeValid();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Assumes that the range list is a concatenation of the bounds list, then
    /// the alpha id list. Perhaps too clever.
    /// </remarks>
    private void DeleteRange()
    {
      this.groupsCollection[this.GroupIndex].Delete(this.RangeCollection[this.RangeIndex]);

      this.ResetRange();
    }

    /// <summary>
    /// Indicates whether the <see cref="AddDeleteCmd"/> command can be run. It requires a valid
    /// <see cref="NewGroup"/> value;
    /// </summary>
    /// <returns>validity flag</returns>
    private bool CanDeleteRange()
    {
      return this.IsValidGroupSelected() && this.IsValidRangeSelected();
    }

    /// <summary>
    /// Select the Ok command.
    /// </summary>
    private void SelectComplete(ICloseable window)
    {
      this.Result = MessageBoxResult.OK;

      this.reader.SaveFile(this.groupsCollection);

      window?.CloseObject();
    }

    /// <summary>
    /// Checks to see if Ok can be selected.
    /// </summary>
    /// <returns>can only select if not null or empty</returns>
    private bool CanSelectComplete(ICloseable window)
    {
      return true;
    }

    /// <Date>17/11/18</Date>
    /// <summary>
    /// Checks to see if a valid group has been selected.
    /// </summary>
    /// <returns>flag indicating whether a valid groups is selected</returns>
    private bool IsValidGroupSelected()
    {
      return 
        this.groupsCollection != null &&
        this.GroupIndex >= 0 &&
        this.GroupIndex < this.groupsCollection.Count;
    }

    /// <Date>17/11/18</Date>
    /// <summary>
    /// Checks to see if a valid group has been selected.
    /// </summary>
    /// <returns>flag indicating whether a valid groups is selected</returns>
    private bool IsValidRangeSelected()
    {
      return 
        this.ranges != null &&
        this.RangeIndex >= 0 &&
        this.RangeIndex < this.ranges.Count;
    }

    /// <summary>
    /// Checks to see if the values in the range fields are valid.
    /// </summary>
    /// <returns>validity flag</returns>
    private bool IsProposedRangeValid()
    {
      bool valid = true;
      int to;
      int from;

      if (!int.TryParse(this.newRangeFrom, out from))
      {
        return false;
      }

      if (!int.TryParse(this.newRangeTo, out to))
      {
        return false;
      }

      return to > from;
    }

    /// <summary>
    /// Determine the names of all the groups.
    /// </summary>
    private void SetupGroupsNamesCollection()
    {
      this.names = new ObservableCollection<string>();

      if (this.groupsCollection == null || this.groupsCollection.Count == 0)
      {
        this.GroupIndex = -1;
        return;
      }

      foreach (GroupsType type in this.groupsCollection)
      {
        this.names.Add(type.Name);
      }
    }

    /// <summary>
    /// Determine the ranges for the currently selected group.
    /// </summary>
    private void SetupRangesCollection()
    {
      this.ranges = new ObservableCollection<string>();

      if (!this.IsValidGroupSelected())
      {
        this.RangeIndex = -1;
        return;
      }

      foreach (GroupBoundsType bounds in this.groupsCollection[this.GroupIndex].Bounds)
      {
        this.ranges.Add(bounds.ToString());
      }

      foreach (string alphaId in this.groupsCollection[this.GroupIndex].AlphaIds)
      {
        this.ranges.Add(alphaId);
      }
    }

    /// <summary>
    /// Resets all index to none selected and refreshes all fields.
    /// </summary>
    private void ResetGroups()
    {
      this.GroupIndex = -1;
      this.RangeIndex = -1;
      this.RefreshAll();
    }

    /// <summary>
    /// Resets the range index to none selected and refreshes all range fields.
    /// </summary>
    private void ResetRange()
    {
      this.RangeIndex = -1;
      this.RefreshRanges();
    }

    /// <summary>
    /// Refreshes all fields.
    /// </summary>
    private void RefreshAll()
    {
      this.NewGroup = string.Empty;
      this.SetupGroupsNamesCollection();
      this.RaisePropertyChangedEvent(nameof(this.GroupNames));
      this.RefreshRanges();
    }

    /// <summary>
    /// Refreshes all range fields.
    /// </summary>
    private void RefreshRanges()
    {
      this.AlphaId = string.Empty;
      this.RangeFrom = string.Empty;
      this.RangeTo = string.Empty;
      this.SetupRangesCollection();
      this.RaisePropertyChangedEvent(nameof(this.RangeCollection));
    }
  }
}