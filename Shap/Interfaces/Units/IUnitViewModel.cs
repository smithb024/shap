namespace Shap.Interfaces.Units
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using NynaeveLib.ViewModel;
    using Shap.Interfaces.Types;
    using Shap.Units;
    using Shap.Types;

    public interface IUnitViewModel : IViewModelBase, IVehicleDetailsType
    {
        // Event used to open a 
        event VehicleDataDelegate OpenEvent;
        event VehicleDataDelegate CloseEvent;
        event VehicleDataDelegate PreviousEvent;
        event VehicleDataDelegate NextEvent;

        /// <summary>
        /// Open window command.
        /// </summary>
        ICommand OpenWindowCmd { get; }


        /// <summary>
        /// 
        /// </summary>
        ICommand PreviousUnitCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        ICommand NextUnitCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        ICommand UpdateUnitCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        ICommand RefreshUnitCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        ICommand SaveUnitCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        ICommand CloseWindowCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        string AlphaIdentifier { get; }

        /// <summary>
        /// Inidicates whether this is the first unit in the list
        /// </summary>
        bool FirstUnit { get; set; }

        /// <summary>
        /// Indicates whether this is the last unit in the list.
        /// </summary>
        bool LastUnit { get; set; }

        ///// <summary>
        ///// Gets the path to the image for this <see cref="IUnitViewModel"/>
        ///// </summary>
        //string ImagePath { get; }

        /// <summary>
        /// Gets the path to the sub class image.
        /// </summary>
        string SubClassImagePath { get; }

        /// <summary>
        /// Gets the unit number to display (Includes alpha identifier if one present).
        /// </summary>
        string DisplayUnitNumber { get; }

        /// <summary>
        /// Refresh the units data.
        /// </summary>
        void RefreshUnit();
    }
}