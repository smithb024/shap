namespace Shap.Config.GroupsAndClasses
{
    using System.Collections.Generic;
    using Shap.Types;

    /// <summary>
    /// Static class which is used to decode the groups and classes.
    /// </summary>
    public static class GacDecoder
    {
        /// <summary>
        /// Find the name of the <see cref="GroupsType"/> which contains the 
        /// <paramref name="unitId"/>.
        /// </summary>
        /// <param name="unitId">The Id to search for.</param>
        /// <param name="groups">The collection of all <see cref="GroupsType"/></param>
        /// <returns>The name of the found <see cref="GroupsType"/> object.</returns>
        public static string GetClass(
            string unitId,
            List<GroupsType> groups)
        {
            string unitClass;
            int index = unitId.IndexOfAny("0123456789".ToCharArray());

            if (index == 0)
            {
                unitClass =
                    GacDecoder.GetClassFromId(
                        unitId,
                        groups);
            }
            else
            {
                string alphaId = unitId.Substring(0, index);
                unitClass =
                    GacDecoder.StringGetClassFromAlphaId(
                        alphaId,
                        groups);
            }

            return unitClass;
        }

        /// <summary>
        /// Gets the name from the <see cref="GroupsType"/> which contains the
        /// <paramref name="unitId"/>. The ID is assumed to be a pure integer,
        /// so will be found within the range provided by the
        /// <see cref="GroupsType"/>.
        /// </summary>
        /// <param name="unitId">The Id to search for</param>
        /// <param name="groups">The collection of all <see cref="GroupsType"/></param>
        /// <returns>The name of the found group.</returns>
        private static string GetClassFromId(
            string unitId,
            List<GroupsType> groups)
        {

            if (!int.TryParse(unitId, out int id))
            {
                return string.Empty;
            }

            foreach (GroupsType group in groups)
            {
                if (group.Name.Contains("Fam"))
                {
                    continue;
                }

                foreach (GroupBoundsType bounds in group.Bounds)
                {
                    if (id >= bounds.LowerBound &&
                        id <= bounds.UpperBound)
                    {
                        return group.Name;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name from the <see cref="GroupsType"/> which contains the 
        /// <paramref name="alphaId"/>.
        /// </summary>
        /// <param name="alphaId">
        /// The alpha Id to look for.
        /// </param>
        /// <param name="groups">All the <see cref="GroupsType"/> objects</param>
        /// <returns>
        /// The name of the class which contains the <paramref name="alphaId"/>.
        /// </returns>
        private static string StringGetClassFromAlphaId(
            string alphaId,
            List<GroupsType> groups)
        {
            foreach (GroupsType group in groups)
            {
                if (group.Name.Contains("Fam"))
                {
                    continue;
                }

                if (group.AlphaIds.Contains(alphaId))
                {
                    return group.Name;
                }
            }

            return string.Empty;
        }
    }
}