namespace Shap.Units.Base
{
    using System.Collections.ObjectModel;
    using NynaeveLib.ViewModel;

    public class SubClassSelectorViewModel : ViewModelBase
    {
        private ObservableCollection<string> subClasses;
        private int subClassIndex;

        public SubClassSelectorViewModel(
          ObservableCollection<string> subClasses)
        {
            this.subClasses = subClasses;
        }

        /// <summary>
        /// Gets or sets the subclasses.
        /// </summary>
        public ObservableCollection<string> SubClasses
        {
            get
            {
                return this.subClasses;
            }

            set
            {
                this.subClasses = value;
                this.OnPropertyChanged("SubClasses");
            }
        }

        /// <summary>
        /// Gets or sets the sub classes index.
        /// </summary>
        public int SubClassIndex
        {
            get
            {
                return this.subClassIndex;
            }

            set
            {
                this.subClassIndex = value;
                this.OnPropertyChanged("SubClassIndex");
            }
        }
    }
}