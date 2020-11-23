using System.Collections.Generic;
using Helixbase.Foundation.CloudStorage.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Attach;

namespace Helixbase.Foundation.CloudStorage.Pipelines.AttachFile
{
    /// <summary>
    /// Kicks off process to start media upload job
    /// </summary>
    public class ProcessMedia
    {
        public void Process(AttachArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (!args.MediaItem.FileBased)
                return;

            var helper = new PipelineHelper();
            helper.StartMediaProcessorJob(new List<Item> { args.MediaItem });
        }
    }
}
