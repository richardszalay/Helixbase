using System.Linq;
using Helixbase.Feature.Analytics.Models;
using Helixbase.Foundation.XConnect;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Items;
using Helixbase.Foundation.XConnect.Repositories;
namespace Helixbase.Feature.Analytics.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogRepository _logRepository;
        private readonly IInteractionRepository _interactionRepository;
        private readonly IReferenceRepository _referenceRepository;
        private readonly IConfigurationBuilder _configurationBuilder;
 
 

        public AnalyticsService(IContactRepository contactRepository,
            ILogRepository logRepository, IInteractionRepository interactionRepository,
            IReferenceRepository referenceRepository, IConfigurationBuilder configurationBuilder
            )
        {
            _contactRepository = contactRepository;
            _logRepository = logRepository;
            _interactionRepository = interactionRepository;
            _referenceRepository = referenceRepository;
            _configurationBuilder = configurationBuilder;
        }
 

 
    }
}