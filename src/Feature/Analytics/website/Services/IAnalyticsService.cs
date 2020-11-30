using Helixbase.Feature.Analytics.Models;
using Sitecore.XConnect;
using System.Threading.Tasks;

namespace Helixbase.Feature.Analytics.Services
{
    public interface IAnalyticsService
    {
        Task<ContactIdentifier> IdentifyContact(ContactDetails data);
    }
}
