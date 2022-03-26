﻿namespace Shap.Interfaces.Units
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Shap.Types;

    /// <summary>
    /// Interface which supports the class configuration view model.
    /// </summary>
    public interface IClassConfigViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the version of this file.
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets or sets the formation of the unit.
        /// </summary>
        string Formation { get; set; }

        /// <summary>
        /// Gets or sets an alpha prefix.
        /// </summary>
        string AlphaIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the year of introduction.
        /// </summary>
        int Year { get; set; }

        /// <summary>
        /// Gets a collection containing all the enumerations in <see cref="VehicleServiceType"/>.
        /// </summary>
        /// <remarks>
        /// This is bound to a combo box list.
        /// </remarks>
        List<VehicleServiceType> ServiceTypeList { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected in service state from the
        /// <see cref="ServiceTypeList"/> list of in service states.
        /// </summary>
        int ServiceIndex { get; set; }

        /// <summary>
        /// Gets a collection of all known families.
        /// </summary>
        ObservableCollection<string> FamilyList { get; }

        /// <summary>
        /// Gets or sets the index of the currently selected family.
        /// </summary>
        int FamilyIndex { get; set; }

        /// <summary>
        /// Gets the collection of sub classes present in this class.
        /// </summary>
        ObservableCollection<string> SubClassNumbers { get; }

        /// <summary>
        /// Gets or sets the index of the selected subclass.
        /// </summary>
        int SubClassListIndex { get; set; }

        /// <summary>
        /// Get the collection of unit numbers in the current subclass.
        /// </summary>
        ObservableCollection<int> NumbersList { get; }

        /// <summary>
        /// Get the collection of image selector view models.
        /// </summary>
        ObservableCollection<IClassConfigImageSelectorViewModel> Images { get; }

        /// <summary>
        /// Indicates whether the save command can be run.
        /// </summary>
        bool CanSave { get; set; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand SaveCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand AddNewSubClassCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand AddNewNumberCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand AddNewNumberSeriesCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand RenumberCmd { get; }

        /// <summary>
        /// Close window command.
        /// </summary>
        ICommand CloseCmd { get; }

        ///// <summary>
        /////   return m_classData.getSubClassCount()
        ///// </summary>
        ///// <returns>sub class count</returns>
        //int GetSubClassCount();

        ///// <summary>
        /////   return m_classData.getSubClassCount()
        ///// </summary>
        ///// <param name="index">sub class index</param>
        ///// <returns>sub class</returns>
        //string GetSubClass(int index);

        ///// <summary>
        /////   Gets the number of CurrentNumbers stored in m_classData
        ///// </summary>
        ///// <param name="subClassIndex">sub class index</param>
        ///// <returns>current number count</returns>
        //int GetCurrentNumberCount(int subClassIndex);

        ///// <summary>
        /////   Gets the current number in m_classData for the subClass at
        /////     subClassIndex and the number at numberIndex.
        ///// </summary>
        ///// <param name="subClassIndex">sub class index</param>
        ///// <param name="numberIndex">number index</param>
        ///// <returns>current number</returns>
        //int GetCurrentNumber(int subClassIndex, int numberIndex);
    }
}
