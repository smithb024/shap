using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shap.StationDetails
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Interfaces.StationDetails;

    /// <summary>
    /// Interaction logic for MileageDetailsWindow.xaml
    /// </summary>
    public partial class MileageDetailsWindow : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MileageDetailsWindow"/> class.
        /// </summary>
        public MileageDetailsWindow()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<IMileageDetailsViewModel>();
        }
    }
}