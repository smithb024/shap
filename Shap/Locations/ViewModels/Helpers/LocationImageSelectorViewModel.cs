namespace Shap.Locations.ViewModels.Helpers
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Common;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Locations.ViewModels.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 
    /// </summary>
    public class LocationImageSelectorViewModel : ObservableRecipient, ILocationImageSelectorViewModel
    {
        /// <summary>
        /// The index of the image list for the currently selected image.
        /// </summary>
        int imageIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="LocationImageSelectorViewModel"/> class.
        /// </summary>
        /// <param name="unitsIoController">IO Controller</param>
        /// <param name="loadedImageName">initial value of the image</param>
        public LocationImageSelectorViewModel(
            IIoControllers ioControllers,
            string loadedImageName)
        {

            this.LocationImageList = new ObservableCollection<string>();
            List<string> imageFileNames = ioControllers.Units.GetImageFileList();
            foreach (string str in imageFileNames)
            {
                this.LocationImageList.Add(str);
            }

            this.imageIndex = -1;

            if (!string.IsNullOrWhiteSpace(loadedImageName))
            {
                for (int index = 0; index < this.LocationImageList.Count; ++index)
                {
                    if (string.Compare(this.LocationImageList[index], loadedImageName) == 0)
                    {
                        this.imageIndex = index;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when a new selection has been raised.
        /// </summary>
        public event Action SelectionMadeEvent;

        /// <summary>
        /// Gets the list of sub class image lists.
        /// </summary>
        public ObservableCollection<string> LocationImageList { get; }

        /// <summary>
        /// Gets or sets the index for the list of sub class image lists.
        /// </summary>
        public int LocationImageListIndex
        {
            get
            {
                return this.imageIndex;
            }

            set
            {
                if (this.imageIndex == value)
                {
                    return;
                }

                this.imageIndex = value;
                this.OnPropertyChanged(nameof(this.LocationImageListIndex));
                this.OnPropertyChanged(nameof(this.SelectedImage));
                this.OnPropertyChanged(nameof(this.Path));
                this.SelectionMadeEvent?.Invoke();
            }
        }

        /// <summary>
        /// Gets the image which has been selected within in the view model.
        /// </summary>
        public string SelectedImage
        {
            get
            {
                if (this.LocationImageList == null ||
                    this.LocationImageList.Count == 0)
                {
                    return string.Empty;
                }

                if (this.LocationImageListIndex >= 0 &&
                    this.LocationImageListIndex < this.LocationImageList.Count)
                {
                    return this.LocationImageList[this.LocationImageListIndex];
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the path to the selected image.
        /// </summary>
        public string Path
        {
            get
            {
                if (string.IsNullOrEmpty(this.SelectedImage))
                {
                    return string.Empty;
                }

                string returnString = BasePathReader.GetBasePathUri() +
              StaticResources.classImgPath +
              this.LocationImageList[this.LocationImageListIndex] +
              ".jpg";

                return returnString;
            }
        }
    }
}
