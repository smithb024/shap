namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Interfaces.ViewModels;
    using NynaeveLib.Logger;
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
        /// Initialises a new instance of the <see cref="SubClassViewModel"/> class.
        /// </summary>
        /// <param name="alphaIdentifier">
        /// The alpha identifier for this <see cref="SubClassViewModel"/>
        /// </param>
        /// <param name="units">list of units</param>
        public SubClassViewModel(
            string alphaIdentifier,
            ObservableCollection<IUnitViewModel> units)
        {
            this.AlphaIdentifier = alphaIdentifier;
            this.Units = units;
            this.vcleWindows = new List<VehicleDataWindow>();

            foreach (IUnitViewModel unit in this.Units)
            {
                unit.OpenEvent += new VehicleDataDelegate(this.ShowVcleData);
                unit.CloseEvent += new VehicleDataDelegate(this.CloseVcleData);
                unit.NextEvent += new VehicleDataDelegate(this.NextUnit);
                unit.PreviousEvent += new VehicleDataDelegate(this.PreviousUnit);
            }
        }

        /// <summary>
        /// Gets the units in this sub class.
        /// </summary>
        public ObservableCollection<IUnitViewModel> Units { get; }

        /// <summary>
        /// Gets the alpha identifier for this <see cref="SubClassViewModel"/>.
        /// </summary>
        public string AlphaIdentifier { get; }

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
                window =
                    new VehicleDataWindow
                    {
                        DataContext = unit
                    };

                if (unit.JourneysList != null)
                {
                    window.SetUpGraph(unit.JourneysList);
                }
                else
                {
                    window.SetUpGraph(new List<IJourneyViewModel>());
                }
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
        /// <param name="sender">vehicle data window</param>
        /// <param name="e">event args</param>
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
        /// <param name="unit">the unit from where the request originated</param>
        private void NextUnit(IUnitViewModel unit)
        {
            int? index = this.GetUnitIndex(unit);

            if (index != null &&
                index < this.Units.Count - 1) 
            {
                VehicleDataWindow window = this.vcleWindows.Find(vw => vw.DataContext == unit);

                this.ChangeWindowUnit(
                    window,
                    this.Units[(int)index + 1]);
            }
        }

        /// <summary>
        /// Display the previous unit.
        /// </summary>
        /// <param name="unit">Unit to display</param>
        private void PreviousUnit(IUnitViewModel unit)
        {
            int? index = this.GetUnitIndex(unit);

            if (index != null &&
                index > 0)
            {
                VehicleDataWindow window = this.vcleWindows.Find(vw => vw.DataContext == unit);

                this.ChangeWindowUnit(
                    window,
                    this.Units[(int)index - 1]);
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

        /// <summary>
        /// Change the data context of an existing window to display the indicated unit.
        /// Update the graphs.
        /// </summary>
        /// <remarks>
        /// The command is rejected if there is a window which already displays the indicated unit.
        /// In that case, the other window is focused.
        /// </remarks>
        /// <param name="window">Window to change</param>
        /// <param name="unit">Unit to display on the window</param>
        private void ChangeWindowUnit(
            VehicleDataWindow window,
            IUnitViewModel unit)
        {
            // Focus on an existing window if one already exists.
            if (this.vcleWindows.Exists(vw => vw.DataContext == unit))
            {
                VehicleDataWindow existingWindow =
                    this.vcleWindows.Find(
                        vw => vw.DataContext == unit);
                existingWindow.Show();
                existingWindow.Activate();

                return;
            }

            if (window == null)
            {
                Logger.Instance.WriteLog("SubClassViewModel: failed to change unit in a window, failed to find valid window");
                return;
            }

            window.DataContext = unit;

            if (unit.JourneysList != null)
            {
                window.SetUpGraph(unit.JourneysList);
            }
            else
            {
                window.SetUpGraph(new List<IJourneyViewModel>());
            }
        }
    }
}