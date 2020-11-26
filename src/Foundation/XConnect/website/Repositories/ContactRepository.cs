 
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helixbase.Foundation.XConnect.Extensions;
namespace Helixbase.Foundation.XConnect.Repositories
{
    public class ContactRepository : IContactRepository
    {
		private  readonly ILogRepository _LogRepository;
		public ContactRepository(ILogRepository logRepository)
        {
			_LogRepository = logRepository;

		}
		public async Task<ContactIdentifier> CreateContact(XConnectClientConfiguration cfg,string source,
			string identifierId, PersonalInformation entity)
		{ 
			var identifier = new ContactIdentifier(source, identifierId, ContactIdentifierType.Known);
			var identifiers = new ContactIdentifier[] { identifier };


			_LogRepository.Debug("Creating Contact with Identifier:" + identifier.Identifier);

			// Initialize a client using the validated configuration
			using (var client = new XConnectClient(cfg))
			{
				try
				{
				 
					var knownContact = new Contact(identifiers);
 
					var personalInfoFacet = new PersonalInformation() { FirstName = entity.FirstName, LastName = entity.LastName,
						JobTitle = entity.JobTitle, Birthdate = entity.Birthdate
					};
				 
					client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

				 
					client.AddContact(knownContact);

		 
					await client.SubmitAsync();
					_LogRepository.Debug("Submit Contact with Identifier:" + identifier.Identifier);

				}
				catch (XdbExecutionException ex)
				{
				 
					_LogRepository.Error("Exception creating contact", ex);
				}
			}

			return identifier;

		}

        public async Task<Contact> DeleteContact(XConnectClientConfiguration cfg, string identifierId)
		{
			_LogRepository.Debug("Deleting Contact with Identifier:" + identifierId);

 
 
			var existingContact = await GetContact(cfg, identifierId);
			if (existingContact != null)
			{
				using (var client = new XConnectClient(cfg))
				{
					try
					{
						//Add the delete operation onto the client for the specified contact and execute
						client.DeleteContact(existingContact);
						await client.SubmitAsync();

						_LogRepository.Debug(">> Contact successfully deleted.");
					}
					catch (XdbExecutionException ex)
					{
						_LogRepository.Error("Exception deleting the Contact", ex);
					}
				}
			}
			else
			{
				_LogRepository.Debug("WARNING: No Contact found with Identifier:" 
					+ identifierId + ". Cannot delete Contact.");
			}

			return existingContact;

		}

        public async Task<Contact> GetContact(XConnectClientConfiguration cfg, string identifierId)
        {
			return await GetContactWithInteractions(cfg, identifierId, null, null);
		}

        public async Task<Contact> GetContactWithInteractions(XConnectClientConfiguration cfg, 
			string identifierId, DateTime? interactionStartTime, DateTime? interactionEndTime)
        {

			Contact existingContact = null;

			_LogRepository.Debug("Retrieving Contact with Identifier:" + identifierId);

 
			using (var client = new XConnectClient(cfg))
			{
				try
				{
					var contactOptions = new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey });

			 
					if (interactionStartTime.HasValue || interactionEndTime.HasValue)
					{
						contactOptions.Interactions = new RelatedInteractionsExpandOptions(IpInfo.DefaultFacetKey)
						{
							StartDateTime = interactionStartTime,
							EndDateTime = interactionEndTime
						};
					}

					//Build up options for the query
					var reference = new IdentifiedContactReference("twitter", identifierId);
					// Get a known contact
					existingContact = await client.GetAsync<Contact>(reference, contactOptions);
					if (existingContact == null)
					{
						_LogRepository.Debug("No contact found with ID '{0}'", identifierId);
						return null;
					}

		 
				}
				catch (XdbExecutionException ex)
				{
					// Deal with exception
					_LogRepository.Error("Exception retrieving contact", ex);
				}
			}

			return existingContact;
		}

        public async Task<List<Contact>> GetMultipleContacts(XConnectClientConfiguration cfg,
			List<Guid> contactIds)
        {
			if (contactIds == null) { contactIds = new List<Guid>(); }
			_LogRepository.Debug("Getting Multiple Contacts: [{0}]", contactIds.Count);

			var contacts = new List<Contact>();

 
			using (var client = new XConnectClient(cfg))
			{
				try
				{
					//Configure the options to extract the personal information facet
					var contactOptions = new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey });

					//Get a list of reference objects for the contactIds provided
					var references = new List<IEntityReference<Contact>>();
					foreach (var contactId in contactIds)
					{
						references.Add(new ContactReference(contactId));
					}

					//Get all the matches
					var contactsResult = await client.GetAsync<Contact>(references, contactOptions);

					//Get the Contact objects from the results
					foreach (var result in contactsResult)
					{
						if (result.Exists)
						{
							contacts.Add(result.Entity);
						}
					}

 
					_LogRepository.Debug("> Retrieved {0} matching Contacts.", contacts.Count);
				}
				catch (XdbExecutionException ex)
				{
					// Deal with exception
					_LogRepository.Error("Exception retrieving contact", ex);
				}
			}

			return contacts;
		}

        public async Task<Contact> UpdateContact(XConnectClientConfiguration cfg, string identifierId, PersonalInformation updatedPersonalInformation)
        {
			_LogRepository.Debug("Updating personal information about Contact with Identifier:" + identifierId);

			//If no updated information was provided, we can shortcut exit and save the connections
			if (updatedPersonalInformation == null)
				return null;

			//Get the existing contact that we want to update
 
			var existingContact = await GetContact(cfg, identifierId);

			if (existingContact != null)
			{
				//Get the existing personal information that needs to be updated
				var personalInfoFacet = existingContact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
				if (personalInfoFacet != null)
				{
					//Check for any changes. No need to send updates if it is the same!
					bool hasFacetChanged = personalInfoFacet.HasChanged(updatedPersonalInformation);

					//If any change has occurred, make the update.
					if (hasFacetChanged)
					{
						//Update the current facet data with the new pieces
						personalInfoFacet.Update(updatedPersonalInformation);

						//Open a client connection and make the update
						using (var client = new XConnectClient(cfg))
						{
							try
							{
								client.SetFacet<PersonalInformation>(existingContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);
							}
							catch (XdbExecutionException ex)
							{
								_LogRepository.Error("Exception updating personal information", ex);
							}
						}
					}
				}
				else
				{
					//If there is no personal information facet, we need to send all the data
					using (var client = new XConnectClient(cfg))
					{
						try
						{
							client.SetFacet<PersonalInformation>(existingContact, PersonalInformation.DefaultFacetKey, updatedPersonalInformation);
						}
						catch (XdbExecutionException ex)
						{
							_LogRepository.Error("Exception creating personal information for the Contact", ex);
						}
					}

					//Update the current facet data with the new pieces
					personalInfoFacet = updatedPersonalInformation;
				}

				//Output information about the updated contact
				_LogRepository.Debug("Updated contact information:");
		 
			}
			else
			{
				_LogRepository.Debug("WARNING: No Contact found with Identifier:" + identifierId + ". Cannot update personal information.");
			}

			return existingContact;
		}
    }
}
