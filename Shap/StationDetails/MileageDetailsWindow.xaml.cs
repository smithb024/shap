namespace Shap.StationDetails
{
    using System.Windows;
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