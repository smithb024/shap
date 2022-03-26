namespace Shap.Units
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using NynaeveLib.ViewModel;
    using Shap.Units.IO;
    using Shap.Interfaces.Config;
    using Stats;

    // TODO _ This, ClassIndexGroupViewModel and IndexItemViewModel seem to be a single 
    // view model covering the ClassIndexWindow. Can they be grouped together in a
    // refactor.
    public class ClassIndexViewModel : ViewModelBase
    {
        private const string c_titleIdentifier = "*";

        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private FirstExampleManager firstExamples;

        private bool inConfigurationMode;

        private UnitsIOController unitsIoController;
        private UnitsXmlIOController unitsXmlIoController;
        //private IndividualUnitIOController individualUnitIoController;

        /// <summary>
        /// IO controller which is used to access XML data for families.
        /// </summary>
        private IXmlFamilyIoController familyIoController;

        /// <summary>
        ///   Creates a new instance of the class index form
        /// </summary>
        /// <param name="unitsIoController">units IO Controller</param>
        /// <param name="unitsXmlIoController">units XML IO controller</param>
        /// <param name="familyIoController">Family IO controller</param>
        /// <param name="firstExamples">first examples manager</param>
        public ClassIndexViewModel(
          UnitsIOController unitsIoController,
          UnitsXmlIOController unitsXmlIoController,
          IXmlFamilyIoController familyIoController,
          FirstExampleManager firstExamples)
        {
            this.unitsIoController = unitsIoController;
            this.unitsXmlIoController = unitsXmlIoController;
            this.familyIoController = familyIoController;
            this.firstExamples = firstExamples;

            this.ItemsGroup = new ObservableCollection<ClassIndexGroupViewModel>();
            this.inConfigurationMode = false;

            this.AddControls();
        }

        /// <summary>
        /// Gets or sets a group of index items.
        /// </summary>
        public ObservableCollection<ClassIndexGroupViewModel> ItemsGroup
        {
            get;
            set;
        }

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
                this.RaisePropertyChangedEvent("InConfigurationMode");

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
        ///   Dynamically add the controls to the form based on the contents
        ///     of class list txt.
        /// </summary>
        private void AddControls()
        {
            List<string> classList = this.unitsIoController.GetClassList();

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
                        this.unitsIoController,
                        this.unitsXmlIoController,
                        this.familyIoController,
                        this.firstExamples,
                        classList[i]));
                }
            }

            if (buildGroup.Items.Count > 0)
            {
                this.ItemsGroup.Add(buildGroup);
            }
        }

        private void NewClassSelected(string className)
        {
            string name = className;
        }

    }
}