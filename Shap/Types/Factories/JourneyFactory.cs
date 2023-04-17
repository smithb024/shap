namespace Shap.Types.Factories
{
    using System.Collections.Generic;
    using Interfaces.Stats;
    using Shap.Common.ViewModel;
    using Shap.Interfaces.Common.ViewModels;
    using Shap.Interfaces.Types;

    /// <summary>
    /// Factory class created to transfer between the classes <see cref="IJourneyDetailsType"/> and
    /// <see cref="IJourneyViewModel"/>.
    /// </summary>
    public static class JourneyFactory
    {
        /// <summary>
        /// Convert from a <see cref="IJourneyViewModel"/> to a <see cref="IJourneyDetailsType"/>.
        /// </summary>
        /// <param name="input"><see cref="IJourneyViewModel"/> to convert</param>
        /// <returns>converted <see cref="IJourneyDetailsType"/></returns>
        public static IJourneyDetailsType ToJourneyModel(IJourneyViewModel input)
        {
            List<string> units = new List<string>();
            JourneyFactory.Add(units, input.UnitOne);
            JourneyFactory.Add(units, input.UnitTwo);
            JourneyFactory.Add(units, input.UnitThree);
            JourneyFactory.Add(units, input.UnitFour);

            IJourneyDetailsType model =
              new JourneyDetailsType(
                input.JnyId.Date,
                input.JnyId.JnyNumber,
                input.From,
                input.To,
                input.Route,
                input.Distance,
                units);

            return model;
        }

        /// <summary>
        /// Convert from a <see cref="IJourneyDetailsType"/> to a <see cref="IJourneyViewModel"/>.
        /// </summary>
        /// <param name="input"><see cref="IJourneyDetailsType"/> to convert</param>
        /// <param name="firstExamples">first examples manager</param>
        /// <param name="parentNumber">parent unit id</param>
        /// <returns>converted <see cref="IJourneyViewModel"/></returns>
        public static IJourneyViewModel ToJourneyViewModel(
          IJourneyDetailsType input,
          IFirstExampleManager firstExamples,
          string parentNumber = "")
        {
            string unitOne = input.Units.Count > 0 ? input.Units[0] : string.Empty;
            string unitTwo = input.Units.Count > 1 ? input.Units[1] : string.Empty;
            string unitThree = input.Units.Count > 2 ? input.Units[2] : string.Empty;
            string unitFour = input.Units.Count > 3 ? input.Units[3] : string.Empty;

            JourneyViewModel viewModel =
              new JourneyViewModel(
                firstExamples,
                parentNumber,
                input.JnyId.JnyNumber,
                input.From,
                input.To,
                input.Via,
                input.JnyId.Date,
                input.Distance,
                unitOne,
                unitTwo,
                unitThree,
                unitFour);

            viewModel.CalculateStates();

            return viewModel;
        }

        /// <summary>
        /// Add <paramref name="newValue"/> to <paramref name="collection"/> if not null or whitespace.
        /// </summary>
        /// <param name="collection">collection of strings</param>
        /// <param name="newValue">value to add to the collection</param>
        private static void Add(
          List<string> collection,
          string newValue)
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                collection.Add(newValue);
            }
        }
    }
}