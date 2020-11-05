using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Units
{
  using System.Collections.ObjectModel;
  using NynaeveLib.ViewModel;

  /// <summary>
  /// 
  /// </summary>
  public class ClassIndexGroupViewModel : ViewModelBase
  {
    private ObservableCollection<IndexItemViewModel> items;

    /// <summary>
    /// Initialise a new instance of the <see cref="ClassIndexGroupViewModel"/> class.
    /// </summary>
    public ClassIndexGroupViewModel(string groupName)
    {
      this.GroupName = groupName;
      this.Items = new ObservableCollection<IndexItemViewModel>();

      //this.items = new ObservableCollection<IndexItemViewModel>()
      //{
      //  new IndexItemViewModel(groupName + "-Test1"),
      //  new IndexItemViewModel(groupName + "-Test2"),
      //  new IndexItemViewModel(groupName + "-Test3")
      //};
    }

    /// <summary>
    /// Gets a class index item.
    /// </summary>
    public ObservableCollection<IndexItemViewModel> Items
    {
      get
      {
        return this.items;
      }

      private set
      {
        this.items = value;
        this.RaisePropertyChangedEvent("Items");
      }
    }

    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    public string GroupName
    {
      get;
      private set;
    }

    /// <summary>
    /// Add a new item to the list.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddNewItem(IndexItemViewModel newItem)
    {
      this.Items.Add(newItem);
    }

    /// <summary>
    /// Sets the configuration mode.
    /// </summary>
    /// <param name="inConfigurationMode"></param>
    public void SetConfigurationMode(bool inConfigurationMode)
    {
      if (this.Items != null)
      {
        foreach (IndexItemViewModel item in this.Items)
        {
          item.InConfigurationMode = inConfigurationMode;
        }
      }
    }
  }
}
