namespace Shap.Locations.Model
{
    using NynaeveLib.Logger;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class which stores the detail of a single row of the line file.
    /// </summary>
    /// <remarks>
    /// The line file is a csv file with two sections, the first is a string of icon codes and
    /// the second is a location name. 
    /// </remarks>
    public class LineDetail
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LineDetail"/> class.
        /// </summary>
        /// <param name="detail">
        /// The line detail to translate and store.
        /// </param>
        public LineDetail(
            string detail)
        {
            string[] details = detail.Split(',');

            if (details.Length != 2)
            {
                Logger.Instance.WriteLog($"Line detail {detail} is invalid");
            }

            this.Location = detail.Length > 1 ? details[1] : string.Empty;
            this.CodesString = details[0];

            this.Codes = new List<string>();
            foreach (char c in this.CodesString)
            {
                this.Codes.Add(c.ToString());
            }
        }

        /// <summary>
        /// Gets the location value.
        /// </summary>
        public string Location { get; }

        /// <summary>
        /// Gets the raw string of codes.
        /// </summary>
        public string CodesString { get; }

        /// <summary>
        /// Gets the icon codes.
        /// </summary>
        public List<string> Codes { get; }

        /// <summary>
        /// Gets the number of icons to display
        /// </summary>
        public int Count => this.Codes.Count;
    }
}