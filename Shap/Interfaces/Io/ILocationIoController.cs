namespace Shap.Interfaces.Io
{
    using Shap.Common.SerialiseModel.Location;
    using Shap.Locations.Model;
    using System.Collections.Generic;

    /// <summary>
    /// Used to read and write to the location XML file.
    /// </summary>
    public interface ILocationIoController
    {
        /// <summary>
        /// Deserialise the <see cref="LocationDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">The name of the location</param>
        /// <returns>deserialised file</returns>
        LocationDetails Read(string filename);

        /// <summary>
        /// Serialise the <see cref="LocationDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">The name of the location</param>
        void Write(
            LocationDetails file,
            string filename);

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="filename">The name of the location</param>
        /// <returns>file exists flag</returns>
        bool DoesFileExist(string filename);

        /// <summary>
        /// Returns all the files in location image Path.
        /// </summary>
        /// <returns>list of image names</returns>
        List<string> GetImageFileList();

        /// <summary>
        /// Get all the regions from the config file.
        /// </summary>
        /// <returns>
        /// A list of all regions.
        /// </returns>
        List<string> GetRegions();

        /// <summary>
        /// Get all the lines by reading the names of all files.
        /// </summary>
        /// <returns>
        /// A list of all lines.
        /// </returns>
        List<string> GetLines();

        /// <summary>
        /// Read the details of a specific line.
        /// </summary>
        /// <returns>
        /// All details of a specific line
        /// </returns>
        List<LineDetail> ReadLine(
            string filename);
    }
}