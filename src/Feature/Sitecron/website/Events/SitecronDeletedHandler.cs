using Sitecore.Data.Items;
using Sitecore.Events;
using Helixbase.Feature.Sitecron.SitecronSettings;
using System;
using Helixbase.Feature.Sitecron.Core.Quartz;

namespace Helixbase.Feature.Sitecron.Events
{
    public class SitecronDeletedHandler
    {
        public void OnItemDeleted(object sender, EventArgs args)
        {
            Item deletedItem = Event.ExtractParameter(args, 0) as Item;

            if (deletedItem != null && SitecronConstants.Templates.SitecronJobTemplateID == deletedItem.TemplateID) //matched Sitecron job template
            {
                ScheduleHelper scheduler = new ScheduleHelper();
                scheduler.InitializeScheduler();

            }
        }
    }
}