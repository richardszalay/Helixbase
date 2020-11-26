using Helixbase.Foundation.XConnect.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Helixbase.Foundation.XConnect.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ILogRepository, LogRepository>();
            serviceCollection.AddTransient<IContactRepository, ContactRepository>();
            serviceCollection.AddTransient<IInteractionRepository, InteractionRepository>();
          serviceCollection.AddTransient<IReferenceRepository,ReferenceRepository>();
            serviceCollection.AddTransient<IConfigurationBuilder, ConfigurationBuilder>();
        }
    }
}