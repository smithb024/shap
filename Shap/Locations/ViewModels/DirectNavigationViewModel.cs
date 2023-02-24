﻿namespace Shap.Locations.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Shap.Icon;
    using Shap.Interfaces.Locations.ViewModels;
    using System;

    public class DirectNavigationViewModel : ObservableRecipient, IDirectNavigationViewModel
    {
        public DirectNavigationViewModel()
        {
        }

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
    }
}
