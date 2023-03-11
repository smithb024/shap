namespace Shap
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Input;
    using Interfaces.Input;
    using Interfaces.Io;
    using Interfaces.StationDetails;
    using Interfaces.Stats;
    using Locations.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Shap.Interfaces;
    using Shap.Interfaces.Locations.Model;
    using Shap.Interfaces.Locations.ViewModels;
    using Shap.Io;
    using Shap.Locations.Model;
    using StationDetails;
    using Stats;

    /// <summary>
    /// Factory class, used to set up dependency injection
    /// </summary>
    public static class IocFactory
    {
        /// <summary>
        /// Setup IOC.
        /// </summary>
        public static void Setup()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IIoControllers, IoControllers>()
                .AddSingleton<IFirstExampleManager, FirstExampleManager>()
                .AddSingleton<ILocationManager, LocationManager>()
                .AddSingleton<ILocationAnalyser, LocationAnalyser>()
                .AddSingleton<ILocationsIndexViewModel, LocationsIndexViewModel>()
                .AddSingleton<IInputFormViewModel, InputFormViewModel>()
                .AddSingleton<IMileageDetailsViewModel, MileageDetailsViewModel>()
                .AddSingleton<IEditMileageViewModel, EditMileageViewModel>()
                .AddSingleton<IMainWindowViewModel, MainWindowViewModel>()
                .BuildServiceProvider());
        }
    }
}