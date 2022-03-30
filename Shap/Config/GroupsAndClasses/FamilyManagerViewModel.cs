namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Io;

    /// <summary>
    /// View model which manages the ability to add families
    /// </summary>
    public class FamilyManagerViewModel : ViewModelBase, IViewModelBase
    {
        /// <summary>
        /// IO Controllers.
        /// </summary>
        private readonly IIoControllers ioControllers;

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
        /// <param name="ioControllers">IO Controllers</param>
        public FamilyManagerViewModel(
            IIoControllers ioControllers)
        {
            this.ioControllers = ioControllers;
            this.serialisedFamilies = this.ioControllers.Family.Read();
            this.Families = new ObservableCollection<string>();

            if (this.serialisedFamilies != null)
            {
                foreach (SingleFamily singleFamily in this.serialisedFamilies.Families)
                {
                    this.Families.Add(singleFamily.Name);
                }
            }
            else
            {
                this.serialisedFamilies = new FamilyDetails();
                this.serialisedFamilies.Families = new List<SingleFamily>();
            }

            this.AddFamily = new CommonCommand(this.Add, this.CanAdd);
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
        /// Collection of all known families.
        /// </summary>
        public ObservableCollection<string> Families { get; }

        /// <summary>
        /// Add an item.
        /// </summary>
        public ICommand AddFamily{ get; private set; }

        /// <summary>
        /// Save the updates.
        /// </summary>
        public void Save()
        {
            this.ioControllers.Family.Write(this.serialisedFamilies);
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

        /// <summary>
        /// Indicates whether the <see cref="AddFamily"/> command can be run. It requires a valid
        /// <see cref="Family"/> value;
        /// </summary>
        /// <returns>validity flag</returns>
        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(this.Family);
        }
    }
}