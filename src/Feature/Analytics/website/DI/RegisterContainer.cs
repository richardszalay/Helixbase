 
using Helixbase.Feature.Analytics.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Helixbase.Feature.Analytics.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
        
          serviceCollection.AddTransient<IAnalyticsService, AnalyticsService>();
 
        }
    }
}