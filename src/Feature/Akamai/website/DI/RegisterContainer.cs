using Helixbase.Feature.Akamai.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helixbase.Feature.Akamai.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAkamaiService, AkamaiService>();
         
        }
    }
}