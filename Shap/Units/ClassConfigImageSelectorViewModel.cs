namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Common;
    using Shap.Interfaces.Io;
    using Shap.Interfaces.Units;

    /// <summary>
    /// View model which supports the 
    /// </summary>
    public class ClassConfigImageSelectorViewModel : ObservableRecipient,  IClassConfigImageSelectorViewModel
    {
        /// <summary>
        /// The index of the image list for the currently selected image.
        /// </summary>
        int imageIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="ClassConfigImageSelectorViewModel"/> class.
        /// </summary>
        /// <param name="unitsIoController">IO Controller</param>
        /// <param name="loadedImageName">initial value of the image</param>
        public ClassConfigImageSelectorViewModel(
            IIoControllers ioControllers,
            string loadedImageName)
        {

            this.SubClassImageList = new ObservableCollection<string>();
            List<string> imageFileNames = ioControllers.Units.GetImageFileList();
            foreach (string str in imageFileNames)
            {
                this.SubClassImageList.Add(str);
            }

            this.imageIndex = -1;

            if (!string.IsNullOrWhiteSpace(loadedImageName))
            {
                for(int index = 0; index < this.SubClassImageList.Count; ++ index)
                {
                    if (string.Compare(this.SubClassImageList[index], loadedImageName) == 0)
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
        public ObservableCollection<string> SubClassImageList { get; }

        /// <summary>
        /// Gets or sets the index for the list of sub class image lists.
        /// </summary>
        public int SubClassImageListIndex
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
                this.OnPropertyChanged(nameof(this.SubClassImageListIndex));
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
                if (this.SubClassImageList == null ||
                    this.SubClassImageList.Count == 0)
                {
                    return string.Empty;
                }

                if (this.SubClassImageListIndex >= 0 &&
                    this.SubClassImageListIndex < this.SubClassImageList.Count)
                {
                    return this.SubClassImageList[this.SubClassImageListIndex];
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
              this.SubClassImageList[this.SubClassImageListIndex] +
              ".jpg";

                return returnString;
            }
        }
    }
}