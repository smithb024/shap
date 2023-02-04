namespace Shap
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Interfaces.StationDetails;
    using Microsoft.Extensions.DependencyInjection;
    using StationDetails;

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
                .AddSingleton<IMileageDetailsViewModel, MileageDetailsViewModel>()
                .BuildServiceProvider());
        }
    }
}