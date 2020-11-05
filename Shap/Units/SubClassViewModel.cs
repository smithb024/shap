using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shap.Units
{
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Windows.Input;

  using NynaeveLib.ViewModel;

  using Shap.Interfaces.Units;
  using Shap.Types;

  public delegate void VehicleDataDelegate(IUnitViewModel vcle);

  public class SubClassViewModel : ViewModelBase, ISubClassViewModel
  {
    private List<VehicleDataWindow> vcleWindows;
    private ClassFunctionalViewModel parentClass;

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

    public ObservableCollection<IUnitViewModel> Units { get; }

    /// <summary>
    /// Gets the alpha identifier for this <see cref="SubClassViewModel"/>.
    /// </summary>
    public string AlphaIdentifier => this.parentClass.ClassData.AlphaIdentifier;

    private void SetUpEvents(IUnitViewModel newVehicle)
    {
      newVehicle.OpenEvent += new VehicleDataDelegate(this.ShowVcleData);
      newVehicle.CloseEvent += new VehicleDataDelegate(this.CloseVcleData);
      newVehicle.NextEvent += new VehicleDataDelegate(this.NextUnit);
      newVehicle.PreviousEvent += new VehicleDataDelegate(this.PreviousUnit);
    }

    //public void AddNewVcles(IUnitViewModel newVehicle)
    //{
      
    //  newVehicle.OpenEvent += new VehicleDataDelegate(this.ShowVcleData);
    //  newVehicle.CloseEvent += new VehicleDataDelegate(this.CloseVcleData);
    //  newVehicle.NextEvent += new VehicleDataDelegate(this.NextUnit);
    //  newVehicle.PreviousEvent += new VehicleDataDelegate(this.PreviousUnit);

    //  this.Units.Add(newVehicle);
    //}

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

        unit.ClosingRequest += this.CloseVcleDataWindow;
        window.Closed += this.VcleDataWindowClosed;

        this.vcleWindows.Add(window);
      }

      window.Show();
      window.Activate();
    }

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
    /// 
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
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
