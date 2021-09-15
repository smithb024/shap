namespace Shap.Units
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Forms.DataVisualization.Charting;
    using NynaeveLib.Types;
    using Shap.Interfaces.ViewModels;
    using Shap.Styles;

    /// <summary>
    /// Interaction logic for VehicleDataWindow.xaml
    /// </summary>
    public partial class VehicleDataWindow : Window
    {
        /// <summary>
        /// Time of the first point on the graph.
        /// </summary>
        private DateTime graphStartTime; 

        /// <summary>
        /// Initialises a new instance of the <see cref="VehicleDataWindow"/> class.
        /// </summary>
        public VehicleDataWindow()
        {
            this.InitializeComponent();

            //customize the X-Axis to properly display Time 
            chart1.Customize += Chart1Customise;
            graphStartTime = DateTime.Now;
        }

        /// <summary>
        /// Set up the graph to show distance progress. 
        /// </summary>
        /// <param name="journeysList">current set of distances.</param>
        public void SetUpGraph(List<IJourneyViewModel> journeysList)
        {
            this.chart1.ChartAreas[0].AxisX.LineColor = ColoursDictionary.AxisColour;
            this.chart1.ChartAreas[0].AxisY.LineColor = ColoursDictionary.AxisColour;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = ColoursDictionary.AxisColour;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = ColoursDictionary.AxisColour;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = ColoursDictionary.AxisColour;

            if (journeysList == null || journeysList.Count == 0)
            {
                return;
            }

            DateTime startTime = journeysList[journeysList.Count - 1].JnyId.Date.AddMonths(-1);
            this.graphStartTime = startTime;
            DateTime lastTime = journeysList[0].JnyId.Date.AddMonths(1);
            double lastTimeInSeconds = lastTime.Subtract(startTime).TotalSeconds;

            this.chart1.Series[0].Points.Clear();
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

        /// <summary>
        /// Customise the x axis to display the year.
        /// </summary>
        /// <param name="sender">sender graph</param>
        /// <param name="e">event arguments</param>
        private void Chart1Customise(object sender, EventArgs e)
        {
            CustomLabelsCollection xAxisLabels = ((Chart)sender).ChartAreas[0].AxisX.CustomLabels;
            for (int cnt = 0; cnt < xAxisLabels.Count; cnt++)
            {
                TimeSpan ts = TimeSpan.FromSeconds(double.Parse(xAxisLabels[cnt].Text));
                DateTime graphTime = this.graphStartTime + ts;
                xAxisLabels[cnt].Text = graphTime.Year.ToString("0000");
            }
        }
    }
}
