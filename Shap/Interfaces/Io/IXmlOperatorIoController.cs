namespace Shap.Interfaces.Io
{
    using Shap.Common.SerialiseModel.Operator;

    /// <summary>
    /// Used to read and write to the operator XML file.
    /// </summary>
    public interface IXmlOperatorIoController
    {
        /// <summary>
        /// Deserialise the <see cref="ClassDetails"/> from the <paramref name="filename"/>.
        /// </summary>
        /// <returns>deserialised file</returns>
        OperatorDetails Read();

        /// <summary>
        /// Serialise the <see cref="OperatorDetails"/> to <parmref name="filename"/>.
        /// </summary>
        /// <param name="file">file to serialise</param>
        /// <param name="filename">location to save the file to</param>
        void Write(OperatorDetails file);

        /// <summary>
        ///   Checks to see if a file exists.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file exists flag</returns>
        bool DoesFileExist();
    }
}