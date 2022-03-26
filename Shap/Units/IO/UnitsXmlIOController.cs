namespace Shap.Units.IO
{
    using System.IO;
    using Common;
    using Shap.Common.Factories;
    using Shap.Common.SerialiseModel.ClassDetails;
    using Shap.Interfaces.Io;

    /// <summary>
    /// Used to read and write to the uts config file.
    /// </summary>
    public class UnitsXmlIOController : IUnitsXmlIoController
    {
        // Labels in the XML files
        private const string XmlExtensionLabel = ".xml";

        /// <summary>
        /// Prevents a default instance of this class from being created.
        /// </summary>
        public UnitsXmlIOController()
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