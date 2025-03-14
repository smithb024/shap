﻿namespace Shap.Config
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using NynaeveLib.Commands;
    using NynaeveLib.DialogService.Interfaces;
    using NynaeveLib.ViewModel;
    using Shap.Messages;
    using Shap.StationDetails;
    using Shap.Types.Enum;
    using NynaeveMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the popular config stn dialog.
    /// </summary>
    public class PopularLocConfigViewModel : DialogViewModelBase
    {
        /// <summary>
        /// The location collection to show on the dialog.
        /// </summary>
        ObservableCollection<string> stnCollection;

        /// <summary>
        /// The index of the currently selected location.
        /// </summary>
        int stnIndex;

        /// <summary>
        /// The collection of popular locations to show on the dialog.
        /// </summary>
        ObservableCollection<string> popularStnCollection;

        /// <summary>
        /// The index of the currently selected popular location.
        /// </summary>
        int popularStnIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="PopularLocConfigViewModel"/> class.
        /// </summary>
        public PopularLocConfigViewModel()
        {
            this.Result = MessageBoxResult.Cancel;
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

            this.AddCmd = new CommonCommand(this.AddLoc, this.CanAddStn);
            this.DeleteCmd = new CommonCommand(this.DeleteLoc, this.CanDeleteStn);
            this.CompleteCmd = new CommonCommand<ICloseable>(this.SelectComplete, this.CanSelectComplete);
        }

        /// <summary>
        /// Gets the add an item command.
        /// </summary>
        public ICommand AddCmd { get; private set; }

        /// <summary>
        /// Gets the delete the selected item.
        /// </summary>
        public ICommand DeleteCmd { get; private set; }

        /// <summary>
        /// Gets the Ok command.
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
                this.OnPropertyChanged("StnCollection");
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
                this.OnPropertyChanged("StnIndex");
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
                this.OnPropertyChanged("PopularStnCollection");
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
                this.OnPropertyChanged("PopularStnIndex");
            }
        }

        /// <summary>
        /// Add a location to the list.
        /// </summary>
        private void AddLoc()
        {
            FeedbackMessage openMessage =
                      new FeedbackMessage(
                          FeedbackType.Command,
                          $"PopLoc Config - Add location {this.StnCollection[this.StnIndex]}.");
            NynaeveMessenger.Default.Send(openMessage);

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
        /// Indicates whether it is possible to run the add location command.
        /// </summary>
        /// <returns>Can run flag</returns>
        private bool CanAddStn()
        {
            if (this.StnIndex < 0 || this.StnIndex >= this.StnCollection?.Count)
            {
                return false;
            }

            return !this.PopularStnCollection.Contains(this.StnCollection[this.StnIndex]);
        }

        /// <summary>
        /// Delete the selected location.
        /// </summary>
        private void DeleteLoc()
        {
            FeedbackMessage openMessage =
                      new FeedbackMessage(
                          FeedbackType.Command,
                          $"PopLoc Config - Delete location {this.PopularStnCollection[this.PopularStnIndex]}.");
            NynaeveMessenger.Default.Send(openMessage);

            this.PopularStnCollection.RemoveAt(this.PopularStnIndex);
            this.OnPropertyChanged(nameof(this.PopularStnCollection));
        }

        /// <summary>
        /// Indicates whether it is possible to run the delete location command.
        /// </summary>
        /// <returns>Can run flag</returns>
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
            FeedbackMessage openMessage =
                     new FeedbackMessage(
                         FeedbackType.Command,
                         $"PopLoc Config - Complete and close.");
            NynaeveMessenger.Default.Send(openMessage);

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