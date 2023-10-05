namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;

    /// <summary>
    /// Message class, used to pass a set of known groups/classes between view models.
    /// </summary>
    public class GroupsListMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="GroupsListMessage"/> class.
        /// </summary>
        /// <param name="groups"></param>
        public GroupsListMessage(
            List<string> groups)
        {
            this.Groups = groups;
        }

        /// <summary>
        /// Gets the collection of all known groups/classes.
        /// </summary>
        public List<string> Groups { get; }
    }
}