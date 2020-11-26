using Microsoft.Extensions.Logging;
using Sitecore.Xdb.ReferenceData.Client;
using Sitecore.Xdb.ReferenceData.Core;
using Sitecore.Xdb.ReferenceData.Core.Converter;
using System;
using System.Globalization;
using System.Threading.Tasks;


namespace Helixbase.Foundation.XConnect.Repositories
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly ILogRepository _LogRepository;
      private readonly IConfigurationBuilder _ConfigurationBuilder;
        public ReferenceRepository(ILogRepository logRepository,
            IConfigurationBuilder configurationBuilder)
        {
            _LogRepository = logRepository;
            _ConfigurationBuilder = configurationBuilder;

        }
        public ReferenceDataHttpClient BuildClient(string referenceDataHost, string thumbprint)
        {
            var converter = new DefinitionEnvelopeJsonConverter();

            var handlers = _ConfigurationBuilder.GetReferenceDataHandlers(thumbprint);

            var logger = new Logger<ReferenceDataHttpClient>(new LoggerFactory());

            return new ReferenceDataHttpClient(converter,
                new Uri(referenceDataHost),
                handlers,
                logger);
        }

        public async Task<Definition<string, string>> CreateDefinition(string definitionTypeName, 
            string moniker, string definitionName, 
            string referenceDataHost, string thumbprint)
        {
            using (var client = BuildClient(referenceDataHost, thumbprint))
            {
                try
                {
          
                    var definitionType = await client.EnsureDefinitionTypeAsync(definitionTypeName);

        
                    var definitionKey = new DefinitionKey(moniker, definitionType, 1);
                    var definition = new Definition<string, string>(definitionKey)
                    {
                        IsActive = true,
                        CommonData = "Some common data",
                        CultureData =
                        {
                            { new CultureInfo("en"), definitionName }
                        }
                    };

                    //Culture invariant data - has culture, but culture is unknown
                    definition.CultureData[CultureInfo.InvariantCulture] = definitionName;

                    //Create the definition
                    var result = await client.SaveAsync(definition);

   

                    return definition;
                }
                catch (Exception ex)
                {
                    _LogRepository.Error("Error creating definition", ex);
                }
            }

            return null;
        }

        public async Task<Definition<string, string>> GetDefinition(string definitionTypeName, string moniker, string referenceDataHost, string thumbprint)
        {
            using (var client = BuildClient(referenceDataHost, thumbprint))
            {
                try
                {
                    //Build the search criteria for the definition retrieval
                    var definitionType = await client.EnsureDefinitionTypeAsync(definitionTypeName);
                    var criteria = new DefinitionCriteria(moniker, definitionType);

                    var definition = await client.GetDefinitionAsync<string, string>(criteria, false);

                    if (definition != null)
                    {
                        _LogRepository.Debug("Definition found: {0}", definition.Key.Moniker);
                    }
                    else
                    {
                        _LogRepository.Debug("No definition found with moniker: {0} of type {1}", moniker, definitionTypeName);
                    }

                    return definition;
                }
                catch (Exception ex)
                {
                    _LogRepository.Error("Error retrieving definition", ex);
                }
            }

            return null;
        }
    }
}
