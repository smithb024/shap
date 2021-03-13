namespace Shap.Interfaces.Units
{
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
        /// Gets or sets the version of this file.
        /// </summary>
        int Version { get; set; }

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
        string Year { get; set; }

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
        ObservableCollection<string> NumbersList { get; }

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
