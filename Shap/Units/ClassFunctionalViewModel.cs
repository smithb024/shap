namespace Shap.Units
{
    using System.ComponentModel;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Base;
    using Interfaces.Stats;
    using Shap.Common.Commands;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Units;
    using Shap.Units.Factories;
    using NynaeveLib.Logger;
    using Types;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;
    using Shap.Messages;
    using Shap.Types.Enum;

    /// <summary>
    /// This class is used for the <see cref="ClassFrontPage"/> view. It is used to manage all 
    /// aspects of the class which is to be displayed. Effectively this means the sub classes.
    /// </summary>
    public class ClassFunctionalViewModel : SubClassSelectorViewModel
    {
        /// <summary>
        /// Manager class holding collections of the first examples.
        /// </summary>
        private readonly IFirstExampleManager firstExamples;

        /// <summary>
        /// The class data view model.
        /// </summary>
        private readonly ClassDataTypeViewModel classData;

        /// <summary>
        /// The id of the class which is represented by this object.
        /// </summary>
        private readonly string classId;

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ClassFrontPageForm</name>
        /// <date>29/12/12</date>
        /// <summary>
        ///   create a new instance of this class
        /// </summary>
        /// <param name="ioControllers">IO Controllers</param>
        /// <param name="firstExamples">First example manager</param>
        /// <param name="classId">class id</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public ClassFunctionalViewModel(
          IIoControllers ioControllers,
          IFirstExampleManager firstExamples,
          string classId)
          : base(classId, new ObservableCollection<string>())
        {
            this.classId = classId;
            this.firstExamples = firstExamples;

            this.ClassIndexes = new ObservableCollection<SubClassViewModel>();

            if (!ioControllers.UnitsXml.DoesFileExist(classId))
            {
                Logger.Instance.WriteLog($"ClassFunctionalViewModel: Aborted load: {classId} does not exist");
                this.classData = new ClassDataTypeViewModel(classId);
                return;
            }

            this.RefreshAll =
                new CommonCommand(
                    this.RefreshAllUnits,
                    () => true);

            ClassDetails classFile =
                ioControllers.UnitsXml.Read(
                    classId);
            this.classData =
                new ClassDataTypeViewModel(
                    classFile);

            if (this.classData == null)
            {
                Logger.Instance.WriteLog($"ClassFunctionalViewModel: Aborted load: {classId} does not exist");
                return;
            }

            this.classData.InitaliseSubClassIndex();

            this.SubClasses = this.classData.SubClassNumbers;
            if (this.SubClasses.Count > 0)
            {
                this.SubClassIndex = 0;
            }

            this.PropertyChanged += UpdateProperties;

            this.LoadUnits();
        }

        /// <summary>
        /// Gets or sets the collection of all sub class view models.
        /// </summary>
        public ObservableCollection<SubClassViewModel> ClassIndexes { get; set; }

        /// <summary>
        /// The currently selected sub class.
        /// </summary>
        public SubClassViewModel CurrentIndex
        {
            get
            {
                if (this.ClassIndexes == null ||
                  this.ClassIndexes.Count == 0)
                {
                    return null;
                }

                return this.ClassIndexes[this.SubClassIndex];
            }
        }

        /// <summary>
        /// Gets the class data.
        /// </summary>
        public ClassDataTypeViewModel ClassData => this.classData;

        /// <summary>
        ///   return m_classId.
        /// </summary>
        public string ClassId => this.classId;

        /// <summary>
        /// Close window command.
        /// </summary>
        public ICommand RefreshAll { get; private set; }

        /// <date>12/08/18</date>
        /// <summary>
        ///   Loads each vehicle in turn.
        /// </summary>
        private void LoadUnits()
        {
            this.ClassIndexes =
              SubClassFactory.CreateSubClasses(
                this.firstExamples,
                this.classData.SubClassList,
                this);
        }

        private void ModelChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void UpdateProperties(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SubClassIndex"))
            {
                this.OnPropertyChanged("CurrentIndex");
            }
        }

        /// <summary>
        /// Go through each unit in the currently selected sub class and refresh its data.
        /// </summary>
        private void RefreshAllUnits()
        {
            if (!this.IsSubClassValid())
            {
                return;
            }

            FeedbackMessage message =
                 new FeedbackMessage(
                     FeedbackType.Command,
                     $"ClassFrontPage - {this.classId} : Refresh all for {this.ClassId}.");
            NynaeveMessenger.Default.Send(message);

            foreach (IUnitViewModel unit in this.ClassIndexes[this.SubClassIndex].Units)
            {
                unit.RefreshUnit();
                //Searcher.RunCompleteSearch(
                //    unit.DisplayUnitNumber,
                //    unit.FormerNumbers.FormerNumbers);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the currently selected sub class is valid or not.
        /// </summary>
        /// <returns>
        /// Is the sub class valid.
        /// </returns>
        private bool IsSubClassValid()
        {
            return
                this.SubClassIndex >= 0 &&
                this.SubClassIndex < this.ClassIndexes.Count;
        }
    }
}