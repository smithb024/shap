namespace Shap.Interfaces.ViewModels
{
    using NynaeveLib.Types;

    using Shap.Types.Enum;
    using Types;
    using NynaeveLib.ViewModel;

    /// <summary>
    /// Interface describing the view model a journey when it is being input
    /// </summary>
    public interface IJourneyViewModel : IViewModelBase
    {
        /// <summary>
        /// Gets the Jny Identifier
        /// </summary>
        IJnyId JnyId { get; }

        /// <summary>
        /// Gets the journey from details.
        /// </summary>
        string From { get; }

        /// <summary>
        /// Gets the journey to details.
        /// </summary>
        string To { get; }

        /// <summary>
        /// Gets the route details.
        /// </summary>
        string Route { get; }

        /// <summary>
        /// Gets the distance details.
        /// </summary>
        MilesChains Distance { get; }

        /// <summary>
        /// Gets the journey date.
        /// </summary>
        string JourneyDateString { get; }

        /// <summary>
        /// Gets the unit one id.
        /// </summary>
        string UnitOne { get; }

        /// <summary>
        /// Gets the unit two id.
        /// </summary>
        string UnitTwo { get; }

        /// <summary>
        /// Gets the unit three id.
        /// </summary>
        string UnitThree { get; }

        /// <summary>
        /// Gets the unit four id.
        /// </summary>
        string UnitFour { get; }

        /// <summary>
        /// Gets the journey from state.
        /// </summary>
        ComponentState FromState { get; }

        /// <summary>
        /// Gets the journey to state.
        /// </summary>
        ComponentState ToState { get; }

        /// <summary>
        /// Gets the unit one state.
        /// </summary>
        ComponentState UnitOneState { get; }

        /// <summary>
        /// Gets the unit two state.
        /// </summary>
        ComponentState UnitTwoState { get; }

        /// <summary>
        /// Gets the unit three state.
        /// </summary>
        ComponentState UnitThreeState { get; }

        /// <summary>
        /// Gets the unit four state.
        /// </summary>
        ComponentState UnitFourState { get; }

        /// <summary>
        /// Analyse the current states.
        /// </summary>
        void CalculateStates();
    }
}