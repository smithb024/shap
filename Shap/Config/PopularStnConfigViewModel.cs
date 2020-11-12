namespace Shap.Config
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using NynaeveLib.ViewModel;

    using Shap.StationDetails;

    // TODO add an IResult interface, to ensure that a result is set in this dialog view model

    /// <summary>
    /// View model which supports the popular stn view.
    /// </summary>
    public class PopularStnConfigViewModel : DialogViewModelBase
    {
        /// <summary>
        /// collection of locations.
        /// </summary>
        ObservableCollection<string> stnCollection;

        /// <summary>
        /// index of the currently selected location.
        /// </summary>
        int stnIndex;

        /// <summary>
        /// collection of popuular locations.
        /// </summary>
        ObservableCollection<string> popularStnCollection;

        /// <summary>
        /// index of the currently selected popular location.
        /// </summary>
        int popularStnIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="PopularStnConfigViewModel"/> class.
        /// </summary>
        public PopularStnConfigViewModel()
        {
            this.StnCollection = new ObservableCollection<string>();
            this.PopularStnCollection = new ObservableCollection<string>();

            string previousvalue = string.Empty;
            string location = string.Empty;

            JourneyIOController journeyController = JourneyIOController.GetInstance();

            for (int i = 0; i < journeyController.GetMileageDetailsLength(); i++)
            {
                location = journeyController.GetFromStation(i);
                if (location != previousvalue)
                {
                    this.StnCollection.Add(location);
                }

                previousvalue = location;
            }

            PopularStnIOController locationController = PopularStnIOController.GetInstance();

            foreach (string popular in locationController.LoadFile())
            {
                this.PopularStnCollection.Add(popular);
            }

            this.AddCmd = new CommonCommand(this.AddStn, this.CanAddStn);
            this.DeleteCmd = new CommonCommand(this.DeleteStn, this.CanDeleteStn);
            this.CompleteCmd = new CommonCommand<ICloseable>(this.SelectComplete, this.CanSelectComplete);
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        public ICommand AddCmd { get; private set; }

        /// <summary>
        /// Delete the selected item.
        /// </summary>
        public ICommand DeleteCmd { get; private set; }

        /// <summary>
        /// Ok command.
        /// </summary>
        public ICommand CompleteCmd { get; private set; }

        /// <summary>
        /// Gets or sets the location collection.
        /// </summary>
        public ObservableCollection<string> StnCollection
        {
            get
            {
                return this.stnCollection;
            }

            set
            {
                this.stnCollection = value;
                RaisePropertyChangedEvent("StnCollection");
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected location.
        /// </summary>
        public int StnIndex
        {
            get
            {
                return this.stnIndex;
            }

            set
            {
                this.stnIndex = value;
                RaisePropertyChangedEvent("StnIndex");
            }
        }

        /// <summary>
        /// Gets or sets the popular location collection.
        /// </summary>
        public ObservableCollection<string> PopularStnCollection
        {
            get
            {
                return this.popularStnCollection;
            }

            set
            {
                this.popularStnCollection = value;
                RaisePropertyChangedEvent("PopularStnCollection");
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected popular location.
        /// </summary>
        public int PopularStnIndex
        {
            get
            {
                return this.popularStnIndex;
            }

            set
            {
                this.popularStnIndex = value;
                RaisePropertyChangedEvent("PopularStnIndex");
            }
        }

        /// <summary>
        /// Add a location and sort alphabetically.
        /// </summary>
        private void AddStn()
        {
            List<string> tempCollection = this.PopularStnCollection.ToList();
            this.PopularStnCollection = new ObservableCollection<string>();

            tempCollection.Add(this.StnCollection[this.StnIndex]);

            tempCollection.Sort();

            // TODO, don't I have a toObservableCollection method.
            foreach (string stn in tempCollection)
            {
                this.PopularStnCollection.Add(stn);
            }
        }

        /// <summary>
        /// Indicate whether a location can be added.
        /// </summary>
        /// <returns>valid flag</returns>
        private bool CanAddStn()
        {
            if (this.StnIndex < 0 || this.StnIndex >= this.StnCollection?.Count)
            {
                return false;
            }

            return !this.PopularStnCollection.Contains(this.StnCollection[this.StnIndex]);
        }

        /// <summary>
        /// Delete a location.
        /// </summary>
        private void DeleteStn()
        {
            this.PopularStnCollection.RemoveAt(this.PopularStnIndex);
            this.RaisePropertyChangedEvent("PopularStnCollection");
        }

        /// <summary>
        /// Indicates whether a location can be deleted.
        /// </summary>
        /// <returns>valid flag</returns>
        private bool CanDeleteStn()
        {
            if (this.PopularStnCollection == null)
            {
                return false;
            }

            return this.PopularStnIndex >= 0 && this.PopularStnIndex < this.PopularStnCollection.Count;
        }

        /// <summary>
        /// Select the Ok command.
        /// </summary>
        private void SelectComplete(ICloseable window)
        {
            this.Result = MessageBoxResult.OK;

            PopularStnIOController locationController = PopularStnIOController.GetInstance();
            locationController.SaveFile(this.PopularStnCollection.ToList());

            window?.CloseObject();
        }

        /// <summary>
        /// Checks to see if Ok can be selected.
        /// </summary>
        /// <returns>can only select if not null or empty</returns>
        private bool CanSelectComplete(ICloseable window)
        {
            return true;
        }
    }
}