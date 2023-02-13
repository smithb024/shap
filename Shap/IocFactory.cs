namespace Shap
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Input;
    using Interfaces.Input;
    using Interfaces.StationDetails;
    using Interfaces.Stats;
    using Microsoft.Extensions.DependencyInjection;
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
                .AddSingleton<IFirstExampleManager, FirstExampleManager>()
                .AddSingleton<IInputFormViewModel, InputFormViewModel>()
                .AddSingleton<IMileageDetailsViewModel, MileageDetailsViewModel>()
                .AddSingleton<IEditMileageViewModel, EditMileageViewModel>()
                .BuildServiceProvider());
        }
    }
}