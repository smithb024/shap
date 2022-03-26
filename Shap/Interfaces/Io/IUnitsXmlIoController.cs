namespace Shap.Interfaces.Io
{
    using Shap.Common.SerialiseModel.ClassDetails;

    /// <summary>
    /// Used to read and write to the uts config file.
    /// </summary>
    public interface IUnitsXmlIoController
    {
        /// <summary>
        /// Deserialise the <see cref="ClassDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">name of the file to read</param>
        /// <returns>deserialised file</returns>
        ClassDetails Read(string filename);

        /// <summary>
        /// Serialise the <see cref="ClassDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        void Write(
            ClassDetails file,
            string filename);

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file exists flag</returns>
        bool DoesFileExist(string fileName);
    }
}