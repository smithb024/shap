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
        }

        /// <summary>
        /// Gets a class index item.
        /// </summary>
        public ObservableCollection<IndexItemViewModel> Items
        {
            get => this.items;

            private set
            {
                this.items = value;
                this.OnPropertyChanged(nameof(this.Items));
            }
        }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string GroupName { get; private set; }

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

        /// <summary>
        /// The family which is currently being filtered on. Inform the Index Items.
        /// </summary>
        /// <param name="familyFilter">family being filtered on</param>
        public void SetFamilyFilter(string familyFilter)
        {
            foreach (IndexItemViewModel indexViewModel in this.Items)
            {
                indexViewModel.SetFamilyFilter(familyFilter);
            }
        }

        /// <summary>
        /// The operator which is currently being filtered on. Inform the Index Items.
        /// </summary>
        /// <param name="operatorFilter">operator being filtered on</param>
        public void SetOperatorFilter(string operatorFilter)
        {
            foreach (IndexItemViewModel indexViewModel in this.Items)
            {
                indexViewModel.SetOperatorFilter(operatorFilter);
            }
        }
    }
}