namespace Shap.Units
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Interaction logic for VehicleDataWindow.xaml
    /// </summary>
    public partial class VehicleDataWindow : Window
  {
        public VehicleDataWindow()
        {
            this.InitializeComponent();
            this.SetupChartProperties();
        }

        private void SetupChartProperties()
        {
            IEnumerable<string> titles = new List<string>
            {
                "Point1",
                "Point2",
                "Point3",
                "Point4",
                "Point5",
            };
            IEnumerable<double> values = new List<double>
            {
                10,
                5,
                15,
                28,
                2
            };

            this.chart1.Series[0].Points.DataBindXY(titles, values);
            this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        }
    }
}
