namespace Shap.Locations.IO
{
    using System.IO;
    using Shap.Common.Factories;
    using Shap.Common;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;

    /// <summary>
    /// Used to read and write to the location XML file.
    /// </summary>
    public class LocationIoController : ILocationIoController
    {
        /// <summary>
        /// Extension for the location filename.
        /// </summary>
        private const string XmlExtensionLabel = ".xml";

        /// <summary>
        /// Deserialise the <see cref="LocationDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">The name of the location</param>
        /// <returns>deserialised file</returns>
        public LocationDetails Read(string filename)
        {
            string myPath =
            BasePathReader.GetBasePath() +
            StaticResources.classDetailsPath +
            filename +
            XmlExtensionLabel;
            LocationDetails results =
                XmlFileIo.ReadXml<LocationDetails>(
                    myPath);

            return results;
        }

        /// <summary>
        /// Serialise the <see cref="LocationDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">The name of the location</param>
        public void Write(
            LocationDetails file,
            string filename)
        {
            string myPath =
               BasePathReader.GetBasePath() +
               StaticResources.classDetailsPath +
               filename +
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