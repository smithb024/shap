namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Config;

    /// <summary>
    /// View model which manages the ability to add families
    /// </summary>
    public class FamilyManagerViewModel : ViewModelBase
    {
        private string family;

        private FamilyDetails serialisedFamilies;

        private IXmlFamilyIoController ioController;

        public FamilyManagerViewModel(
            IXmlFamilyIoController ioController)
        {
            this.ioController = ioController;
            this.serialisedFamilies = ioController.Read();
            this.Families = new List<string>();

            foreach (SingleFamily singleFamily in this.serialisedFamilies.Families)
            {
                this.Families.Add(singleFamily.Name);
            }

            this.AddFile = new CommonCommand(this.Add);
        }

        public string Family
        {
            get
            {
                return this.family;
            }

            set
            {
                if (this.family == value)
                {
                    return;
                }

                this.family = value;
                this.RaisePropertyChangedEvent(nameof(this.Family));
            }
        }

        public List<string> Families { get; }

        /// <summary>
        /// Add an item.
        /// </summary>
        public ICommand AddFile{ get; private set; }

        /// <summary>
        /// Save the updates.
        /// </summary>
        public void Save()
        {
            this.ioController.Write(this.serialisedFamilies);
        }

        /// <summary>
        /// Add the string as a new family. Reset the input.
        /// </summary>
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(this.Family))
            {
                return;
            }

            SingleFamily newSingleFamily =
                new SingleFamily()
                {
                    Name = this.Family
                };

            this.serialisedFamilies.Families.Add(newSingleFamily);

            this.Families.Add(this.Family);
            this.RaisePropertyChangedEvent(nameof(this.Families));

            this.Family = string.Empty;
        }

    }
}