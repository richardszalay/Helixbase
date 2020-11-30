using System.Linq;
using Helixbase.Feature.Analytics.Models;
using Helixbase.Foundation.XConnect;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Items;
using Helixbase.Foundation.XConnect.Repositories;
using System.Threading.Tasks;
using Sitecore.XConnect;

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
       public async  Task<ContactIdentifier> IdentifyContact(ContactDetails  data)
        {
            var cfg = _configurationBuilder.GetClientConfiguration(
              "https://helixbasexconnect.dev.local",
              "https://helixbasexconnect.dev.local",
              "https://helixbasexconnect.dev.local",
              "368779A310FBF4EDDC259092C5EDD0377B637EF9");

            await cfg.InitializeAsync();
            var existingContact = _contactRepository.GetContact(cfg, data.identifierSource, data.identifier);

            ContactIdentifier identifier;
            if (existingContact == null)
            {
                identifier= await _contactRepository.CreateContact(cfg, data.identifierSource, data.identifier,
                     new Sitecore.XConnect.Collection.Model.PersonalInformation()
                     {
                         FirstName = data.firstName,
                         LastName = data.lastName,
                      
                     }); ;
            }
            else
            {
              var contact =  await _contactRepository.UpdateContact(cfg, data.identifierSource, data.identifier
                    , new Sitecore.XConnect.Collection.Model.PersonalInformation()
                    {
                        FirstName = data.firstName,
                        LastName = data.lastName,
                    });
                identifier = contact.Identifiers.FirstOrDefault(
                    t=>t.Source.Equals(data.identifierSource) &&
                    t.Identifier.Equals(data.identifier)
                    );
            }
            return identifier;
        }


    }
}