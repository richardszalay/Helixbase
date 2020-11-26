using Sitecore.Xdb.ReferenceData.Client;
using Sitecore.Xdb.ReferenceData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helixbase.Foundation.XConnect.Repositories
{
    public interface IReferenceRepository
    {
        Task<Definition<string, string>> GetDefinition(string definitionTypeName,
            string moniker, string referenceDataHost, string thumbprint);
        Task<Definition<string, string>> CreateDefinition(string definitionTypeName, string moniker,
            string definitionName, string referenceDataHost, string thumbprint);
        ReferenceDataHttpClient BuildClient(string referenceDataHost, string thumbprint);
    }
}
