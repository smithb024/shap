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
        /// <summary>
        /// The family IO Controller.
        /// </summary>
        private readonly IXmlFamilyIoController ioController;

        /// <summary>
        /// The name of the family to be added.
        /// </summary>
        private string family;

        /// <summary>
        /// The deserialised family, read from the config file.
        /// </summary>
        private FamilyDetails serialisedFamilies;

        /// <summary>
        /// Initialises a new instance of the <see cref="FamilyManagerViewModel"/> class.
        /// </summary>
        /// <param name="ioController">family io controller</param>
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

        /// <summary>
        /// Gets or sets a new family name.
        /// </summary>
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

        /// <summary>
        /// Collection of all known familys.
        /// </summary>
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