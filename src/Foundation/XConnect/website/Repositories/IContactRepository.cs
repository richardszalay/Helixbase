using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helixbase.Foundation.XConnect.Repositories
{
   public interface IContactRepository
    {
        Task<ContactIdentifier> CreateContact(XConnectClientConfiguration cfg,string source,
            string identifierId, PersonalInformation entity);
        Task<Contact> GetContact(XConnectClientConfiguration cfg, string source,string identifierId);
        Task<Contact> GetContactWithInteractions(XConnectClientConfiguration cfg, string source, string identifierId, DateTime? interactionStartTime, DateTime? interactionEndTime);
        Task<Contact> DeleteContact(XConnectClientConfiguration cfg, string source, string identifierId);
        Task<List<Contact>> GetMultipleContacts(XConnectClientConfiguration cfg, List<Guid> contactIds);
        Task<Contact> UpdateContact(XConnectClientConfiguration cfg, string source,

            string identifierId, PersonalInformation updatedPersonalInformation);
    }
}
