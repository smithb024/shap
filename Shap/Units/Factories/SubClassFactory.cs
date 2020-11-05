namespace Shap.Units.Factories
{
    using System.Collections.ObjectModel;

    using Shap.Units.IO;
    using Shap.Interfaces.Units;
    using Types;
    using Stats;
    using NynaeveLib.Logger;

    /// <summary>
    /// Factory class handling sub class view model creation.
    /// </summary>
    public static class SubClassFactory
    {
        public static ObservableCollection<SubClassViewModel> CreateSubClasses(
          FirstExampleManager firstExamples,
          ObservableCollection<SubClassDataType> modelSubClasses,
          ClassFunctionalViewModel parent)
        {
            ObservableCollection<SubClassViewModel> subClasses =
              new ObservableCollection<SubClassViewModel>();

            foreach (SubClassDataType modelSubClass in modelSubClasses)
            {
                ObservableCollection<IUnitViewModel> units =
                  new ObservableCollection<IUnitViewModel>();

                for (int index = 0; index < modelSubClass.VehicleNumbersList.Count; ++index)
                {
                    // Read raw data from the file and used to create a new unit.
                    IndividualUnitFileContents unitRaw =
                      IndividualUnitIOController.ReadIndividualUnitFile(
                        parent.ClassId,
                        modelSubClass.VehicleNumbersList[index].VehicleNumber.ToString());

                    if (unitRaw == null)
                    {
                        Logger.Instance.WriteLog($"Failed to read unit from file, data missing: {modelSubClass.VehicleNumbersList[index].VehicleNumber}");
                    }
                    else
                    {
                        IUnitViewModel newUnit =
                          new UnitViewModel(
                            IndividualUnitIOController.WriteIndividualUnitFile,
                            firstExamples,
                            parent.ClassId,
                            unitRaw,
                            index == 0,
                            index == modelSubClass.VehicleNumbersList.Count - 1,
                            modelSubClass.SubClassImagePath,
                            parent.ClassData.AlphaIdentifier);

                        units.Add(newUnit);
                    }
                }

                SubClassViewModel newSubClass =
                  new SubClassViewModel(
                    parent,
                    units);

                subClasses.Add(newSubClass);
            }

            return subClasses;
        }
    }
}