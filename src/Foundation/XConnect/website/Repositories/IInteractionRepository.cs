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
    public interface IInteractionRepository
    {
        Task<Interaction> RegisterGoalInteraction(XConnectClientConfiguration cfg,
            Contact contact,
            string channelId, string goalId,
            IpInfo ipInfo, DateTime? eventTime = null);

        Task<Interaction> GetInteraction(XConnectClientConfiguration cfg,
            Guid contactId, Guid interactionId);

        Task<List<Interaction>> SearchInteractionsByDate(XConnectClientConfiguration cfg,
            DateTime startDate, DateTime endDate);


    }
}
