namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Locations.Enums;
    using Shap.Locations.Messages;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A view model which supports location navigation. This view model supports a combo box from 
    /// which the navigation search criteria is selected.
    /// </summary>
    public class DirectNavigationViewModel : ObservableRecipient, IDirectNavigationViewModel
    {
        /// <summary>
        /// The index of the currently selected <see cref="SearchCriteria"/>.
        /// </summary>
        private int index;

        /// <summary>
        /// The type of object being displayed.
        /// </summary>
        private SelectorType type;

        /// <summary>
        /// Initialises a new instance of the <see cref="DirectNavigationViewModel"/> class.
        /// </summary>
        /// <param name="ioControllers">The IO Controller manager object</param>
        /// <param name="type">The type of selection criteria</param>
        public DirectNavigationViewModel(
            IIoControllers ioControllers,
            SelectorType type)
        {
            this.type = type;

            switch (type)
            {
                case SelectorType.Operator:
                    this.SearchCriteria = new List<string>();
                    break;

                case SelectorType.Region:
                    this.SearchCriteria = ioControllers.Location.GetRegions();
                    break;

                case SelectorType.Lines:
                    this.SearchCriteria = new List<string>();
                    break;

                default:
                    this.SearchCriteria = new List<string>();
                    break;
            }

            this.index = -1;
        }

        /// <summary>
        /// Gets the index of the selected search criteria.
        /// </summary>
        public int SearchCriteriaIndex
        {
            get => this.index;
            set
            {
                if (this.index != value)
                {
                    this.index = value;
                    this.OnPropertyChanged(nameof(this.SearchCriteriaIndex));
                    this.SendDisplayRequest();
                }
            }
        }

        /// <summary>
        /// Gets the collection of all possible search criteria.
        /// </summary>
        public List<string> SearchCriteria { get; }

        /// <summary>
        /// Dispose this object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class.
        /// </summary>
        /// <param name="disposing">Is the class being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        /// Send a request to display a new set of locations.
        /// </summary>
        private void SendDisplayRequest()
        {
            if (this.SearchCriteriaIndex < 0 ||
                this.SearchCriteriaIndex >= this.SearchCriteria.Count)
            {
                return;
            }

            switch (this.type)
            {
                case SelectorType.Operator:
                    break;

                case SelectorType.Region:
                    RegionSelectorMessage regionMessage =
                        new RegionSelectorMessage(
                            this.SearchCriteria[this.SearchCriteriaIndex]);
                    this.Messenger.Send(regionMessage);
                    break;

                case SelectorType.Lines:
                    break;

                default:
                    return;
            }

        }
    }
}