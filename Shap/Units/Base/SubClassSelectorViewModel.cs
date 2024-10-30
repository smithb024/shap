namespace Shap.Units.Base
{
    using System.Collections.ObjectModel;
    using NynaeveLib.ViewModel;
    using Shap.Messages;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the selection of a sub class.
    /// </summary>
    public class SubClassSelectorViewModel : ViewModelBase
    {
        /// <summary>
        /// The collection of sub classes.
        /// </summary>
        private ObservableCollection<string> subClasses;

        /// <summary>
        /// The index of the currently selected sub class.
        /// </summary>
        private int subClassIndex;

        /// <summary>
        /// The id of the parent class. Used for logging.
        /// </summary>
        private string classId;

        /// <summary>
        /// Initialises a new instance of the <see cref="SubClassSelectorViewModel"/> class.
        /// </summary>
        /// <param name="classId">the id of the parent class.</param>
        /// <param name="subClasses">The collection of sub classes to display</param>
        public SubClassSelectorViewModel(
            string classId,
            ObservableCollection<string> subClasses)
        {
            this.subClasses = subClasses;
            this.classId = classId;
        }

        /// <summary>
        /// Gets or sets the subclasses.
        /// </summary>
        public ObservableCollection<string> SubClasses
        {
            get => this.subClasses;

            set
            {
                this.subClasses = value;
                this.OnPropertyChanged(nameof(this.SubClasses));
            }
        }

        /// <summary>
        /// Gets or sets the sub classes index.
        /// </summary>
        public int SubClassIndex
        {
            get =>  this.subClassIndex;

            set
            {
                if (this.subClassIndex == value)
                {
                    return;
                }

                this.subClassIndex = value;
                this.OnPropertyChanged(nameof(this.SubClassIndex));

                FeedbackMessage message =
                     new FeedbackMessage(
                         FeedbackType.Info,
                         $"Sub Class Selector - display sub class {this.SubClasses[this.SubClassIndex]} for class {this.classId}.");
                NynaeveMessenger.Default.Send(message);
            }
        }
    }
}