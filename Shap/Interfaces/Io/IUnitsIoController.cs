namespace Shap.Interfaces.Io
{
    using System.Collections.Generic;

    public interface IUnitsIoController
    {
        /// <summary>
        ///   Reads the contents of class list and put it in an array. Returns
        ///     true if successful
        /// </summary>
        List<string> GetClassList();

        /// <summary>
        ///   Returns all the files in class image Path.
        /// </summary>
        List<string> GetImageFileList();   }
}