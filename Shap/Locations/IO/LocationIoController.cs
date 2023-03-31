namespace Shap.Locations.IO
{
    using System.IO;
    using Shap.Common.Factories;
    using Shap.Common;
    using Shap.Common.SerialiseModel.Location;
    using Shap.Interfaces.Io;
    using System.Collections.Generic;
    using NynaeveLib.Logger;
    using Shap.Types;
    using System.Windows.Shapes;
    using System.Linq;

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
        /// Extension for the regiosn filename.
        /// </summary>
        private const string TxtExtensionLabel = ".txt";

        /// <summary>
        /// Deserialise the <see cref="LocationDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">The name of the location</param>
        /// <returns>deserialised file</returns>
        public LocationDetails Read(string filename)
        {
            string myPath =
                BasePathReader.GetBasePath() +
                StaticResources.locIdvlPath +
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
               StaticResources.locIdvlPath +
               filename +
               XmlExtensionLabel;

            XmlFileIo.WriteXml(
                file,
                myPath);
        }

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="filename">The name of the location</param>
        /// <returns>file exists flag</returns>
        public bool DoesFileExist(string filename)
        {
            return File.Exists(
             BasePathReader.GetBasePath() +
             StaticResources.locIdvlPath +
             filename +
             XmlExtensionLabel);
        }

        /// <summary>
        /// Returns all the files in location image Path.
        /// </summary>
        /// <returns>list of image names</returns>
        public List<string> GetImageFileList()
        {
            List<string> imageFileNameList = new List<string>();
            string[] fileNamesArray =
                Directory.GetFiles(
                    BasePathReader.GetBasePath() + StaticResources.locImgPath);

            foreach (string file in fileNamesArray)
            {
                string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                imageFileNameList.Add(fileName);
            }

            return imageFileNameList;
        }

        /// <summary>
        /// Get all the regions from the config file.
        /// </summary>
        /// <returns>
        /// A list of all regions.
        /// </returns>
        public List<string> GetRegions()
        {
            List<string> regions = new List<string>();

            string regionsPath =
               BasePathReader.GetBasePath() +
               StaticResources.locPath +
               StaticResources.FileNameRegions +
               TxtExtensionLabel;

            if (!File.Exists(regionsPath))
            {
                return regions;
            }

            using (StreamReader reader = new StreamReader(regionsPath))
            {
                string currentLine = string.Empty;
                currentLine = reader.ReadLine();

                while (currentLine != null)
                {
                    regions.Add(currentLine);
                    currentLine = reader.ReadLine();
                }
            }

            regions.Sort();

            return regions;
        }
    }
}