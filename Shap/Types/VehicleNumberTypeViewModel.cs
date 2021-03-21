namespace Shap.Types
{
    using System.Collections.Generic;
    using System.Linq;
    using NynaeveLib.ViewModel;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Interfaces.Types;

    /// <summary>
    /// Vehicle number type
    /// </summary>
    public class VehicleNumberTypeViewModel : ViewModelBase, IVehicleNumberType
    {
        /// <summary>
        /// Collection of former numbers associated with this unit.
        /// </summary>
        private List<int> formerNumbers = new List<int>();

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>VehicleNumberType</name>
        /// <date>12/08/12</date>
        /// <summary>
        ///   Create a new instance of this class
        /// </summary>
        /// <param name="number">vcle number</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public VehicleNumberTypeViewModel(int number)
        {
            this.VehicleNumber = number;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="VehicleNumberTypeViewModel"/> class.
        /// </summary>
        /// <param name="xmlData">data from the file</param>
        public VehicleNumberTypeViewModel(Number xmlData)
        {
            this.VehicleNumber = xmlData.CurrentNumber;

            this.FormerNumbers = new List<int>();
            
            foreach(OldNumber oldNumber in xmlData.Historical)
            {
                this.formerNumbers.Add(oldNumber.Number);
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>VehicleNumberType</name>
        /// <date>15/10/12</date>
        /// <summary>
        ///   Empty constructor
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public VehicleNumberTypeViewModel()
          : this(0)
        {
        }

        /// <summary>
        /// Gets the vehicle number
        /// </summary>
        public int VehicleNumber { get; set; }

        /// <summary>
        /// Gets the collection of former numbers associated with this unit.
        /// </summary>
        public List<int> FormerNumbers
        {
            get
            {
                return this.formerNumbers;
            }

            private set
            {
                this.formerNumbers = value;
                this.RaisePropertyChangedEvent("FormerNumbers");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AddFormerNumber</name>
        /// <date>12/08/12</date>
        /// <summary>
        /// addOldNumber, append an old number.
        /// </summary>
        /// <param name="number">old number</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public void AddFormerNumber(int number)
        {
            this.FormerNumbers.Add(number);
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetNumberOfFormerNumbers</name>
        /// <date>19/08/12</date>
        /// <summary>
        ///   returns the count of old numbers
        /// </summary>
        /// <returns>count of old numbers</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int GetNumberOfFormerNumbers() => formerNumbers.Count();

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ToString</name>
        /// <date>02/11/12</date>
        /// <summary>
        ///   return string of format "no1 tab no2 tab no3"
        /// </summary>
        /// <returns>output string</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public override string ToString()
        {
            string outputString = string.Empty;
            for (int index = 0; index < formerNumbers.Count(); ++index)
            {
                if (index == 0)
                {
                    outputString = formerNumbers[index].ToString();
                }
                else
                {
                    outputString += "\t" + formerNumbers[index].ToString();
                }
            }

            return outputString;
        }
    }
}