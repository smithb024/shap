namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Interfaces.ViewModels;
    using NynaeveLib.ViewModel;
    using Shap.Interfaces.Units;

    /// <summary>
    /// Delegate used to pass commands for a vehicle.
    /// </summary>
    /// <param name="vcle"></param>
    public delegate void VehicleDataDelegate(IUnitViewModel vcle);

    /// <summary>
    /// View model for the sub class.
    /// </summary>
    public class SubClassViewModel : ViewModelBase, ISubClassViewModel
    {
        /// <summary>
        /// The vehicle windows.
        /// </summary>
        private List<VehicleDataWindow> vcleWindows;

        /// <summary>
        /// The parent class.
        /// </summary>
        private ClassFunctionalViewModel parentClass;

        /// <summary>
        /// Initialises a new instance of the <see cref="SubClassViewModel"/> class.
        /// </summary>
        /// <param name="parent">parent view model</param>
        /// <param name="units">list of units</param>
        public SubClassViewModel(
          ClassFunctionalViewModel parent,
          ObservableCollection<IUnitViewModel> units)
        {
            this.parentClass = parent;
            this.Units = units;
            this.vcleWindows = new List<VehicleDataWindow>();

            foreach (IUnitViewModel unit in units)
            {
                this.SetUpEvents(unit);
            }
        }

        /// <summary>
        /// Gets the units in this sub class.
        /// </summary>
        public ObservableCollection<IUnitViewModel> Units { get; }

        /// <summary>
        /// Gets the alpha identifier for this <see cref="SubClassViewModel"/>.
        /// </summary>
        public string AlphaIdentifier => this.parentClass.ClassData.AlphaIdentifier;

        /// <summary>
        /// Set up all events
        /// </summary>
        /// <param name="newVehicle">vehicle containing the events</param>
        private void SetUpEvents(IUnitViewModel newVehicle)
        {
            newVehicle.OpenEvent += new VehicleDataDelegate(this.ShowVcleData);
            newVehicle.CloseEvent += new VehicleDataDelegate(this.CloseVcleData);
            newVehicle.NextEvent += new VehicleDataDelegate(this.NextUnit);
            newVehicle.PreviousEvent += new VehicleDataDelegate(this.PreviousUnit);
        }

        /// <summary>
        /// Set up and show a vehicle data window for a unit.
        /// </summary>
        /// <param name="unit">Unit to display</param>
        private void ShowVcleData(IUnitViewModel unit)
        {
            VehicleDataWindow window;

            if (this.vcleWindows.Exists(vw => vw.DataContext == unit))
            {
                window = this.vcleWindows.Find(vw => vw.DataContext == unit);
            }
            else
            {
                window = new VehicleDataWindow();
                window.DataContext = unit;

                if (unit.JourneysList != null)
                {
                    window.SetUpGraph(unit.JourneysList);
                }
                else
                {
                    window.SetUpGraph(new List<IJourneyViewModel>());
                }
                unit.ClosingRequest += this.CloseVcleDataWindow;
                window.Closed += this.VcleDataWindowClosed;

                this.vcleWindows.Add(window);
            }

            window.Show();
            window.Activate();
        }

        /// <summary>
        /// Close a vehicle data window.
        /// </summary>
        /// <param name="unit">unit to close</param>
        private void CloseVcleData(IUnitViewModel unit)
        {
            if (this.vcleWindows.Exists(vw => vw.DataContext == unit))
            {
                this.vcleWindows.Find(vw => vw.DataContext == unit).Close();
            }
        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseVcleDataWindow(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Form closed, set to null.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VcleDataWindowClosed(object sender, EventArgs e)
        {
            if (sender.GetType().Equals(typeof(VehicleDataWindow)))
            {
                this.vcleWindows.Remove((VehicleDataWindow)sender);
            }
        }

        /// <summary>
        /// Display the next unit
        /// </summary>
        /// <param name="unit">Unit to display</param>
        private void NextUnit(IUnitViewModel unit)
        {
            if (this.vcleWindows.Exists(vw => vw.DataContext == unit))
            {
                VehicleDataWindow window = this.vcleWindows.Find(vw => vw.DataContext == unit);

                int? index = this.GetUnitIndex(unit);

                if (index != null)
                {
                    window.DataContext = this.Units[(int)index + 1];
                }
            }
        }

        /// <summary>
        /// Display the previous unit.
        /// </summary>
        /// <param name="unit">Unit to display</param>
        private void PreviousUnit(IUnitViewModel unit)
        {
            if (this.vcleWindows.Exists(vw => vw.DataContext == unit))
            {
                VehicleDataWindow window = this.vcleWindows.Find(vw => vw.DataContext == unit);

                int? index = this.GetUnitIndex(unit);

                if (index != null)
                {
                    window.DataContext = this.Units[(int)index - 1];
                }
            }
        }

        /// <summary>
        /// Find and return the index of a unit.
        /// </summary>
        /// <param name="unit">unit to search for</param>
        /// <returns>found index</returns>
        private int? GetUnitIndex(IUnitViewModel unit)
        {
            for (int index = 0; index < this.Units?.Count; ++index)
            {
                if (this.Units[index] == unit)
                {
                    return index;
                }
            }

            return null;
        }
    }
}