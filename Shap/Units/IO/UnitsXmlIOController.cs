namespace Shap.Units.IO
{
    using System.IO;
    using Common;
    using NynaeveLib.Logger;
    using Shap.Common.Factories;
    using Shap.Common.SerialiseModel.ClassDetails;
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
        private ClassDataTypeViewModel classDataFromFile;

        /// <summary>
        /// Prevents a default instance of this class from being created.
        /// </summary>
        /// <param name="unitsIoController">units IO controller</param>
        public UnitsXmlIOController(UnitsIOController unitsIoController)
        {
        }

        /// <summary>
        /// Deserialise the <see cref="ClassDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">name of the file to read</param>
        /// <returns>deserialised file</returns>
        public ClassDetails Read(string filename)
        {
            string myPath = BasePathReader.GetBasePath() + StaticResources.classDetailsPath + filename + XmlExtensionLabel;
            ClassDetails results =
                XmlFileIo.ReadXml<ClassDetails>(
                    myPath);

            return results;
        }

        /// <summary>
        /// Serialise the <see cref="ClassDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        public void Write(
            ClassDetails file,
            string filename)
        {
            string myPath = BasePathReader.GetBasePath() + StaticResources.classDetailsPath + filename + XmlExtensionLabel;

            XmlFileIo.WriteXml<ClassDetails>(
                file,
                myPath);
        }

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file exists flag</returns>
        public bool DoesFileExist(string fileName)
        {
            return File.Exists(
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                fileName +
                XmlExtensionLabel);
        }
    }
}