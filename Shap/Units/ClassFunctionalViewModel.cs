namespace Shap.Units
{
  using System;
  using System.ComponentModel;
  using System.Collections.ObjectModel;

  using Base;

  using Shap.Interfaces.Units;
  using Shap.Units.Factories;
  using Shap.Units.IO;
  using NynaeveLib.Logger;

  using Stats;
  using Types;

  /// <summary>
  /// This class is used for the <see cref="ClassFrontPage"/> view. It is used to manage all 
  /// aspects of the class which is to be displayed. Effectively this means the sub classes.
  /// </summary>
  public class ClassFunctionalViewModel : SubClassSelectorViewModel
  {
    /// <summary>
    /// Manager class holding collections of the first examples.
    /// </summary>
    private FirstExampleManager firstExamples;
    private string classId = string.Empty;
    private ClassDataType classData;

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ClassFrontPageForm</name>
    /// <date>29/12/12</date>
    /// <summary>
    ///   create a new instance of this class
    /// </summary>
    /// <param name="unitsIoController">units IO controller</param>
    /// <param name="unitsXmlIoController">Units XML IO Controller</param>
    /// <param name="individualUnitIoController">Individual Unit IO Controller</param>
    /// <param name="classId">class id</param>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public ClassFunctionalViewModel(
      UnitsIOController unitsIoController,
      UnitsXmlIOController unitsXmlIoController,
      FirstExampleManager firstExamples,
      string classId)
      : base(new ObservableCollection<string>())
    {
      this.classId          = classId;
      this.firstExamples = firstExamples;

      this.classData        = new ClassDataType(classId);
      this.ClassIndexes = new ObservableCollection<SubClassViewModel>();

      if (!unitsXmlIoController.DoesFileExist(classId))
      {
        Logger.Instance.WriteLog($"ClassFunctionalViewModel: Aborted load: {classId} does not exist");
        return;
      }

      this.classData =
        unitsXmlIoController.ReadClassDetailsXML(
          unitsIoController,
          classId);

      if (this.classData == null)
      {
        Logger.Instance.WriteLog($"ClassFunctionalViewModel: Aborted load: {classId} does not exist");
        return;
      }

          this.classData.InitaliseSubClassIndex();

          this.SubClasses = this.classData.SubClassNumbers;
          if (this.SubClasses.Count > 0)
          {
            this.SubClassIndex = 0;
          }

          this.PropertyChanged += UpdateProperties;

      this.LoadUnits();
    }

    public ObservableCollection<SubClassViewModel> ClassIndexes { get; set; }

    public SubClassViewModel CurrentIndex
    {
      get
      {
        if (this.ClassIndexes == null ||
          this.ClassIndexes.Count == 0)
        {
          return null;
        }

        return this.ClassIndexes[this.SubClassIndex];
      }
    }

    /// <summary>
    ///   return m_classId.
    /// </summary>
    public string ClassId => this.classId;

    /// <summary>
    /// Gets the class data.
    /// </summary>
    public ClassDataType ClassData => this.classData;

    /// <date>12/08/18</date>
    /// <summary>
    ///   Loads each vehicle in turn.
    /// </summary>
    private void LoadUnits()
    {
      this.ClassIndexes =
        SubClassFactory.CreateSubClasses(
          this.firstExamples,
          this.ClassData.SubClassList,
          this);
    }

    private void ModelChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    private void UpdateProperties(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName.Equals("SubClassIndex"))
      {
        this.RaisePropertyChangedEvent("CurrentIndex");
      }
    }
  }
}
