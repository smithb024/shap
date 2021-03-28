namespace Shap.Units.Factories
{
    using System.Collections.Generic;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Units.IO;

    /// <summary>
    /// Newid ffatri rhifau trên.
    /// </summary>
    public static class RenumberFactory
    {
        /// <summary>
        /// Renumber each unit as directed.
        /// </summary>
        /// <param name="classFileConfiguration">Class file contents</param>
        /// <param name="classId">id of the class</param>
        /// <param name="initialNumber">the first number to change</param>
        /// <param name="initialSubclass">location of the first number</param>
        /// <param name="destinationNumber">the first destination number</param>
        /// <param name="destinationSubclass">location of the destination number</param>
        /// <param name="numberToChange">number of numbers to change</param>
        public static void Renumber(
            ClassDetails classFileConfiguration,
            string classId,
            int initialNumber,
            string initialSubclass,
            int destinationNumber,
            string destinationSubclass,
            int numberToChange)
        {
            List<int> originalNumbers = new List<int>();

            Subclass subclass =
                classFileConfiguration.Subclasses.Find(
                    s => string.Compare(s.Type, initialSubclass) == 0);
            Subclass newSubclass =
                classFileConfiguration.Subclasses.Find(
                    s => string.Compare(s.Type, destinationSubclass) == 0);

            if (subclass == null || destinationSubclass == null)
            {
                return;
            }

            originalNumbers =
                RenumberFactory.GetOriginalNumbers(
                    classFileConfiguration,
                    initialNumber,
                    subclass,
                    numberToChange);

            RenumberFactory.RenumberUnits(
                subclass,
                newSubclass,
                originalNumbers,
                classId,
                destinationNumber);
        }

        /// <summary>
        /// Cael yr holl rifau gwreiddiol.
        /// </summary>
        /// <param name="classFileConfiguration">class file contents</param>
        /// <param name="initialNumber">rhif cyntaf</param>
        /// <param name="subclass">subclass containing our numbers</param>
        /// <param name="numberToChange">total number of changes</param>
        /// <returns></returns>
        private static List<int> GetOriginalNumbers(
            ClassDetails classFileConfiguration,
            int initialNumber,
            Subclass subclass,
            int numberToChange)
        {
            bool startCount = false;
            List<int> numbers = new List<int>();

            foreach (Number unitNumber in subclass.Numbers)
            {
                if (unitNumber.CurrentNumber == initialNumber)
                {
                    startCount = true;
                }

                if (startCount)
                {
                    numbers.Add(unitNumber.CurrentNumber);
                }

                if (numbers.Count == numberToChange)
                {
                    return numbers;
                }
            }

            return numbers;
        }

        /// <summary>
        /// Newid trên rhifau.
        /// </summary>
        /// <param name="originalSubclass">the original sub class</param>
        /// <param name="destinationSubclass">the destination sub class</param>
        /// <param name="originalNumbers">original numbers</param>
        /// <param name="classId">the class id</param>
        /// <param name="initialDestinationNumber">the first of the new numbers</param>
        private static void RenumberUnits(
            Subclass originalSubclass,
            Subclass destinationSubclass,
            List<int> originalNumbers,
            string classId,
            int initialDestinationNumber)
        {
            int loop = 0;
            foreach (int originalNumber in originalNumbers)
            {
                int destinationNumber = initialDestinationNumber + loop;

                Number updatedNumber = 
                    originalSubclass.Numbers.Find(
                        n => n.CurrentNumber == originalNumber);
                originalSubclass.Numbers.Remove(updatedNumber);

                OldNumber oldNumber =
                    new OldNumber()
                    {
                        Number = originalNumber
                    };

                updatedNumber.Historical.Add(oldNumber);
                updatedNumber.CurrentNumber = destinationNumber;

                destinationSubclass.Numbers.Add(updatedNumber);
                destinationSubclass.Numbers.Sort((x, y) => x.CurrentNumber.CompareTo(y.CurrentNumber));

                IndividualUnitIOController.RenameIndividualUnitFile(
                    destinationNumber,
                    classId,
                    originalNumber);

                    ++loop;
            }
        }
    }
}
