namespace Shap.Interfaces.Types
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NynaeveLib.ViewModel;
    using Units;
    using Common;

    using Shap.Types;

    /// <summary>
    /// Interface for the sub class data
    /// </summary>
    public interface ISubClassDataType
    {
        /// <summary>
        /// Gets the sub class number.
        /// </summary>
        string SubClassNumber { get; }

        /// <summary>
        /// List of sub class image lists.
        /// </summary>
        ObservableCollection<string> SubClassImageList { get; set; }

        /// <summary>
        /// Name of the image file used.
        /// </summary>
        string ImageName { get; set; }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        ObservableCollection<VehicleNumberTypeViewModel> VehicleNumbersList { get; }

        /// <summary>
        /// Index for the list of sub class image lists.
        /// </summary>
        ObservableCollection<string> NumbersList { get; }

        /// <summary>
        ///   addCurrentNumber, append an new number to the m_numberList
        ///   It first checks to see if the number doesn't already exist.
        /// </summary>
        /// <param name="currentNumber">current number</param>
        /// <returns>success flag</returns>
        bool AddCurrentNumber(int currentNumber);

        /// <summary>
        /// Add the <paramref name="newNumber"/> to the number list.
        /// </summary>
        /// <param name="newNumber"></param>
        void AddNumber(VehicleNumberTypeViewModel newNumber);

        /// <summary>
        /// Remove the <paramref name="oldNumber"/> from the number list.
        /// </summary>
        /// <param name="oldNumber"></param>
        void RemoveNumber(VehicleNumberTypeViewModel oldNumber);

        /// <summary>
        /// append an new oldNumber to the specified <paramref name="currentNumber"/>.
        /// </summary>
        /// <param name="oldNumber">old number</param>
        /// <param name="currentNumber">current number</param>
        void AddOldNumber(
          int oldNumber,
          int currentNumber);

        /// <summary>
        ///   returns the vehicle number at the specified index.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <returns>current number</returns>
        int GetCurrentNumber(int numberIndex);

        /// <summary>
        ///   returns the length of the numberList
        /// </summary>
        /// <returns>number count</returns>
        int GetCurrentNumberCount();

        /// <summary>
        ///   The current number is known, but what index is it?
        /// </summary>
        /// <param name="currentNumber">current number</param>
        /// <returns>current number index</returns>
        int GetCurrentNumberIndex(string currentNumber);

        /// <summary>
        ///   returns the oldNumber at the specified current number.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <param name="oldNumberIndex">old number index</param>
        /// <returns>old number</returns>
        int GetOldNumber(
          int numberIndex,
          int oldNumberIndex);

        /// <summary>
        ///   returns the number of oldNumbers for the specified current
        ///   number.
        /// </summary>
        /// <param name="numberIndex">number index</param>
        /// <returns>old number count</returns>
        int GetOldNumberCount(int numberIndex);

        /// <summary>
        ///   Sorts the m_subClassList by subclass number.
        /// </summary>
        void SortNumbers();

        /// <summary>
        ///   deletes number.
        /// </summary>
        /// <param name="currentNumberIndex">current number index</param>
        void DeleteCurrentNumber(int currentNumberIndex);

        /// <summary>
        /// Gets a list of all the current numbers in this subclass.
        /// </summary>
        /// <returns>list of current numbers</returns>
        ObservableCollection<int> GetAllNumbers();

        /// <summary>
        /// Return the path for an image, based on a specific index.
        /// </summary>
        /// <param name="index">image index</param>
        /// <returns>image path</returns>
        string GetImagePath(int index);
    }
}