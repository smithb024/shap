namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using NynaeveLib.Commands;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Io;
    using Shap.Types;

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
        /// Gets the index of the currently selected family.
        /// </summary>
        private int familyIndex;

        /// <summary>
        /// Gets the index of the currently selected group.
        /// </summary>
        private int groupsIndex;

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
            this.MemberGroups = new ObservableCollection<string>();

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

            List<GroupsType> groupsCollection = ioControllers.Gac.LoadFile();
            this.Groups =
                new ObservableCollection<string>()
                {
                    string.Empty
                };
            this.groupsIndex = 0;

            if (groupsCollection != null)
            {
                foreach (GroupsType groupType in groupsCollection)
                {
                    this.Groups.Add(groupType.Name);
                }
            }

            this.ResetMembers();

            this.AddFamily = new CommonCommand(this.Add, this.CanAdd);
            this.AddMember = new CommonCommand(this.AddMemberToFamily, () => true);
            this.Messenger.Register<GroupsListMessage>(
                this,
                (r, message) => this.NewGroupsList(message));
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
                this.OnPropertyChanged(nameof(this.Family));
            }
        }

        /// <summary>
        /// Collection of all known families.
        /// </summary>
        public ObservableCollection<string> Families { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected family.
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
                this.OnPropertyChanged(nameof(this.familyIndex));
                this.ResetMembers();
            }
        }

        /// <summary>
        /// Gets the names of all the available groups/classes.
        /// </summary>
        public ObservableCollection<string> Groups { get; private set; }

        /// <summary>
        /// Gets all the groups which are a member of the current family.
        /// </summary>
        public ObservableCollection<string> MemberGroups { get; private set; }

        /// <summary>
        /// Gets the index of the currently selected group/class.
        /// </summary>
        public int GroupsIndex
        {
            get => this.groupsIndex;
            set => this.SetProperty(ref this.groupsIndex, value);
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        public ICommand AddFamily { get; private set; }

        /// <summary>
        /// Add a member to the family.
        /// </summary>
        public ICommand AddMember { get; private set; }

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
            this.OnPropertyChanged(nameof(this.Families));

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

        /// <summary>
        /// Add the selected group to the member groups property.
        /// </summary>
        public void AddMemberToFamily()
        {
            if (this.MemberGroups.Contains(this.Groups[this.GroupsIndex]))
            {
                return;
            }

            this.MemberGroups.Add(this.Groups[this.GroupsIndex]);
            this.MemberGroups = new ObservableCollection<string>(this.MemberGroups.OrderBy(i => i));
            this.OnPropertyChanged(nameof(this.MemberGroups));

            SingleClass newClass =
                new SingleClass() {
                    Name = this.Groups[this.GroupsIndex]
                };
            this.serialisedFamilies.Families[this.FamilyIndex].Classes.Add(newClass);
        }

        /// <summary>
        /// A new Groups List message have been received via the messenger.
        /// </summary>
        /// <param name="message">The message</param>
        private void NewGroupsList(GroupsListMessage message)
        {
            this.GroupsIndex = 0;
            this.Groups =
                new ObservableCollection<string>
                {
                    string.Empty
                };

            foreach (string name in message.Groups)
            {
                this.Groups.Add(name);
            }

            this.OnPropertyChanged(nameof(this.Groups));
        }

        /// <summary>
        /// Set up the Group Members list to contain all members for the currently selected group/
        /// class.
        /// </summary>
        private void ResetMembers()
        {
            this.MemberGroups.Clear();

            foreach(SingleClass familyClass in this.serialisedFamilies.Families[this.FamilyIndex].Classes)
            {
                this.MemberGroups.Add(familyClass.Name);
            }
        }
    }
}