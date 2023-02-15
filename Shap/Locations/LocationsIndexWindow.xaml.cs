namespace Shap.Locations
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Shap.Interfaces.Locations;
    using System.Windows;

    /// <summary>
    /// Interaction logic for LocationsIndexWindow.xaml
    /// </summary>
    public partial class LocationsIndexWindow : Window
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LocationsIndexWindow"/> class.
        /// </summary>
        public LocationsIndexWindow()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<ILocationsIndexViewModel>();
        }
    }
}