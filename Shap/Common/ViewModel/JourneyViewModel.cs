namespace Shap.Common.ViewModel
{
    using System;

    using Interfaces.Common.ViewModels;
    using Interfaces.Stats;
    using Interfaces.Types;

    using NynaeveLib.Types;
    using NynaeveLib.ViewModel;
    using Shap.Types;
    using Shap.Types.Enum;
    using Stats;

    /// <summary>
    /// Journey View Model
    /// </summary>
    public class JourneyViewModel : ViewModelBase, IJourneyViewModel
    {
        /// <summary>
        /// Link to the first examples.
        /// </summary>
        private IFirstExampleManager firstExamples;

        /// <summary>
        /// Id of the parent unit, only relevent if this jny is recorded against a specfic unit. 
        /// </summary>
        private string parentUnit;

        /// <summary>
        /// Initialises a new instance of the <see cref="JourneyViewModel"/> class.
        /// </summary>
        /// <param name="modelJourney"></param>
        /// <param name="firstExamples"></param>
        public JourneyViewModel(
          IFirstExampleManager firstExamples,
          string parentUnit,
          string journeyNumber,
          string from,
          string to,
          string route,
          DateTime date,
          MilesChains distance,
          string unitOne,
          string unitTwo = "",
          string unitThree = "",
          string unitFour = "")
        {
            this.firstExamples = firstExamples;
            this.parentUnit = parentUnit;

            this.JnyId =
              new JnyId(
                date,
                journeyNumber);
            this.From = from;
            this.To = to;
            this.Route = route;
            this.Distance = distance;
            this.UnitOne = unitOne;
            this.UnitTwo = unitTwo;
            this.UnitThree = unitThree;
            this.UnitFour = unitFour;
            this.FromState = ComponentState.Unknown;
            this.ToState = ComponentState.Unknown;
            this.UnitOneState = ComponentState.Unknown;
            this.UnitTwoState = ComponentState.Unknown;
            this.UnitThreeState = ComponentState.Unknown;
            this.UnitFourState = ComponentState.Unknown;

        }

        /// <summary>
        /// Gets the Jny Identifier
        /// </summary>
        public IJnyId JnyId { get; }

        /// <summary>
        /// Gets the journey from details.
        /// </summary>
        public string From { get; }

        /// <summary>
        /// Gets the journey to details.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Gets the route details.
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Gets the distance details.
        /// </summary>
        public MilesChains Distance { get; }

        /// <summary>
        /// Gets the journey date.
        /// </summary>
        public string JourneyDateString => this.JnyId.Date.ToString("dd/MM/yyyy");

        /// <summary>
        /// Gets the unit one id.
        /// </summary>
        public string UnitOne { get; }

        /// <summary>
        /// Gets the unit two id.
        /// </summary>
        public string UnitTwo { get; }

        /// <summary>
        /// Gets the unit three id.
        /// </summary>
        public string UnitThree { get; }

        /// <summary>
        /// Gets the unit four id.
        /// </summary>
        public string UnitFour { get; }

        /// <summary>
        /// Gets the journey from state.
        /// </summary>
        public ComponentState FromState { get; private set; }

        /// <summary>
        /// Gets the journey to state.
        /// </summary>
        public ComponentState ToState { get; private set; }

        /// <summary>
        /// Gets the unit one state.
        /// </summary>
        public ComponentState UnitOneState { get; private set; }

        /// <summary>
        /// Gets the unit two state.
        /// </summary>
        public ComponentState UnitTwoState { get; private set; }

        /// <summary>
        /// Gets the unit three state.
        /// </summary>
        public ComponentState UnitThreeState { get; private set; }

        /// <summary>
        /// Gets the unit four state.
        /// </summary>
        public ComponentState UnitFourState { get; private set; }

        /// <summary>
        /// Analyse the current states.
        /// </summary>
        public void CalculateStates()
        {
            this.FromState =
              this.CalculateLocationState(
                this.From,
                this.JnyId,
                this.firstExamples);
            this.ToState =
              this.CalculateLocationState(
                this.To,
                this.JnyId,
                this.firstExamples);

            this.UnitOneState =
              this.CalculateUnitState(
                this.UnitOne,
                this.JnyId,
                this.firstExamples,
                this.parentUnit);
            this.UnitTwoState =
              this.CalculateUnitState(
                this.UnitTwo,
                this.JnyId,
                this.firstExamples,
                this.parentUnit);
            this.UnitThreeState =
              this.CalculateUnitState(
                this.UnitThree,
                this.JnyId,
                this.firstExamples,
                this.parentUnit);
            this.UnitFourState =
              this.CalculateUnitState(
                this.UnitFour,
                this.JnyId,
                this.firstExamples,
                this.parentUnit);

            this.RaisePropertyChangedEvent(nameof(this.FromState));
            this.RaisePropertyChangedEvent(nameof(this.ToState));
            this.RaisePropertyChangedEvent(nameof(this.UnitOneState));
            this.RaisePropertyChangedEvent(nameof(this.UnitTwoState));
            this.RaisePropertyChangedEvent(nameof(this.UnitThreeState));
            this.RaisePropertyChangedEvent(nameof(this.UnitFourState));
        }

        /// <summary>
        /// Calculate the location state
        /// </summary>
        /// <param name="location">location to check</param>
        /// <param name="id">journey id</param>
        /// <param name="firstExamples">first example manager</param>
        /// <returns>current location state</returns>
        private ComponentState CalculateLocationState(
          string location,
          IJnyId id,
          IFirstExampleManager firstExamples)
        {
            if (firstExamples.IsCopLocation(location, id, LocalListType.complete))
            {
                return ComponentState.Cop;
            }
            else if (firstExamples.IsCopLocation(location, id, LocalListType.annual))
            {
                return ComponentState.CopYear;
            }
            else
            {
                return ComponentState.None;
            }
        }

        /// <summary>
        /// Calculate the unit state
        /// </summary>
        /// <param name="unit">unit to check</param>
        /// <param name="id">journey id</param>
        /// <param name="firstExamples">first example manager</param>
        /// <param name="parent">
        /// Used where this journey is set against a specific unit. This is the id of that unit
        /// </param>
        /// <returns>current unit state</returns>
        private ComponentState CalculateUnitState(
          string unit,
          IJnyId id,
          IFirstExampleManager firstExamples,
          string parent)
        {
            if (string.Compare(unit, parent) == 0)
            {
                return ComponentState.CurrentUnit;
            }

            if (firstExamples.IsCopNumber(unit, id, LocalListType.complete))
            {
                return ComponentState.Cop;
            }
            else if (firstExamples.IsCopNumber(unit, id, LocalListType.annual))
            {
                return ComponentState.CopYear;
            }
            else
            {
                return ComponentState.None;
            }
        }
    }
}