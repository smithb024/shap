namespace Shap.Interfaces.Config
{
    using Shap.Common.SerialiseModel.Family;

    /// <summary>
    /// Used to read and write to the family XML file.
    /// </summary>
    public interface IXmlFamilyIoController
    {
        /// <summary>
        /// Deserialise the <see cref="ClassDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <param name="filename">name of the file to read</param>
        /// <returns>deserialised file</returns>
        FamilyDetails Read(string filename);

        /// <summary>
        /// Serialise the <see cref="FamilyDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        void Write(FamilyDetails file);

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file exists flag</returns>
        bool DoesFileExist();
    }
}