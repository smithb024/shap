namespace Shap.Config
{
    using System.IO;
    using Common;
    using Shap.Common.Factories;
    using Shap.Common.SerialiseModel.Family;
    using Shap.Interfaces.Io;

    /// <summary>
    /// Used to read and write to the family XML file.
    /// </summary>
    public class XmlFamilyIoController : IXmlFamilyIoController
    {
        /// <summary>
        /// Extension for the familty filename.
        /// </summary>
        private const string XmlExtensionLabel = ".xml";

        /// <summary>
        /// Prevents a default instance of this class from being created.
        /// </summary>
        public XmlFamilyIoController()
        {
        }

        /// <summary>
        /// Deserialise the <see cref="FamilyDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">name of the file to read</param>
        /// <returns>deserialised file</returns>
        public FamilyDetails Read()
        {
            string myPath = 
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                StaticResources.FileNameFamily + 
                XmlExtensionLabel;
            FamilyDetails results =
                XmlFileIo.ReadXml<FamilyDetails>(
                    myPath);

            return results;
        }

        /// <summary>
        /// Serialise the <see cref="FamilyDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        public void Write(
            FamilyDetails file)
        {
            string myPath = 
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                StaticResources.FileNameFamily + 
                XmlExtensionLabel;

            XmlFileIo.WriteXml(
                file,
                myPath);
        }

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file exists flag</returns>
        public bool DoesFileExist()
        {
            return File.Exists(
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                StaticResources.FileNameFamily +
                XmlExtensionLabel);
        }
    }
}