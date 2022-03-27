namespace Shap.Units
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Io;
    using Stats;

    /// <summary>
    /// View model for the class index view. The index items are split into groups and this view
    /// model manages the groups.
    /// </summary>
    public class ClassIndexViewModel : ViewModelBase
    {
        /// <summary>
        /// Used to identify a new group.
        /// </summary>
        private const string c_titleIdentifier = "*";

        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private readonly FirstExampleManager firstExamples;

        /// <summary>
        /// IO controllers.
        /// </summary>
        private readonly IIoControllers ioControllers;

        /// <summary>
        /// Indicates whether configuration mode has been selected.
        /// </summary>
        private bool inConfigurationMode;

        /// <summary>
        /// The currently selected family index.
        /// </summary>
        private int familyIndex;

        /// <summary>
        ///   Creates a new instance of the class index form
        /// </summary>
        /// <param name="iocontrollers">IO controllers</param>
        /// <param name="firstExamples">first examples manager</param>
        public ClassIndexViewModel(
          IIoControllers ioControllers,
          FirstExampleManager firstExamples)
        {
            this.ioControllers = ioControllers;
            this.firstExamples = firstExamples;

            this.ItemsGroup = new ObservableCollection<ClassIndexGroupViewModel>();
            this.inConfigurationMode = false;

            FamilyDetails serialisedFamilies = ioControllers.Family.Read();
            this.Families =
                new ObservableCollection<string>
            {
                string.Empty
            };

            if (serialisedFamilies != null)
            {
                foreach (SingleFamily singleFamily in serialisedFamilies.Families)
                {
                    this.Families.Add(singleFamily.Name);
                }
            }

            this.AddControls();
        }

        /// <summary>
        /// Gets or sets a group of index items.
        /// </summary>
        public ObservableCollection<ClassIndexGroupViewModel> ItemsGroup { get; set; }

        /// <summary>
        /// Gets or sets the currently selected family from the filter list.
        /// </summary>
        public int FamilyIndex
        {
            get
            {
                return this.familyIndex;
            }

            set
            {
                if (this.familyIndex == value)
                {
                    return;
                }

                this.familyIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.FamilyIndex));
                this.SetFamilyFilter();
            }
        }

        /// <summary>
        /// Gets the list of families which can be used as a filter.
        /// </summary>
        public ObservableCollection<string> Families { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the class is in configuration mode or not.
        /// </summary>
        public bool InConfigurationMode
        {
            get
            {
                return this.inConfigurationMode;
            }

            set
            {
                this.inConfigurationMode = value;
                this.RaisePropertyChangedEvent(nameof(this.InConfigurationMode));

                if (this.ItemsGroup != null)
                {
                    foreach (ClassIndexGroupViewModel group in this.ItemsGroup)
                    {
                        group.SetConfigurationMode(value);
                    }
                }
            }
        }

        /// <summary>
        /// The family which is currently being filtered on. Inform the groups
        /// </summary>
        private void SetFamilyFilter()
        {
            if (this.FamilyIndex >= 0 && this.FamilyIndex < this.Families.Count)
            {
                foreach (ClassIndexGroupViewModel indexViewModel in this.ItemsGroup)
                {
                    indexViewModel.SetFamilyFilter(
                        this.Families[this.FamilyIndex]);
                }
            }
        }

        /// <summary>
        ///   Dynamically add the controls to the form based on the contents
        ///     of class list txt.
        /// </summary>
        private void AddControls()
        {
            List<string> classList = 
                this.ioControllers.Units.GetClassList();

            ClassIndexGroupViewModel buildGroup = new ClassIndexGroupViewModel("NameNotSet");

            for (int i = 0; i < classList.Count; ++i)
            {
                // if the line begins with a "*" then it's a title, not an icon.
                if (classList[i].Substring(0, 1) == c_titleIdentifier)
                {
                    this.ItemsGroup.Add(buildGroup);
                    buildGroup = new ClassIndexGroupViewModel("NameNotSet");
                }
                else
                {
                    buildGroup.AddNewItem(
                      new IndexItemViewModel(
                        this.ioControllers,
                        this.firstExamples,
                        classList[i]));
                }
            }

            if (buildGroup.Items.Count > 0)
            {
                this.ItemsGroup.Add(buildGroup);
            }
        }
    }
}