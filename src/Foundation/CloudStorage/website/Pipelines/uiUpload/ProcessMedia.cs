using Helixbase.Foundation.CloudStorage.Helpers;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Upload;

namespace Helixbase.Foundation.CloudStorage.Pipelines.uiUpload
{
    /// <summary>
    /// Processes file based media and calls custom pipeline to further process item
    /// </summary>
    public class ProcessMedia : UploadProcessor
    {
        public void Process(UploadArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.Destination == UploadDestination.File)
            {
                var helper = new PipelineHelper();
                helper.StartMediaProcessorJob(args.UploadedItems);
            }
        }
    }
}