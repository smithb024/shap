namespace Shap.Config
{
    using System.IO;
    using Common;
    using Shap.Common.Factories;
    using Shap.Common.SerialiseModel.Operator;
    using Shap.Interfaces.Io;

    /// <summary>
    /// Used to read and write to the operator XML file.
    /// </summary>
    public class XmlOperatorIoController : IXmlOperatorIoController
    {
        /// <summary>
        /// Extension for the operator filename.
        /// </summary>
        private const string XmlExtensionLabel = ".xml";

        /// <summary>
        /// Prevents a default instance of this class from being created.
        /// </summary>
        public XmlOperatorIoController()
        {
        }

        /// <summary>
        /// Deserialise the <see cref="OperatorDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">name of the file to read</param>
        /// <returns>deserialised file</returns>
        public OperatorDetails Read()
        {
            string myPath = 
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                StaticResources.FileNameOperator + 
                XmlExtensionLabel;
            OperatorDetails results =
                XmlFileIo.ReadXml<OperatorDetails>(
                    myPath);

            return results;
        }

        /// <summary>
        /// Serialise the <see cref="OperatorDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        public void Write(
            OperatorDetails file)
        {
            string myPath = 
                BasePathReader.GetBasePath() +
                StaticResources.classDetailsPath +
                StaticResources.FileNameOperator + 
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
                StaticResources.FileNameOperator +
                XmlExtensionLabel);
        }
    }
}