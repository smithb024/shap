namespace Shap.Types.Enum
{
    /// <summary>
    /// Converter class for the <see cref="LocationCategories"/> enum.
    /// </summary>
    public static class LocationCategoriesConverter
    {
        /// <summary>
        /// Convert a <see cref="LocationCategories"/> value.
        /// </summary>
        /// <param name="input">
        /// Input value
        /// </param>
        /// <returns>return string</returns>
        public static string Convert(LocationCategories input)
        {
            switch (input)
            {
                case LocationCategories.A:
                    return "Hub";

                case LocationCategories.B:
                    return "Regional";

                case LocationCategories.C1:
                    return "Feeder City/Jn";

                case LocationCategories.C2:
                    return "Feeder Other";

                case LocationCategories.D:
                    return "Medium";

                case LocationCategories.E:
                    return "Small";

                case LocationCategories.F:
                    return "Unstaffed";

                case LocationCategories.ND:
                    return "Not Defined";

                default:
                    return "Not Defined";
            }
        }
    }
}
