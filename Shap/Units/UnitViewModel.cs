namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Interfaces.Stats;
    using Shap.Common.Commands;
    using Factories;
    using Shap.Interfaces.Types;
    using Shap.Interfaces.Units;
    using Shap.Interfaces.ViewModels;
    using Shap.Types;
    using Shap.Types.Factories;
    using Shap.Units.IO;

    /// <summary>
    /// View model for a single unit. 
    /// </summary>
    public class UnitViewModel : VehicleDetailsViewModel, IUnitViewModel
    {
        /// <summary>
        /// Action to run to save this <see cref="UnitViewModel"/> to file.
        /// </summary>
        private Func<IUnitViewModel, string, bool> saveAction;

        /// <summary>
        /// First Example manager class.
        /// </summary>
        private IFirstExampleManager firstExamples;

        /// <summary>
        /// Event used to indicate to the parent <see cref="SubClassViewModel"/> that an open unit data
        /// window has been requested.
        /// </summary>
        public event VehicleDataDelegate OpenEvent;

        /// <summary>
        /// Event used to indicate to the parent <see cref="SubClassViewModel"/> that a close unit data
        /// window has been requested. It should only close a window pertaining to this unit.
        /// </summary>
        public event VehicleDataDelegate CloseEvent;

        /// <summary>
        /// Event used to inform the parent <see cref="SubClassViewModel"/> that the previous unit in the
        /// list has been selected for this <see cref="UnitViewModel"/>.
        /// </summary>
        public event VehicleDataDelegate PreviousEvent;

        /// <summary>
        /// Event used to inform the parent <see cref="SubClassViewModel"/> that the next unit in the
        /// list has been selected for this <see cref="UnitViewModel"/>.
        /// </summary>
        public event VehicleDataDelegate NextEvent;

        ///// <summary>
        ///// Maintains the index of the currently selected in service state from the
        ///// <see cref="ServiceTypeList"/> list of in service states.
        ///// </summary>
        //private int serviceIndex;

        /// <summary>
        /// This is the name of the parent class.
        /// </summary>
        private string className;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitViewModel"/> class.
        /// </summary>
        /// <param name="individualUnitIoController">Unit IO manager</param>
        /// <param name="className">name of the parent class.</param>
        public UnitViewModel(
          Func<IUnitViewModel, string, bool> saveAction,
          IFirstExampleManager firstExamples,
          string className,
          IndividualUnitFileContents rawData,
          bool isFirst,
          bool isLast,
          string imagePath,
          string alphaIdentifier)
          : base(rawData, firstExamples)
        {
            this.saveAction = saveAction;
            this.firstExamples = firstExamples;

            this.OpenWindowCmd = new CommonCommand(this.OpenWindow, () => true);
            this.PreviousUnitCmd = new CommonCommand(this.PreviousUnit, this.PreviousUnitAvailable);
            this.NextUnitCmd = new CommonCommand(this.NextUnit, this.NextUnitAvailable);
            this.UpdateUnitCmd = new CommonCommand(this.UpdateUnit, this.UpdateCmdAvailable);
            this.RefreshUnitCmd = new CommonCommand(this.RefreshUnit, () => true);
            this.SaveUnitCmd = new CommonCommand(this.SaveUnit, () => true);
            this.CloseWindowCmd = new CommonCommand(this.CloseWindow, () => true);

            this.FirstUnit = isFirst;
            this.LastUnit = isLast;
            this.SubClassImagePath = imagePath;
            this.AlphaIdentifier = alphaIdentifier;
            this.className = className;

            this.CompleteUpdate();
        }

        /// <summary>
        /// Command to open a new <see cref="VehicleDataWindow"/> and populate it with the details 
        /// from this view model.
        /// </summary>
        public ICommand OpenWindowCmd { get; }

        /// <summary>
        /// Command to populate the current <see cref="VehicleDataWindow"/> with the details of the 
        /// previous unit in the sub class list.
        /// </summary>
        public ICommand PreviousUnitCmd { get; }

        /// <summary>
        /// Command to populate the current <see cref="VehicleDataWindow"/> with the details of the 
        /// next unit in the sub class list.
        /// </summary>
        public ICommand NextUnitCmd { get; }

        /// <summary>
        /// Not currently implemented.
        /// </summary>
        public ICommand UpdateUnitCmd { get; }

        /// <summary>
        /// Command to refresh the current from the database data.
        /// </summary>
        public ICommand RefreshUnitCmd { get; }

        /// <summary>
        /// Command to save the unit details.
        /// </summary>
        public ICommand SaveUnitCmd { get; }

        /// <summary>
        /// Command to close the <see cref="VehicleDataWindow"/> which is showing details of this view
        /// model.
        /// </summary>
        public ICommand CloseWindowCmd { get; }

        /// <summary>
        /// 
        /// </summary>
        public string AlphaIdentifier { get; }

        /// <summary>
        /// Inidicates whether this is the first unit in the list
        /// </summary>
        public bool FirstUnit { get; set; }

        /// <summary>
        /// Indicates whether this is the last unit in the list.
        /// </summary>
        public bool LastUnit { get; set; }

        ///// <summary>
        ///// Gets the path to the image for this <see cref="UnitViewModel"/>
        ///// </summary>
        //public string ImagePath { get; }

        /// <summary>
        /// Gets the path to the sub class image.
        /// </summary>
        public string SubClassImagePath { get; }

        /// <summary>
        /// Gets the unit number to display (Includes alpha identifier if one present).
        /// </summary>
        public string DisplayUnitNumber => $"{this.AlphaIdentifier}{this.UnitNumber}";

        ///// <summary>
        ///// Gets the alpha identifier.
        ///// </summary>
        //public string AlphaIdentifier { get; }

        ///// <summary>
        ///// Gets the current status of the unit.
        ///// </summary>
        //public override VehicleServiceType Status
        //{
        //  get
        //  {
        //    return this.ServiceTypeList[this.ServiceIndex];
        //  }
        //}

        ///// <summary>
        ///// Gets a collection containing all the enumerations in <see cref="VehicleServiceType"/>.
        ///// </summary>
        //public List<VehicleServiceType> ServiceTypeList =>
        //  Enum.GetValues(typeof(VehicleServiceType)).
        //  Cast<VehicleServiceType>().
        //  ToList();

        ///// <summary>
        ///// Gets or sets the index of the currently selected in service state from the
        ///// <see cref="ServiceTypeList"/> list of in service states.
        ///// </summary>
        //public int ServiceIndex
        //{
        //  get
        //  {
        //    return this.serviceIndex;
        //  }

        //  set
        //  {
        //    this.serviceIndex = value;
        //    this.RaisePropertyChangedEvent(nameof(ServiceIndex));
        //    this.RaisePropertyChangedEvent(nameof(Status));
        //  }
        //}

        /// <summary>
        /// Refresh the units data.
        /// </summary>
        public void RefreshUnit()
        {
            this.JourneysList = new List<IJourneyViewModel>();

            // TODO new each time. Really>?
            //Searcher searcher = new Searcher();

            SearcherResults results =
            Searcher.RunCompleteSearch(
              this.DisplayUnitNumber,
              this.FormerNumbers.FormerNumbers);

            //List<IJourneyDetailsType> rawData = searcher.FoundJourneys;

            foreach (IJourneyDetailsType jny in results.FoundJourneys)
            {
                IJourneyViewModel journey =
                  JourneyFactory.ToJourneyViewModel(
                    jny,
                    this.firstExamples,
                    this.UnitNumber);

                this.AddJourney(journey);
            }

            this.UnitLastCheck = results.LastDateChecked;

            //this.RefreshUnitDistance();
            this.CompleteUpdate();

            this.SaveUnit();
        }

        private void OpenWindow()
        {
            this.OpenEvent(this);
        }

        private void CloseWindow()
        {
            this.CloseEvent(this);
        }

        private void PreviousUnit()
        {
            this.PreviousEvent(this);
        }

        private void NextUnit()
        {
            this.NextEvent(this);
        }

        //private bool OpenCmdAvailable()
        //{
        //  return true;
        //}

        private bool PreviousUnitAvailable()
        {
            return !this.FirstUnit;
        }

        private bool NextUnitAvailable()
        {
            return !this.LastUnit;
        }

        private void UpdateUnit()
        {
            // TODO Deal with this much later.
        }

        private bool UpdateCmdAvailable()
        {
            return false;
        }

        //private bool RefreshCmdAvailable()
        //{
        //  return true;
        //}

        private void SaveUnit()
        {
            this.saveAction.Invoke(this, className);
            //IndividualUnitIOController individualUnitController = IndividualUnitIOController.GetInstance();
            //this.individualUnitIoController.WriteIndividualUnitFile(this, className);
        }

        //private bool SaveCmdAvailable()
        //{
        //  return true;
        //}

        private new void CompleteUpdate()
        {
            this.RaisePropertyChangedEvent(nameof(this.DisplayUnitNumber));

            base.CompleteUpdate();
        }
    }
}