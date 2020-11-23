using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Helixbase.Foundation.CloudStorage.Pipelines.MediaProcessor
{
    public class MediaProcessorArgs : PipelineArgs
    {
        public IEnumerable<Item> UploadedItems { get; set; }
    }
}
