using System.Collections.Generic;
using Helixbase.Foundation.CloudStorage.Pipelines.MediaProcessor;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Jobs;
namespace Helixbase.Foundation.CloudStorage.Helpers
{
    public class PipelineHelper
    {
        /// <summary>
        /// Creates and starts a Sitecore Job to run as a long running background task
        /// </summary>
        /// <param name="args">The UploadArgs</param>
        public void StartMediaProcessorJob(IEnumerable<Item> uploadedItems)
        {
            var args = new MediaProcessorArgs { UploadedItems = uploadedItems };
            var jobOptions = new DefaultJobOptions("CloudMediaProcessor", "MediaProcessing",
                                                          Sitecore.Context.Site.Name,
                                                          this, "RunMediaProcessor", new object[] { args });
            Sitecore.Jobs.JobManager.Start(jobOptions);
        }

        /// <summary>
        /// Calls Custom Pipeline with the supplied args
        /// </summary>
        /// <param name="args">The UploadArgs</param>
        public void RunMediaProcessor(MediaProcessorArgs args)
        {
            CorePipeline.Run("cloud.MediaProcessor", args);
        }
    }
}
