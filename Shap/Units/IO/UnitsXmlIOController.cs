namespace Shap.Units.IO
{
  using System;
  using System.Linq;
  using System.Xml.Linq; // XML
  using Common;
  using NynaeveLib.Logger;
  using Shap.Types;

  /// <summary>
  /// Used to read and write to the uts config file.
  /// </summary>
  public class UnitsXmlIOController
  {
    // Labels in the XML files
    private const string ClassVersionLabel = "ClassVersion";
    private const string FormationLabel = "Formation";
    private const string IdLabel = "Id";
    private const string ImageLabel = "Image";
    private const string NumberAttLabel = "No";
    private const string NumberLabel = "Number";
    private const string OldNumberAttLabel = "on";
    private const string OldNumberLabel = "OldNumber";
    private const string RootLabel = "ClassDetails";
    private const string SubClassLabel = "Subclass";
    private const string SubClassTypeLabel = "Type";
    private const string YearLabel = "Year";
    private const string AlphaIdLabel = "AlphaId";
    private const string XmlExtensionLabel = ".xml";

    //private bool inProgress = false;
    private string errorString = string.Empty;
    private Logger logger = Logger.Instance;
    private ClassDataType classDataFromFile;

    /// <summary>
    /// Prevents a default instance of this class from being created.
    /// </summary>
    /// <param name="unitsIoController">units IO controller</param>
    public UnitsXmlIOController(UnitsIOController unitsIoController)
    {
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>readClassDetailsXML</name>
    /// <date>12/05/13</date>
    /// <summary>
    ///   Reads the XML document at filename and populates a 
    ///     ClassDataType object (m_classDataFromFile) with the results.
    ///   It sets a m_inProgress flag when it starts to read the data, 
    ///     but doesn't clear it when completed. This is to prevent the
    ///     data from being overwritten before it has the chance to be 
    ///     used properly.
    ///   It returns a success flag.
    /// </summary>
    /// <param name="unitsIoController">units IO controller</param>
    /// <param name="filename">file name</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public ClassDataType ReadClassDetailsXML(
      UnitsIOController unitsIoController,
      string filename)
    {
      XDocument reader;
      int subClassIndex = 0;

      classDataFromFile = new ClassDataType(filename);

      try
      {
        reader =
        XDocument.Load(
          BasePathReader.GetBasePath() + StaticResources.classDetailsPath + filename + XmlExtensionLabel);
      }
      catch (Exception ex)
      {
        logger.WriteLog("ERROR: UnitsXmlIOController: Failed to read " + filename + XmlExtensionLabel + ": " + ex.ToString());
        return null;
      }

      XElement rootElement = reader.Root;
      string rootString = rootElement.Attribute(IdLabel).Value;

      XElement classVersionElement = rootElement.Element(ClassVersionLabel);
      classDataFromFile.Version = ConvertStringToInt((string)classVersionElement);

      classVersionElement = rootElement.Element(FormationLabel);
      classDataFromFile.Formation = (string)classVersionElement;

      classVersionElement = rootElement.Element(YearLabel);
      classDataFromFile.Year = ConvertStringToInt((string)classVersionElement);

      classVersionElement = rootElement.Element(AlphaIdLabel);
      classDataFromFile.SetAlphaIdentifier((string)classVersionElement);

      var subClasses = from Subclass in reader.Root.Elements(SubClassLabel)
                       select new
                       {
                         subClassId = (string)Subclass.Attribute(SubClassTypeLabel),
                         image = (string)Subclass.Element(ImageLabel),
                         number = from Number in Subclass.Elements(NumberLabel)
                                  select new
                                  {
                                    anumber = (string)Number.Attribute(NumberAttLabel),
                                    oldnumber = from OldNumber in Number.Elements(OldNumberLabel)
                                                select (string)OldNumber.Attribute(OldNumberAttLabel)
                                  }
                       };

      foreach (var subClass in subClasses)
      {
        classDataFromFile.AddNewSubClass(
          new SubClassDataType(
            unitsIoController,
            subClass.subClassId.ToString(),
            subClass.image));

        // loop through each number
        foreach (var num in subClass.number)
        {
          // read the number, convert it to an int, then add it to cDT.
          int currentNumber = ConvertStringToInt(num.anumber.ToString());
          if (currentNumber != 0)
          {
            classDataFromFile.AddCurrentNumber(
              subClassIndex,
              currentNumber);
          }
          else
          {
            return null;
          }

          // loop through any old numbers and append them to the current one. 
          foreach (var oldNum in num.oldnumber)
          {
            int oldNumber = ConvertStringToInt(oldNum);
            if (oldNumber != 0)
            {
              classDataFromFile.AddOldNumber(
                subClassIndex,
                oldNumber,
                currentNumber);
            }
            else
            {
              return null;
            }
          }
        }

        ++subClassIndex;
      }
      return classDataFromFile;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>writeClassDetailsXML</name>
    /// <date>12/05/13</date>
    /// <summary>
    ///   Creates a XDocument structure based on the incoming classData
    ///     data and writes it to the file defined by the filename.
    /// </summary>
    /// <param name="filename">file name</param>
    /// <param name="classData">class data</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool WriteClassDetailsXML(string filename, ClassDataType classData)
    {
      bool success = true;
      try
      {
        XDocument writer = new XDocument(
          new XDeclaration("1.0", "uft-8", "yes"),
          new XComment("abs myXmlFile.xml"));
        XElement classDetails = new XElement(RootLabel,
          new XAttribute(IdLabel, filename));
        XElement classVersion = new XElement(ClassVersionLabel, classData.Version);
        XElement classYear = new XElement(YearLabel, classData.Year);
        XElement classFormation = new XElement(FormationLabel, classData.Formation);
        XElement classAlphaId = new XElement(AlphaIdLabel, classData.AlphaIdentifier);

        // add the singleton data
        classDetails.Add(classVersion);
        classDetails.Add(classYear);
        classDetails.Add(classFormation);
        classDetails.Add(classAlphaId);

        // loop through each sub class and create an element for each one.
        foreach (SubClassDataType subClass in classData.GetSubClassList())
        {
          XElement subClassElement = new XElement(SubClassLabel,
            new XAttribute(SubClassTypeLabel, subClass.SubClassNumber));

          // create and add the singleton data.
          XElement imagePath = new XElement(ImageLabel, subClass.ImageName);
          subClassElement.Add(imagePath);

          // loop through each number in the sub class and create and element for each one
          foreach (VehicleNumberType vehNumber in subClass.VehicleNumbersList)
          {
            XElement vehNumberElement = new XElement(NumberLabel,
              new XAttribute(NumberAttLabel, vehNumber.VehicleNumber.ToString()));

            // loop through each old number for the current number and create an element for each one
            foreach (int oldNumber in vehNumber.FormerNumbers)
            {
              XElement oldNumberElement = new XElement(OldNumberLabel,
                new XAttribute(OldNumberAttLabel, oldNumber.ToString()));

              // add the old number element to the current number
              vehNumberElement.Add(oldNumberElement);
            }

            // add the number element to the current subclass
            subClassElement.Add(vehNumberElement);
          }

          // add the subclass element to the root element.
          classDetails.Add(subClassElement);
        }

        writer.Add(classDetails);

        // This creates a new file based on writer.
        writer.Save(BasePathReader.GetBasePath() +
                    StaticResources.classDetailsPath +
                    filename +
                    XmlExtensionLabel);
      }
      catch (Exception ex)
      {
        logger.WriteLog("ERROR: UnitsXmlIOController: Failed to create XDocument");
        logger.WriteLog(ex.ToString());
      }

      return success;
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>doesFileExist</name>
    /// <date>28/09/12</date>
    /// <summary>
    ///   Checks to see if a file exists.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <returns>file exists flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public bool DoesFileExist(string fileName)
    {
      return System.IO.File.Exists(BasePathReader.GetBasePath() +
                                   StaticResources.classDetailsPath +
                                   fileName +
                                   XmlExtensionLabel);
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>convertStringToInt</name>
    /// <date>06/05/13</date>
    /// <summary>
    ///   Converts the incoming string to an integer.
    /// </summary>
    /// <param name="inputString">input string</param>
    /// <returns>integer number</returns>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    private int ConvertStringToInt(string inputString)
    {
      int tempNumber = 0;

      try
      {
        tempNumber = int.Parse(inputString);
      }
      catch (Exception ex)
      {
        logger.WriteLog("ERROR: UnitsXmlIOController: Failed to convert number: " + inputString + " from XML file");
        logger.WriteLog(ex.ToString());
      }

      return tempNumber;
    }
  }
}