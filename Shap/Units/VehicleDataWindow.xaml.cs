namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using NynaeveLib.Types;
    using Shap.Interfaces.ViewModels;

    /// <summary>
    /// Interaction logic for VehicleDataWindow.xaml
    /// </summary>
    public partial class VehicleDataWindow : Window
  {
        /// <summary>
        /// Initialises a new instance of the <see cref="VehicleDataWindow"/> class.
        /// </summary>
        public VehicleDataWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Set up the graph to show distance progress. 
        /// </summary>
        /// <param name="journeysList">current set of distances.</param>
        public void SetUpGraph(List<IJourneyViewModel> journeysList)
        {
            DateTime startTime = journeysList[journeysList.Count - 1].JnyId.Date.AddMonths(-1);
            DateTime lastTime = journeysList[0].JnyId.Date.AddMonths(1);
            double lastTimeInSeconds = lastTime.Subtract(startTime).TotalSeconds;

            this.chart1.Series[0].Points.AddXY(0, 0);
            MilesChains distance = new MilesChains();

            for (int index = journeysList.Count - 1; index >= 0; --index)
            {
                double time = journeysList[index].JnyId.Date.Subtract(startTime).TotalSeconds;
                this.chart1.Series[0].Points.AddXY(time, distance.Miles);

                distance += journeysList[index].Distance;

                this.chart1.Series[0].Points.AddXY(time, distance.Miles);
            }

            this.chart1.Series[0].Points.AddXY(lastTimeInSeconds, distance.Miles);
        }
    }
}
