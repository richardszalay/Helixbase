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
    public class InteractionRepository : IInteractionRepository
	{
		private readonly ILogRepository _LogRepository;
		public InteractionRepository(ILogRepository logRepository)
		{
			_LogRepository = logRepository;

		}

        public async Task<Interaction> GetInteraction(XConnectClientConfiguration cfg,
			
			Guid contactId, Guid interactionId)
        {
			_LogRepository.Debug("Retrieving interaction for contact with ID: '{0}'. Interaction ID: '{1}'.",
				contactId, interactionId);
			using (var client = new XConnectClient(cfg))
			{
				try
				{
				 
					var interactionReference = new InteractionReference(contactId, interactionId);

					//Define the facets that should be expanded
					var expandOptions = new InteractionExpandOptions(new string[] { IpInfo.DefaultFacetKey });
					expandOptions.Contact = new RelatedContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey });
					expandOptions.Expand<WebVisit>();

					//Query the client for the interaction
					var interaction = await client.GetAsync<Interaction>(interactionReference, expandOptions);

	 

					return interaction;
				}
				catch (XdbExecutionException ex)
				{
					// Deal with exception
					_LogRepository.Error("Exception creating interaction", ex);
				}
			}

			return null;
		}

        public async Task<Interaction> RegisterGoalInteraction(XConnectClientConfiguration cfg, 
			Contact contact, string channelId, string goalId, 
			IpInfo ipInfo, DateTime? eventTime = null)
		{ 
			var interactionTimestamp = eventTime.HasValue ? eventTime.Value : DateTime.UtcNow;
			_LogRepository.Debug("Creating interaction for contact with ID: '{0}'. Channel: '{1}'. Goal: '{2}'. Time: '{3}'", contact.Id, channelId, goalId, interactionTimestamp);

			using (var client = new XConnectClient(cfg))
			{
				try
				{
			 
					var interaction = new Interaction(contact, InteractionInitiator.Brand, Guid.Parse(channelId), "");

				 
					var xConnectEvent = new Goal(Guid.Parse(goalId), interactionTimestamp);
					interaction.Events.Add(xConnectEvent);

		 
					if (ipInfo != null)
					{
						client.SetFacet<IpInfo>(interaction, IpInfo.DefaultFacetKey, ipInfo);
					}

					//Add the interaction to the client
					client.AddInteraction(interaction);

					//Submit the interaction
					await client.SubmitAsync();

		 

					return interaction;
				}
				catch (XdbExecutionException ex)
				{
		 
					_LogRepository.Error("Exception creating interaction", ex);
				}
			}

			return null;

		}

		public async Task<Interaction> RegisterOutcomeInteraction(XConnectClientConfiguration cfg,
			Contact contact, string channelId, string outcomeId,string currentcyCode,decimal monetaryValue, 
		 DateTime? eventTime)
		{
			var interactionTimestamp = eventTime.HasValue ? eventTime.Value : DateTime.UtcNow;
			_LogRepository.Debug("Creating interaction for contact with ID: '{0}'. Channel: '{1}'. Goal: '{2}'. Time: '{3}'", contact.Id, channelId, outcomeId, interactionTimestamp);

			using (var client = new XConnectClient(cfg))
			{
				try
				{

					var interaction = new Interaction(contact, InteractionInitiator.Brand, Guid.Parse(channelId), "");


					var outcome = new Outcome(Guid.Parse(outcomeId), interactionTimestamp, currentcyCode, monetaryValue);
					interaction.Events.Add(outcome);


					 
 
					client.AddInteraction(interaction);

					//Submit the interaction
					await client.SubmitAsync();



					return interaction;
				}
				catch (XdbExecutionException ex)
				{

					_LogRepository.Error("Exception creating interaction", ex);
				}
			}

			return null;
		}

        public async Task<Interaction> RegisterPageviewInteraction(XConnectClientConfiguration cfg,
			Contact contact, string channelId,string goalId, Guid itemId,int itemVersion,string language,TimeSpan duration,
			
			WebVisit webVisit, DateTime? eventTime)
        {
			var interactionTimestamp = eventTime.HasValue ? eventTime.Value : DateTime.UtcNow;
			_LogRepository.Debug("RegisterPageviewInteraction for contact with ID: '{0}'. Channel: '{1}'. itemId: '{2}'. Time: '{3}'", contact.Id, channelId, itemId, interactionTimestamp);

			using (var client = new XConnectClient(cfg))
			{
				try
				{

					var webInteraction = new Interaction(contact, InteractionInitiator.Brand, Guid.Parse(channelId), "");


					PageViewEvent pageView = new PageViewEvent(interactionTimestamp, 
						 itemId, itemVersion, language);
					pageView.ItemLanguage = language;
					pageView.Duration = duration;

					webInteraction.Events.Add(pageView);

					Goal goal = new Goal(Guid.Parse(goalId), interactionTimestamp);

					goal.ParentEventId = pageView.Id;

					webInteraction.Events.Add(goal);
		 
					client.SetWebVisit(webInteraction, webVisit);
					client.AddInteraction(webInteraction);

					//Submit the interaction
					await client.SubmitAsync();



					return webInteraction;
				}
				catch (XdbExecutionException ex)
				{

					_LogRepository.Error("Exception creating interaction", ex);
				}
			}

			return null;
		}

        public async Task<List<Interaction>> SearchInteractionsByDate(XConnectClientConfiguration cfg, 
			DateTime startDate, DateTime endDate)
        {
			_LogRepository.Debug("Searching for all interactions between {0} and {1}", startDate, endDate);
			using (var client = new XConnectClient(cfg))
			{
				try
				{
					 
					var queryable = client.Interactions.Where(i => i.StartDateTime >= startDate && i.EndDateTime <= endDate);

				 
					var enumerable = await queryable.GetBatchEnumerator(10);

					//Output the data that was retrieved and collect into a list to return
					var interactions = new List<Interaction>();
					while (await enumerable.MoveNext())
					{
						foreach (var interaction in enumerable.Current)
						{
							interactions.Add(interaction);
			 
						}
					}

					//Return the set of interactions to the calling application
					return interactions;
				}
				catch (XdbExecutionException ex)
				{
					// Deal with exception
					_LogRepository.Error("Exception executing search operation", ex);
				}
			}

			return null;
		}
    }
}
