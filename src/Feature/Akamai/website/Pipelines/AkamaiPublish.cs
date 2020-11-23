#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Helixbase.Feature.Akamai.Services;
using Helixbase.Foundation.Logging.Repositories;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.PublishItem;
using SC = Sitecore.Configuration;
#endregion

 
namespace Helixbase.Feature.Akamai.Pipelines.HTTPRequestBegin
{
    public class AkamaiPublish : PublishItemProcessor
    {
        private readonly static string _rootPath = SC.Settings.GetSetting("Redirect.RootPath");
        private IAkamaiService _AkamaiService;
        private readonly ILogRepository _logRepository;
         
        public AkamaiPublish(IAkamaiService akamaiService, ILogRepository logRepository)
        {
            _AkamaiService = akamaiService;
            _logRepository = logRepository;
        }
        public override void Process(PublishItemContext context)
        {
            PublishItemResult result = context.Result;
            Item sourceItem = context.PublishHelper.GetItemToPublish(context.ItemId);
            if (result != null && sourceItem != null)
            {

                if ((int)result.Operation == (int)PublishOperation.Updated && result.Explanation.IndexOf("ja-JP_1") > 0 && result.Explanation.IndexOf("published") > 0)
                {
                    var sourcePath = sourceItem.Paths.Path;

                    if (sourcePath.Contains(_rootPath))
                    {
                        _AkamaiService.InvalidateByUrl(sourcePath.Replace(_rootPath, ""));
                    }
                }

            }

        }
    }
}
 