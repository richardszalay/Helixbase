using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Helixbase.Feature.Sitecron.SitecronSettings;

namespace Helixbase.Feature.Sitecron.Core.Jobs
{
    public class InheritingSitecronJobProvider : DatabaseJobProvider
    {
        public InheritingSitecronJobProvider(ISitecronJobValidator jobValidator) : base(jobValidator)
        {
        }

        protected override IEnumerable<Item> GetJobItems()
        {
            return GetJobItems(SitecronConstants.ItemIds.RootFolderID);
        }

        protected override IEnumerable<Item> GetJobItems(ID rootFolderId)
        {
            var folder = ContextDatabase?.GetItem(rootFolderId);
            if (folder == null)
            {
                return Enumerable.Empty<Item>();
            }

            return folder.Axes.GetDescendants().Where(i =>
                TemplateManager.IsFieldPartOfTemplate(SitecronConstants.SiteCronFieldIds.CronExpression, i));
        }
    }
}