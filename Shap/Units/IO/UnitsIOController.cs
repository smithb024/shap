namespace Shap.Units.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NynaeveLib.Logger;
    using Shap.Common;
    using Shap.Interfaces.Io;

    public class UnitsIOController : IUnitsIoController
    {
        /// <summary>
        /// The base path for the configuration files.
        /// </summary>
        private readonly string basePath = string.Empty;

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>UnitsIOController</name>
        /// <date>28/04/12</date>
        /// <summary>
        ///  Creates a new instance of the UnitsIOController class.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public UnitsIOController()
        {
            this.basePath = BasePathReader.GetBasePath();
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ReadClassList</name>
        /// <date>28/04/12</date>
        /// <summary>
        ///   Reads the contents of class list and put it in an array. Returns
        ///     true if successful
        /// </summary>
        /// <returns>success flag</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public List<string> GetClassList()
        {
            List<string> classList = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(basePath + StaticResources.classDetailsPath + "classlist.txt"))
                {
                    string currentLine = string.Empty;
                    currentLine = reader.ReadLine();
                    while (currentLine != null)
                    {
                        classList.Add(currentLine);

                        currentLine = reader.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger logger = Logger.Instance;
                logger.WriteLog(
                  "ERROR: Error reading classlist.txt. Error is " + ex.ToString());

                return new List<string>();
            }

            return classList;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>GetImageFileList</name>
        /// <date>13/12/12</date>
        /// <summary>
        ///   Returns all the files in class image Path.
        /// </summary>
        /// <returns>list of image names</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public List<string> GetImageFileList()
        {
            // TODO, does this really belong here? It gets images for subclasses
            List<string> imageFileNameList = new List<string>();
            string[] fileNamesArray =
                Directory.GetFiles(
                    this.basePath + StaticResources.classImgPath);

            foreach (string file in fileNamesArray)
            {
                string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
                imageFileNameList.Add(fileName);
            }

            return imageFileNameList;
        }
    }
}