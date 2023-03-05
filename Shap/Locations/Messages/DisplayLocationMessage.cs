using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Locations.Messages
{
    /// <summary>
    /// Message class used to request that a new location is displayed.
    /// </summary>
    public class DisplayLocationMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DisplayLocationMessage"/> class.
        /// </summary>
        /// <param name="location"></param>
        public DisplayLocationMessage(string location)
        {
            this.Location = location;
        }

        /// <summary>
        /// Gets the location to display.
        /// </summary>
        /// <remarks>
        /// Use an empty string to clear.
        /// </remarks>
        public string Location { get; }
    }
}
