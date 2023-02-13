namespace Shap.Interfaces.Input
{
    using NynaeveLib.Types;
    using Shap.Input;
    using Shap.Interfaces.ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public interface IInputFormViewModel
    {
        /// <summary>
        /// view close request event handler
        /// </summary>
        event EventHandler ClosingRequest;

        ICommand AddJnyCmd { get; }
        ICommand CloseCmd { get; }
        DateTime Date { get; set; }
        int Day { get; }
        MilesChains DayDistance { get; }
        string DayDistanceString { get; }
        ObservableCollection<DayViewModel> Days { get; }
        string FirstVehicle { get; set; }
        string FourthVehicle { get; set; }
        MilesChains JnyDistance { get; }
        string JnyDistanceString { get; }
        int JnyFromIndex { get; set; }
        ObservableCollection<string> JnyFromList { get; set; }
        ObservableCollection<IJourneyViewModel> JnyList { get; set; }
        int JnyNumber { get; set; }
        int JnyToIndex { get; set; }
        ObservableCollection<string> JnyToList { get; set; }
        int JnyViaIndex { get; set; }
        ObservableCollection<string> JnyViaList { get; set; }
        int Month { get; }
        ICommand NextDayCmd { get; }
        int NumberOfVehicles { get; }
        ICommand PreviousDayCmd { get; }
        ICommand SaveCmd { get; }
        string SecondVehicle { get; set; }
        string Status { get; set; }
        string ThirdVehicle { get; set; }
        int Year { get; }
    }
}