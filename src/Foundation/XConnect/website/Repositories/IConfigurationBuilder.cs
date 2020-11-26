using Sitecore.XConnect.Client;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helixbase.Foundation.XConnect.Repositories
{
    public interface IConfigurationBuilder
    {
        XConnectClientConfiguration GetClientConfiguration(string collectionHost, string searchHost, string configHost, string thumbprint);
        IEnumerable<IHttpClientHandlerModifier> GetReferenceDataHandlers(string thumbprint);
    }
}
